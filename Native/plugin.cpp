//
//  plugin.cpp
//  Plugin
//
#include "plugin.hpp"

#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <iostream>

using namespace std;

#define DllExport __attribute__((visibility("default")))

#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif

extern "C" {

struct TwoStrings{
    char string1[1000];
    char string2[1000];
    char concatenated[1000];
};

void __stdcall concatenateStrings(/*[in]*/ TwoStrings* test_struct)
{
  printf("test_struct.string1 : [%s].\r\n", test_struct->string1);
    char result[1000]="";
    strncat(result,test_struct->string1, 100);
    strncat(result,test_struct->string2, 100);
    strncpy(test_struct->concatenated, result, 100);
  
}

}
