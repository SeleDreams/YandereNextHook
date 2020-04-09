#pragma once
#include <Windows.h>
#include "include/detours.h"
#include "Mono.h"

class MonoHooker
{
public:
	static void Hook();
	static void DisplayMessage(const char* message);
	static void DisplayError(const char* message);


	static void HookMonoMethodDesc();

	static bool isHooked() { return m_hooked; }
	static void SetHooked(bool hookStatus) { m_hooked = hookStatus; };
	
	static const char* GetApplicationDirectory() { return m_application_directory; }

	static Mono Mono;

	// custom mono functions
	static void* custom_mono_jit_init_version(const char* AppName, const char* version);
	static void* custom_mono_method_desc_new(const char* method, bool useNamespace);
	static FARPROC(_stdcall* Original_GetProcAddress)(HMODULE, LPCSTR);
	static FARPROC _stdcall HookedProcAdress(HMODULE hModule, LPCSTR filename);
	
private:
	// Fields
	static bool m_hooked;
	static const char* m_application_directory;
	
};


