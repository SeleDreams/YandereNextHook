#include <Windows.h>
#include "MonoHooker.h"
#include "MessageManager.h"

#pragma region Proxy
struct version_dll {
	HMODULE dll;
	FARPROC oGetFileVersionInfoA;
	FARPROC oGetFileVersionInfoByHandle;
	FARPROC oGetFileVersionInfoExA;
	FARPROC oGetFileVersionInfoExW;
	FARPROC oGetFileVersionInfoSizeA;
	FARPROC oGetFileVersionInfoSizeExA;
	FARPROC oGetFileVersionInfoSizeExW;
	FARPROC oGetFileVersionInfoSizeW;
	FARPROC oGetFileVersionInfoW;
	FARPROC oVerFindFileA;
	FARPROC oVerFindFileW;
	FARPROC oVerInstallFileA;
	FARPROC oVerInstallFileW;
	FARPROC oVerLanguageNameA;
	FARPROC oVerLanguageNameW;
	FARPROC oVerQueryValueA;
	FARPROC oVerQueryValueW;
} version;

extern "C" {
	FARPROC PA = 0;
	int runASM();

	void fGetFileVersionInfoA() { PA = version.oGetFileVersionInfoA; runASM(); }
	void fGetFileVersionInfoByHandle() { PA = version.oGetFileVersionInfoByHandle; runASM(); }
	void fGetFileVersionInfoExA() { PA = version.oGetFileVersionInfoExA; runASM(); }
	void fGetFileVersionInfoExW() { PA = version.oGetFileVersionInfoExW; runASM(); }
	void fGetFileVersionInfoSizeA() { PA = version.oGetFileVersionInfoSizeA; runASM(); }
	void fGetFileVersionInfoSizeExA() { PA = version.oGetFileVersionInfoSizeExA; runASM(); }
	void fGetFileVersionInfoSizeExW() { PA = version.oGetFileVersionInfoSizeExW; runASM(); }
	void fGetFileVersionInfoSizeW() { PA = version.oGetFileVersionInfoSizeW; runASM(); }
	void fGetFileVersionInfoW() { PA = version.oGetFileVersionInfoW; runASM(); }
	void fVerFindFileA() { PA = version.oVerFindFileA; runASM(); }
	void fVerFindFileW() { PA = version.oVerFindFileW; runASM(); }
	void fVerInstallFileA() { PA = version.oVerInstallFileA; runASM(); }
	void fVerInstallFileW() { PA = version.oVerInstallFileW; runASM(); }
	void fVerLanguageNameA() { PA = version.oVerLanguageNameA; runASM(); }
	void fVerLanguageNameW() { PA = version.oVerLanguageNameW; runASM(); }
	void fVerQueryValueA() { PA = version.oVerQueryValueA; runASM(); }
	void fVerQueryValueW() { PA = version.oVerQueryValueW; runASM(); }
}

void setupFunctions() {
	version.oGetFileVersionInfoA = GetProcAddress(version.dll, "GetFileVersionInfoA");
	version.oGetFileVersionInfoByHandle = GetProcAddress(version.dll, "GetFileVersionInfoByHandle");
	version.oGetFileVersionInfoExA = GetProcAddress(version.dll, "GetFileVersionInfoExA");
	version.oGetFileVersionInfoExW = GetProcAddress(version.dll, "GetFileVersionInfoExW");
	version.oGetFileVersionInfoSizeA = GetProcAddress(version.dll, "GetFileVersionInfoSizeA");
	version.oGetFileVersionInfoSizeExA = GetProcAddress(version.dll, "GetFileVersionInfoSizeExA");
	version.oGetFileVersionInfoSizeExW = GetProcAddress(version.dll, "GetFileVersionInfoSizeExW");
	version.oGetFileVersionInfoSizeW = GetProcAddress(version.dll, "GetFileVersionInfoSizeW");
	version.oGetFileVersionInfoW = GetProcAddress(version.dll, "GetFileVersionInfoW");
	version.oVerFindFileA = GetProcAddress(version.dll, "VerFindFileA");
	version.oVerFindFileW = GetProcAddress(version.dll, "VerFindFileW");
	version.oVerInstallFileA = GetProcAddress(version.dll, "VerInstallFileA");
	version.oVerInstallFileW = GetProcAddress(version.dll, "VerInstallFileW");
	version.oVerLanguageNameA = GetProcAddress(version.dll, "VerLanguageNameA");
	version.oVerLanguageNameW = GetProcAddress(version.dll, "VerLanguageNameW");
	version.oVerQueryValueA = GetProcAddress(version.dll, "VerQueryValueA");
	version.oVerQueryValueW = GetProcAddress(version.dll, "VerQueryValueW");
}
#pragma endregion

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
	switch (ul_reason_for_call) {
	case DLL_PROCESS_ATTACH:
		//char path[1024];
		//GetWindowsDirectoryA(path, MAX_PATH);
		//MessageManager::DisplayMessage(path);
		// Example: "\\System32\\version.dll"
		//strcat_s(path, "\\System32\\version.dll");
		version.dll = LoadLibraryA("C:\\Windows\\System32\\version.dll");
		setupFunctions();
		MonoHooker::Hook();
		break;
	case DLL_PROCESS_DETACH:
		FreeLibrary(version.dll);
		break;
	}
	return 1;
}
