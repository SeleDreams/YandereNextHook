#ifndef MONOHOOKER
#define MONOHOOKER
#include <Windows.h>
#include <detours/detours.h>
#include "Mono.h"

class MonoHooker
{
public:
	static void Hook();
	

	static void StartMod();
	static void HookGetProc();

	static bool isHooked() { return m_hooked; }
	static void SetHooked(bool hookStatus) { m_hooked = hookStatus; };
	
	static const char* GetApplicationDirectory() { return m_application_directory; }

	static Mono Mono;

	// custom mono functions
	static void* custom_mono_jit_init_version(const char* AppName, const char* version);

	static FARPROC(_stdcall* Original_GetProcAddress)(HMODULE, LPCSTR);
	static FARPROC _stdcall HookedProcAdress(HMODULE hModule, LPCSTR filename);
	static const char* HookLib;
	

private:
	// Fields
	static bool m_hooked;
	static const char* m_application_directory;
	
};


#endif