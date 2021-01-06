#include "Mono.h"
#include"MessageManager.h"
void Mono::PopulateMonoFunctions() {
	Module = GetModuleHandleA("mono-2.0-bdwgc.dll");
	if (Module == NULL) {
		
		MessageManager::DisplayError("The mono module was null!");
	}
	jit_init_version = (void* (*)(const char*, const char*))GetProcAddress(Module, "mono_jit_init_version");
	thread_current = (void* (*) ()) GetProcAddress(Module, "mono_thread_current");
	thread_set_main = (void(*)(void*))GetProcAddress(Module, "mono_thread_set_main");
	domain_assembly_open = (void* (*)(void*, const char*))GetProcAddress(Module, "mono_domain_assembly_open");
	assembly_get_image = (void* (*)(void*))GetProcAddress(Module, "mono_assembly_get_image");
	method_desc_search_in_image = (void* (*)(void*, void*))GetProcAddress(Module, "mono_method_desc_search_in_image");
	runtime_invoke = (void* (*)(void*, void*, void**, void**))GetProcAddress(Module, "mono_runtime_invoke");
	assembly_getrootdir = (const char* (*)())GetProcAddress(Module, "mono_assembly_getrootdir");
	method_desc_new = (void* (*)(const char*, bool))GetProcAddress(Module, "mono_method_desc_new");
}