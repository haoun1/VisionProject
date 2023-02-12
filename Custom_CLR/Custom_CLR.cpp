#include "pch.h"

#include "Custom_CLR.h"

namespace Custom_CLR
{
	void CustomCV::Custom_Threshold(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int nThresh, bool bDark)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::Custom_Threshold(src, dst, nW, nH, nThresh, bDark);
	}
	void CustomCV::Custom_erode(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::Custom_erode(src, dst, nW, nH, Kernel_Size);
	}
	void CustomCV::Custom_dilate(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::Custom_dilate(src, dst, nW, nH, Kernel_Size);
	}
}