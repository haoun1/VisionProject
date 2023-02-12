#pragma once
#include <string>
#include "pch.h"
#include "windows.h"
#include "memoryapi.h"
#include <stdio.h>
#include <list>
#include <iostream>
#include "..\\CustomCV\\CustomCV.h"

using namespace System::Collections::Generic;

namespace Custom_CLR 
{
	public ref class CustomCV
	{
	public:
		static void Custom_Threshold(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int nThresh, bool bDark);
		static void Custom_erode(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size);
		static void Custom_dilate(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size);
	};
	public ref class CustomAlgo
	{

	};
}
