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