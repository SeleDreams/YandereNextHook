#include "MonoHooker.h"
#include "include/detours.h"
#include <ole2.h>
#include <Windows.h>
#include <objidlbase.h>
#include <string>
#include "Mono.h"
#include "MessageManager.h"

using namespace std;

FARPROC(_stdcall* MonoHooker::Original_GetProcAddress)(HMODULE, LPCSTR) = nullptr;
const char* MonoHooker::HookLib;
bool MonoHooker::m_hooked;
Mono MonoHooker::Mono;

void MonoHooker::Hook() {
	HookLib = "Hook";
	HookGetProc();
}

PVOID functionAddress;

void MonoHooker::HookGetProc() {
	HMODULE kernelModule = GetModuleHandleA("Kernel32.dll");
	 functionAddress = GetProcAddress(kernelModule, "GetProcAddress");
	DetourTransactionBegin();
	PVOID realTarget, realDetour;
	PDETOUR_TRAMPOLINE realTrampoline;
	long result = DetourAttachEx(&functionAddress, HookedProcAdress, &realTrampoline, &realTarget, &realDetour);
	DetourTransactionCommit();
	Original_GetProcAddress = (FARPROC(_stdcall*)(HMODULE, LPCSTR))realTrampoline;
}

void MonoHooker::StartMod() {
	DetourTransactionBegin();
	DetourDetach(&functionAddress, HookedProcAdress);
	DetourTransactionCommit();
	char buffer[255];
	GetCurrentDirectoryA(sizeof(buffer), buffer);
	string realPath = string(buffer) + "\\" + HookLib + ".dll";

	void* monoAssembly = Mono.domain_assembly_open(Mono.MonoDomain, realPath.c_str());
	if (monoAssembly != nullptr) {
		void* monoImage = Mono.assembly_get_image(monoAssembly);
		void* monoMethodDesc = Mono.method_desc_new((string(HookLib) + ".EntryPoint:Hooked()").c_str(), true);
		void* monoMethod = Mono.method_desc_search_in_image(monoMethodDesc, monoImage);
		void* result = Mono.runtime_invoke(monoMethod, nullptr, nullptr, nullptr);
	}
	else {
		MessageManager::DisplayError((realPath + " Doesn't exist or was incorrectly loaded ! The game will start normally !").c_str());
		ExitProcess(1);
	}
}

void* MonoHooker::custom_mono_jit_init_version(const char* AppName, const char* version) {
	Mono.PopulateMonoFunctions();
	Mono.MonoDomain = Mono.jit_init_version(AppName, version);
	const char* path = Mono.assembly_getrootdir();
	Mono.thread_set_main(Mono.thread_current());
	StartMod();
	return Mono.MonoDomain;
}

FARPROC _stdcall MonoHooker::HookedProcAdress(HMODULE hModule, LPCSTR filename)
{
	if (!m_hooked && string(filename) == string("mono_jit_init_version")) {
		m_hooked = true;
		return (FARPROC)MonoHooker::custom_mono_jit_init_version;
	}
	return Original_GetProcAddress(hModule, filename);
}
