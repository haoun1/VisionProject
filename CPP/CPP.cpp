#include "pch.h"
#include "CPP.h"


#pragma warning(disable: 4101)
#pragma warning(disable: 4244)
#pragma warning(disable: 4838)
#pragma warning(disable: 6297)
#pragma warning(disable: 26451)


void Tool::Custom_Threshold(BYTE* source, BYTE* destination, long long nW, long long nH, int nThresh, bool bDark)
{
	for (int i = 0; i < nH; i++)
	{
		for (int j = 0; j < nW; j++)
		{
			if (bDark)
			{
				if (source[i * nW + j] > nThresh) destination[i * nW + j] = 0;
				else destination[i * nW + j] = 255;
			}
			else
			{
				if (source[i * nW + j] > nThresh) destination[i * nW + j] = 255;
				else destination[i * nW + j] = 0;
			}
		}
	}
}


//Gaussian Filter
void Tool::CV2_GaussianFilter(BYTE* pSrc, BYTE* pDst, int nW, int nH, int nSize, double dSigma)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	GaussianBlur(imgSrc, imgDst, Size(nSize, nSize), dSigma, dSigma);
}

//GaussianFilter without opencv
void Tool::AI_GaussianFilter(BYTE* pSrc, BYTE* pDst, int nW, int nH, int nSize, double dSigma)
{
	double* pGaussian = new double[nSize * nSize];
	double dSum = 0;
	for (int i = 0; i < nSize; i++)
	{
		for (int j = 0; j < nSize; j++)
		{
			pGaussian[i * nSize + j] = exp(-(pow(i - nSize / 2, 2) + pow(j - nSize / 2, 2)) / (2 * pow(dSigma, 2)));
			dSum += pGaussian[i * nSize + j];
		}
	}
	for (int i = 0; i < nSize * nSize; i++)
	{
		pGaussian[i] /= dSum;
	}
	for (int i = 0; i < nH; i++)
	{
		for (int j = 0; j < nW; j++)
		{
			double dSum = 0;
			for (int k = 0; k < nSize; k++)
			{
				for (int l = 0; l < nSize; l++)
				{
					if (i + k - nSize / 2 < 0 || i + k - nSize / 2 >= nH || j + l - nSize / 2 < 0 || j + l - nSize / 2 >= nW) continue;
					dSum += pSrc[(i + k - nSize / 2) * nW + j + l - nSize / 2] * pGaussian[k * nSize + l];
				}
			}
			pDst[i * nW + j] = (BYTE)dSum;
		}
	}
	delete[] pGaussian;
}

void Tool::Custom_erode(BYTE* source, BYTE* destination, long long nW, long long nH, int Kernel_Size)
{
	bool bCheck;
	for (int i = 0; i < nH; i++)
	{
		for (int j = 0; j < nW; j++)
		{
			if (KernelCheck_erode(source, i * nW + j, Kernel_Size, nW, nH))
			{
				destination[i * nW + j] = 0;
			}
			else
			{
				destination[i * nW + j] = 255;
			}
		}
	}
}
void Tool::Custom_dilate(BYTE* source, BYTE* destination, long long nW, long long nH, int Kernel_Size)
{
	bool bCheck;
	for (int i = 0; i < nH; i++)
	{
		for (int j = 0; j < nW; j++)
		{
			if (KernelCheck_dilate(source, i * nW + j, Kernel_Size, nW, nH))
			{
				destination[i * nW + j] = 255;
			}
			else
			{
				destination[i * nW + j] = 0;
			}
		}
	}
}

void Tool::CV2_Labeling(BYTE* pSrc, BYTE* pBin, std::vector<LabeledData>& vtOutLabeled, int nW, int nH, bool bDark)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgBin = Mat(nH, nW, CV_8UC1, pBin);
	Mat imgMask;
	cv::bitwise_and(imgSrc, imgBin, imgMask);
	std::vector<std::vector<Point>> contours; //outputArray of Arrays
	std::vector<Vec4i> hierarchy; //outputArray
								  //4개의 int 0=다음외곽선번호 1=이전 2=자식 3=부모
	cv::findContours(imgBin, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, Point(0, 0));
	Rect bounding_rect;
	for (int i = 0; i < contours.size(); i++)
	{
		if (hierarchy[i][3] != -1) continue;//부모 Contour가 있으면 pass
		bounding_rect = cv::boundingRect(contours[i]);

		Mat defectROI = imgMask(bounding_rect);
		Mat defectMask = imgBin(bounding_rect);

		double min, max;
		cv::minMaxIdx(defectROI, &min, &max, NULL, NULL, defectMask);
		LabeledData data;
		data.bound = { bounding_rect.x, bounding_rect.y, bounding_rect.x + bounding_rect.width, bounding_rect.y + bounding_rect.height };
		data.width = bounding_rect.width;
		data.height = bounding_rect.height;
		data.centerX = bounding_rect.x + (double)bounding_rect.width / 2;
		data.centerY = bounding_rect.y + (double)bounding_rect.height / 2;
		data.area = (bounding_rect.width > bounding_rect.height) ? bounding_rect.width : bounding_rect.height;
		data.value = (bDark) ? min : max;
		vtOutLabeled.push_back(data);
	}
}

bool KernelCheck_dilate(BYTE* source, long long idx, int kSize, long long width, long long height)
{
	long long idx_H;
	long long idx_WH;
	for (int i = 0; i < kSize; i++)
	{
		idx_H = idx + i * width;
		for (int j = 0; j < kSize; j++)
		{
			idx_WH = idx_H + j;
			if (0 <= idx_WH && idx_WH < width * height)
			{
				if (source[idx_WH] == 0) return false;
			}
			if (j > 0)
			{
				idx_WH = idx_H - j;
				if (0 <= idx_WH && idx_WH < width * height)
				{
					if (source[idx_WH] == 0) return false;
				}
			}
		}
		if (i > 0)
		{
			idx_H = idx - i * width;
			for (int j = 0; j < kSize; j++)
			{
				idx_WH = idx_H + j;
				if (0 <= idx_WH && idx_WH < width * height)
				{
					if (source[idx_WH] == 0) return false;
				}
				if (j > 0)
				{
					idx_WH = idx_H - j;
					if (0 <= idx_WH && idx_WH < width * height)
					{
						if (source[idx_WH] == 0) return false;
					}
				}
			}
		}
	}
	return true;
}

bool KernelCheck_erode(BYTE* source, long long idx, int kSize, long long width, long long height)
{
	long long idx_H;
	long long idx_WH;
	for (int i = 0; i < kSize; i++)
	{
		idx_H = idx + i * width;
		for (int j = 0; j < kSize; j++)
		{
			idx_WH = idx_H + j;
			if (0 <= idx_WH && idx_WH < width * height)
			{
				if (source[idx_WH] == 255) return false;
			}
			if (j > 0)
			{
				idx_WH = idx_H - j;
				if (0 <= idx_WH && idx_WH < width * height)
				{
					if (source[idx_WH] == 255) return false;
				}
			}
		}
		if (i > 0)
		{
			idx_H = idx - i * width;
			for (int j = 0; j < kSize; j++)
			{
				idx_WH = idx_H + j;
				if (0 <= idx_WH && idx_WH < width * height)
				{
					if (source[idx_WH] == 255) return false;
				}
				if (j > 0)
				{
					idx_WH = idx_H - j;
					if (0 <= idx_WH && idx_WH < width * height)
					{
						if (source[idx_WH] == 255) return false;
					}
				}
			}
		}
	}
	return true;
}

//라플라스 필터
void Tool::CV2_Laplacian(BYTE* pSrc, BYTE* pDst, int nW, int nH)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	cv::Laplacian(imgSrc, imgDst, CV_8UC1, 3);
}

//OpenCV 사용안하는 라플라스 필터
void Tool::AI_Laplacian(BYTE* pSrc, BYTE* pDst, int nW, int nH)
{
	int nSize = nW * nH;
	BYTE* pTemp = new BYTE[nSize];
	memcpy(pTemp, pSrc, nSize);
	int nIdx;
	int nIdx_H;
	int nIdx_WH;
	for (int i = 1; i < nH - 1; i++)
	{
		nIdx_H = i * nW;
		for (int j = 1; j < nW - 1; j++)
		{
			nIdx = nIdx_H + j;
			nIdx_WH = nIdx - nW - 1;
			int nSum = 0;
			nSum += pSrc[nIdx_WH] * -1;
			nIdx_WH++;
			nSum += pSrc[nIdx_WH] * -1;
			nIdx_WH += 2;
			nSum += pSrc[nIdx_WH] * -1;
			nIdx_WH += nW - 2;
			nSum += pSrc[nIdx_WH] * -1;
			nIdx_WH += 2;
			nSum += pSrc[nIdx_WH] * -1;
			nIdx_WH += nW - 2;
			nSum += pSrc[nIdx_WH] * -1;
			nIdx_WH++;
			nSum += pSrc[nIdx_WH] * -1;
			nIdx_WH += 2;
			nSum += pSrc[nIdx_WH] * -1;
			nSum += pSrc[nIdx] * 8;
			pTemp[nIdx] = (BYTE)nSum;
		}
	}
	memcpy(pDst, pTemp, nSize);
	delete[] pTemp;
}

//Template Matching
void Tool::AI_TemplateMatching(BYTE* pSrc, BYTE* pDst, int nW, int nH, BYTE* pTemp, int nTempW, int nTempH, int method)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	Mat imgTemp = Mat(nTempH, nTempW, CV_8UC1, pTemp);
	Mat imgResult;
	matchTemplate(imgSrc, imgTemp, imgResult, method);
	normalize(imgResult, imgResult, 0, 1, NORM_MINMAX, -1, Mat());
	double minVal, maxVal;
	Point minLoc, maxLoc, matchLoc;
	minMaxLoc(imgResult, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
	matchLoc = maxLoc;
	rectangle(imgDst, matchLoc, Point(matchLoc.x + imgTemp.cols, matchLoc.y + imgTemp.rows), Scalar::all(0), 2, 8, 0);
	waitKey();
}

//Histogram equalization
void Tool::CV2_HistogramEqualization(BYTE* pSrc, BYTE* pDst, int nW, int nH)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	equalizeHist(imgSrc, imgDst);
}

//Histogram equalization without cv
void Tool::AI_HistogramEqualization(BYTE* pSrc, BYTE* pDst, int nW, int nH)
{
	int nHist[256] = { 0, };
	int nSumHist[256] = { 0, };
	int nSum = 0;
	int nSize = nW * nH;
	for (int i = 0; i < nSize; i++) {
		nHist[pSrc[i]]++;
	}
	for (int i = 0; i < 256; i++) {
		nSum += nHist[i];
		nSumHist[i] = nSum;
	}
	for (int i = 0; i < nSize; i++) {
		pDst[i] = (BYTE)(nSumHist[pSrc[i]] * 255 / nSize);
	}
}

//Otsu thresholding
void Tool::CV2_OtsuThresholding(BYTE* pSrc, BYTE* pDst, int nW, int nH)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	threshold(imgSrc, imgDst, 0, 255, THRESH_BINARY | THRESH_OTSU);
}

//Otsu thresholding without cv
void Tool::AI_OtsuThresholding(BYTE* pSrc, BYTE* pDst, int nW, int nH)
{
	int nHist[256] = { 0, };
	int nSumHist[256] = { 0, };
	int nSum = 0;
	int nSize = nW * nH;
	for (int i = 0; i < nSize; i++) {
		nHist[pSrc[i]]++;
	}
	for (int i = 0; i < 256; i++) {
		nSum += nHist[i];
		nSumHist[i] = nSum;
	}
	double dMax = 0;
	int nThreshold = 0;
	for (int i = 0; i < 256; i++) {
		double dW0 = (double)nSumHist[i] / nSize;
		double dW1 = 1 - dW0;
		double dU0 = 0;
		double dU1 = 0;
		for (int j = 0; j <= i; j++) {
			dU0 += (double)j * nHist[j] / nSumHist[i];
		}
		for (int j = i + 1; j < 256; j++) {
			dU1 += (double)j * nHist[j] / (nSize - nSumHist[i]);
		}
		double dSigma = dW0 * dW1 * (dU0 - dU1) * (dU0 - dU1);
		if (dSigma > dMax) {
			dMax = dSigma;
			nThreshold = i;
		}
	}
	for (int i = 0; i < nSize; i++) {
		if (pSrc[i] > nThreshold) {
			pDst[i] = 255;
		}
		else {
			pDst[i] = 0;
		}
	}
}

//FFT Low Pass Filtering
void Tool::CV2_FFTLowPassFiltering(BYTE* pSrc, BYTE* pDst, int nW, int nH, int nD0)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	Mat imgFFT;
	Mat planes[] = { Mat_<float>(imgSrc), Mat::zeros(imgSrc.size(), CV_32F) };

	merge(planes, 2, imgFFT);
	dft(imgFFT, imgFFT);
	split(imgFFT, planes);
	magnitude(planes[0], planes[1], planes[0]);
	Mat magI = planes[0];
	magI += Scalar::all(1);
	log(magI, magI);
	magI = magI(Rect(0, 0, magI.cols & -2, magI.rows & -2));
	int cx = magI.cols / 2;
	int cy = magI.rows / 2;
	Mat q0(magI, Rect(0, 0, cx, cy));
	Mat q1(magI, Rect(cx, 0, cx, cy));
	Mat q2(magI, Rect(0, cy, cx, cy));
	Mat q3(magI, Rect(cx, cy, cx, cy));
	Mat tmp;
	q0.copyTo(tmp);
	q3.copyTo(q0);
	tmp.copyTo(q3);
	q1.copyTo(tmp);
	q2.copyTo(q1);
	tmp.copyTo(q2);
	normalize(magI, magI, 0, 1, NORM_MINMAX);
	Mat imgFilter = Mat::zeros(imgFFT.size(), imgFFT.type());
	circle(imgFilter, Point(cx, cy), nD0, Scalar::all(1), -1, 8, 0);
	multiply(imgFFT, imgFilter, imgFFT);
	idft(imgFFT, imgDst, DFT_SCALE | DFT_REAL_OUTPUT);
	imshow("imgSrc", imgSrc);
	imshow("Spectrum", magI);
	imshow("imgDst", imgDst);
}















