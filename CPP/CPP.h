#pragma once
#include <opencv2/opencv.hpp>
#include <ipp.h>
#include <math.h>
#include <omp.h>
#include "pch.h"

using namespace cv;
class IP
{
	static void Threshold(BYTE* pSrc, BYTE* pDst, int nW, int nH, bool bDark, int nThresh);
};
