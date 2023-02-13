#pragma once
#include "pch.h"
#include "windows.h"
#include "memoryapi.h"
#include <stdio.h>
#include <list>
#include "..\CPP\\CPP.h"
#include <iostream>

using namespace System::Collections::Generic;

namespace CLR
{
	public ref class DataLabel
	{
	public:
		double centerX;
		double centerY;
		int boundTop;
		int boundBottom;
		int boundLeft;
		int boundRight;
		double width;
		double height;
		double area;
		double value;
	};

	public ref class CustomCV
	{
	public:
		static void Custom_Threshold(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int nThresh, bool bDark);
		static void Custom_erode(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size);
		static void Custom_dilate(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size);
		static array<DataLabel^>^ CV2_Labeling(array<BYTE>^ source, array<BYTE>^ mask, int width, int height, bool bDark);
	};
	public ref class CustomAlgo
	{

	};
}