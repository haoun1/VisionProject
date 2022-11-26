#pragma once
#include "pch.h"
#include <opencv2/opencv.hpp>
#include <ipp.h>
#include <math.h>
#include <omp.h>

using namespace cv;
class IP
{
public:
	static void Threshold(BYTE* pSrc, BYTE* pDst, int nW, int nH, bool bDark, int nThresh);
};
