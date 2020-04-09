#pragma once
#include <Windows.h>

class Mono
{
public:

	void* (*jit_init_version)(const char*, const char*) = nullptr;
	void* (*thread_current)() = nullptr ;
	void(*thread_set_main)(void*) = nullptr;
	void* (*domain_assembly_open)(void*, const char*) = nullptr ;
	void* (*assembly_get_image)(void*) = nullptr ;
	void* (*method_desc_search_in_image)(void*, void*) = nullptr;
	void* (*runtime_invoke)(void* method, void* obj, void** params, void** exc) = nullptr ;
	const char* (*assembly_getrootdir)() = nullptr;
	void* (*method_desc_new)(const char*, bool) = nullptr;

	void PopulateMonoFunctions();

	void* MonoDomain = nullptr;
	HMODULE Module;
};

