#pragma once
#include "pch.h"
#include <opencv2/opencv.hpp>
#include <ipp.h>
#include <math.h>
#include <omp.h>
#include "TypeDefines.h"

using namespace cv;
using namespace cv;

class Tool
{
public:
	static void Custom_Threshold(BYTE* source, BYTE* destination, long long nW, long long nH, int nThresh, bool bDark);
	static void Custom_erode(BYTE* source, BYTE* destination, long long nW, long long nH, int Kernel_Size);
	static void Custom_dilate(BYTE* source, BYTE* destination, long long nW, long long nH, int Kernel_Size);
	static void CV2_Labeling(BYTE* pSrc, BYTE* pBin, std::vector<LabeledData>& vtOutLabeled, int nW, int nH, bool bDark);
	static void CV2_GaussianFilter(BYTE* pSrc, BYTE* pDst, int nW, int nH, int nSize, double dSigma);
	static void AI_GaussianFilter(BYTE* pSrc, BYTE* pDst, int nW, int nH, int nSize, double dSigma);
	static void CV2_Laplacian(BYTE* pSrc, BYTE* pDst, int nW, int nH);
	static void AI_Laplacian(BYTE* pSrc, BYTE* pDst, int nW, int nH);
	static void AI_DFT(BYTE* pSrc, BYTE* pDst, int nW, int nH, double R);
	static void AI_HPF(BYTE* pSrc, BYTE* pDst, int nW, int nH, double R);
	static void AI_TemplateMatching(BYTE* pSrc, BYTE* pDst, int nW, int nH, BYTE* pTemp, int nTempW, int nTempH,int method);
};
bool KernelCheck_erode(BYTE* source, long long idx, int kSize, long long width, long long height);
bool KernelCheck_dilate(BYTE* source, long long idx, int kSize, long long width, long long height);
