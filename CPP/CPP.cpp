#include "pch.h"
#include "CPP.h"


#pragma warning(disable: 4101)
#pragma warning(disable: 4244)
#pragma warning(disable: 4838)
#pragma warning(disable: 6297)
#pragma warning(disable: 26451)


void IP::Threshold(BYTE* pSrc, BYTE* pDst, int nW, int nH, bool bDark, int nThresh)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	
	if (bDark)
		cv::threshold(imgSrc, imgDst, nThresh, 255, CV_THRESH_BINARY_INV);
	else
		cv::threshold(imgSrc, imgDst, nThresh, 255, CV_THRESH_BINARY);
}