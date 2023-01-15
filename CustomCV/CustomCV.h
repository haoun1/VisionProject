#pragma once
#define WIN32_LEAN_AND_MEAN             // 거의 사용되지 않는 내용을 Windows 헤더에서 제외합니다.
#include "pch.h"
class Tool
{
public:
	static void Custom_Threshold(BYTE* source, BYTE* destination, int nW, int nH, int nThresh, bool bDark);
	static void Custom_erode(BYTE* source, BYTE* destination, int nW, int nH, int Kernel_Size);
};