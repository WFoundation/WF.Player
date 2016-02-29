
#include <winphone8.h>

int GetTempPath (size_t buffsize, char * buffer)
{
	return 0;
}

int GetTempFileName (const char * path, const char * prefix, int , const char * )
{
	return 0;
}

DWORD GetModuleFileNameA(void * handle, char * buffer, size_t buffsize)
{
	strcpy(buffer, "\\lua.exe");
	return strlen (buffer);
}

HMODULE LoadLibraryExA(const char * path, void * , unsigned int flags)
{
	return 0;
}


// CRT 
#include <stdio.h>

int system (const char * command)
{
	return 0;
}

char * getenv (const char * env)
{
	return NULL;
}

FILE * _popen (const char * pipename, const char *)
{
	return 0;
}

int  _pclose (void *pipe)
{
	return 0;
}
