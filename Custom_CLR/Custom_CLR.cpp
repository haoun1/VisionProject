#include "pch.h"

#include "Custom_CLR.h"

namespace Custom_CLR
{
	void CustomCV::Custom_Threshold(array<BYTE>^ source, array<BYTE>^ destination, int nW, int nH, int nThresh, bool bDark)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::Custom_Threshold(src, dst, nW, nH, nThresh, bDark);
	}
}