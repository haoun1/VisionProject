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
	int nSize2 = nSize / 2;
	double dSigma2 = dSigma * dSigma;
	double dSum = 0;
	double* pGaussian = new double[nSize * nSize];
	for (int i = 0; i < nSize; i++)
	{
		for (int j = 0; j < nSize; j++)
		{
			pGaussian[i * nSize + j] = exp(-(i * i + j * j) / (2 * dSigma2)) / (2 * CV_PI * dSigma2);
			dSum += pGaussian[i * nSize + j];
		}
	}
	for (int i = 0; i < nSize; i++)
	{
		for (int j = 0; j < nSize; j++)
		{
			pGaussian[i * nSize + j] /= dSum;
		}
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
					int nX = j + l - nSize2;
					int nY = i + k - nSize2;
					if (nX < 0) nX = 0;
					if (nX >= nW) nX = nW - 1;
					if (nY < 0) nY = 0;
					if (nY >= nH) nY = nH - 1;
					dSum += pGaussian[k * nSize + l] * pSrc[nY * nW + nX];
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


//HPF
void Tool::AI_HPF(BYTE* pSrc, BYTE* pDst, int nW, int nH, double R)
{
	// 입력 영상을 2차원 Mat 객체로 변환
	cv::Mat srcMat(nH, nW, CV_8UC1, pSrc);

	// 결과 영상을 2차원 Mat 객체로 변환
	cv::Mat dstMat(nH, nW, CV_8UC1, pDst);

	// 입력 영상을 푸리에 변환
	cv::Mat fftInput;
	srcMat.convertTo(fftInput, CV_32FC1, 1.0 / 255.0); // 정규화
	cv::dft(fftInput, fftInput, cv::DFT_COMPLEX_OUTPUT);

	// 주파수 영역 필터링
	cv::Mat filter(fftInput.size(), CV_32FC2);
	cv::Point center = cv::Point(fftInput.cols / 2, fftInput.rows / 2);
	double radius = R * std::min(center.x, center.y);
	for (int i = 0; i < filter.rows; ++i) {
		for (int j = 0; j < filter.cols; ++j) {
			double distance = cv::norm(cv::Point(j, i) - center);
			if (distance > radius) {
				filter.at<cv::Vec2f>(i, j) = cv::Vec2f(0.0, 0.0);
			}
			else {
				filter.at<cv::Vec2f>(i, j) = cv::Vec2f(1.0, 1.0);
			}
		}
	}
	cv::mulSpectrums(fftInput, filter, fftInput, 0);

	// 역푸리에 변환
	cv::idft(fftInput, fftInput, cv::DFT_REAL_OUTPUT | cv::DFT_SCALE);

	// 결과 영상 복사
	fftInput.convertTo(dstMat, CV_8UC1, 255.0);
}

void Tool::AI_DFT(BYTE* pSrc, BYTE* pDst, int nW, int nH, double R)
{
	Mat imgSrc = Mat(nH, nW, CV_8UC1, pSrc);
	Mat imgDst = Mat(nH, nW, CV_8UC1, pDst);
	Mat planes[] = { Mat_<float>(imgSrc), Mat::zeros(imgSrc.size(), CV_32F) };
	Mat complexI;
	merge(planes, 2, complexI);
	dft(complexI, complexI);
	split(complexI, planes);
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
	imshow("Input Image", imgSrc);
	imshow("spectrum magnitude", magI);
	waitKey();
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
	rectangle(imgSrc, matchLoc, Point(matchLoc.x + imgTemp.cols, matchLoc.y + imgTemp.rows), Scalar::all(0), 2, 8, 0);
	imshow("imgSrc", imgSrc);
	imshow("imgTemp", imgTemp);
	imshow("imgResult", imgResult);
	waitKey();
}













