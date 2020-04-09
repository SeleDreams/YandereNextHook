#include "MonoHooker.h"
#include "include/detours.h"
#include <ole2.h>
#include <Windows.h>
#include <objidlbase.h>
#include <string>
#include "Mono.h"

using namespace std;



FARPROC(_stdcall* MonoHooker::Original_GetProcAddress)(HMODULE, LPCSTR) = nullptr;

bool MonoHooker::m_hooked;
Mono MonoHooker::Mono;


void MonoHooker::DisplayError(const char* message) {
	MessageBoxA(NULL, message, "ERROR", MB_ICONERROR);
}

void MonoHooker::DisplayMessage(const char* message) {
	MessageBoxA(NULL, message, "Message", MB_ICONINFORMATION);
}

void MonoHooker::HookMonoMethodDesc() {
	HMODULE monoModule = GetModuleHandleA("mono.dll");
	PVOID functionAddress = GetProcAddress(monoModule, "mono_method_desc_new");
	DetourTransactionBegin();
	PVOID realTarget;
	PVOID realDetour;
	PDETOUR_TRAMPOLINE realTrampoline;
	long result = DetourAttachEx(&functionAddress, custom_mono_method_desc_new, &realTrampoline, &realTarget, &realDetour);
	DetourTransactionCommit();
	Mono.method_desc_new = (void* (*)(const char*, bool))realTrampoline;
}

void MonoHooker::Hook() {
	HMODULE kernelModule = GetModuleHandleA("Kernel32.dll");
	PVOID functionAddress = GetProcAddress(kernelModule, "GetProcAddress");
	DetourTransactionBegin();
	PVOID realTarget, realDetour;
	PDETOUR_TRAMPOLINE realTrampoline;
	long result = DetourAttachEx(&functionAddress, HookedProcAdress, &realTrampoline, &realTarget, &realDetour);
	DetourTransactionCommit();
	Original_GetProcAddress = (FARPROC(_stdcall*)(HMODULE, LPCSTR))realTrampoline;
}


void* MonoHooker::custom_mono_method_desc_new(const char* method, bool useNamespace) {
	char buffer[255];
	GetCurrentDirectoryA(sizeof(buffer), buffer);
	string realPath = string(buffer) + "\\" + "TestAssembly.dll";
	void* monoAssembly = Mono.domain_assembly_open(Mono.MonoDomain, realPath.c_str());
	if (monoAssembly != nullptr) {
		void* monoImage = Mono.assembly_get_image(monoAssembly);
		void* monoMethodDesc = Mono.method_desc_new("TestAssembly.EntryPoint:Hooked()", true);
		void* monoMethod = Mono.method_desc_search_in_image(monoMethodDesc, monoImage);
		void* result = Mono.runtime_invoke(monoMethod, nullptr, nullptr, nullptr);
	}
	else {
		DisplayError("Assembly Null !");
		ExitProcess(1);
	}

	return Mono.method_desc_new(method, useNamespace);
}

void* MonoHooker::custom_mono_jit_init_version(const char* AppName, const char* version) {
	Mono.PopulateMonoFunctions();
	Mono.MonoDomain = Mono.jit_init_version(AppName, version);
	const char* path = Mono.assembly_getrootdir();
	Mono.thread_set_main(Mono.thread_current());
	HookMonoMethodDesc();
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
