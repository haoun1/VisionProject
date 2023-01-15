#pragma once
#include <string>
#include "..\\CustomCV\\CustomCV.h"

namespace Custom_CLR 
{
	public ref class CustomCV
	{
	public:
		static void Custom_Threshold(array<BYTE>^ source, array<BYTE>^ destination,int nW, int nH, int nThresh, bool bDark);
		static void Custom_erode(array<BYTE>^ source, array<BYTE>^ destination, int nW, int nH, int Kernel_Size);
	};
	public ref class CustomAlgo
	{

	};
}
