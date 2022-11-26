#include "pch.h"
#include "CLR.h"
#include <msclr\marshal_cppstd.h>
#pragma warning(disable: 4244)
#pragma warning(disable: 4267)
#pragma warning(disable: 4793)

namespace CLR
{
	void CLR::CPP_Threshold(array<byte>^ pSrcImg, array<byte>^ pDstImg, int nMemW, int nMemH, bool bDark, int nThresh)
	{
		pin_ptr<byte> pSrc = &pSrcImg[0]; // pin: 주소값 고정
		pin_ptr<byte> pDst = &pDstImg[0];

		IP::Threshold(pSrc, pDst, nMemW, nMemH, bDark, nThresh);

		pSrc = nullptr;
		pDst = nullptr;
	}
}