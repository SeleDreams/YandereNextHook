#include "MessageManager.h"
#include <Windows.h>

void MessageManager::DisplayError(const char* message) {
	MessageBoxA(NULL, message, "ERROR", MB_ICONERROR);
}

void MessageManager::DisplayMessage(const char* message) {
	MessageBoxA(NULL, message, "Message", MB_ICONINFORMATION);
}