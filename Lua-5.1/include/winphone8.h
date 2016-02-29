#ifndef __WINPHONE_8_COMPAT_API_H__
#define __WINPHONE_8_COMPAT_API_H__

// Win32 APIs
#include <windows.h>

int GetTempPath (size_t buffsize, char * buffer);
int GetTempFileName (const char * path, const char * prefix, int , const char * );
DWORD GetModuleFileNameA(void *, char *, size_t);

HMODULE LoadLibraryExA(const char * path, void * , unsigned int flags);


// CRT 
#include <stdio.h>

int system (const char * command);

char * getenv (const char * env);

 FILE * _popen (const char * pipename, const char *);

int  _pclose (void *pipe);

#endif // __WINPHONE_8_COMPAT_API_H__