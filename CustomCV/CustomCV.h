#pragma once
#define WIN32_LEAN_AND_MEAN             // 거의 사용되지 않는 내용을 Windows 헤더에서 제외합니다.
#include "pch.h"
#include "TypeDefines.h"
#include <opencv2/opencv.hpp>
#include <ipp.h>
#include <math.h>
#include <omp.h>

using namespace cv;

class Tool
{
public:
	static void Custom_Threshold(BYTE* source, BYTE* destination, long long nW, long long nH, int nThresh, bool bDark);
	static void Custom_erode(BYTE* source, BYTE* destination, long long nW, long long nH, int Kernel_Size);
	static void Custom_dilate(BYTE* source, BYTE* destination, long long nW, long long nH, int Kernel_Size);
	static void CV2_Labeling(BYTE* pSrc, BYTE* pBin, std::vector<LabeledData>& vtOutLabeled, int nW, int nH, bool bDark)
};
bool KernelCheck_erode(BYTE* source, long long idx, int kSize, long long width, long long height);
bool KernelCheck_dilate(BYTE* source, long long idx, int kSize, long long width, long long height);