// CustomCV.cpp : 정적 라이브러리를 위한 함수를 정의합니다.
//

#include "pch.h"
#include "CustomCV.h"
#include <math.h>
#include <omp.h>
#include <stdio.h>
#include <Windows.h>

bool KernelCheck(BYTE* source, int idx, int kSize, int width, int height);

void Tool::Custom_Threshold(BYTE* source, BYTE* destination, int nW, int nH, int nThresh, bool bDark)
{
	for(int i = 0 ; i < nH ; i++)
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
void Tool::Custom_erode(BYTE* source, BYTE* destination, int nW, int nH, int Kernel_Size)
{
	bool bCheck;
	for (int i = 0; i < nH; i++)
	{
		for (int j = 0; j < nW; j++)
		{
			if (KernelCheck(source, i * nW + j, Kernel_Size, nW, nH))
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
bool KernelCheck(BYTE* source,int idx , int kSize, int width, int height)
{
	int idx_H;
	int idx_WH;
	for (int i = 0; i < kSize; i++)
	{
		idx_H = idx + i*width;
		for (int j = 0; j < kSize; j++)
		{
			idx_WH = idx_H + j;
			if(0 <= idx_WH < width * height)
			{
				if (source[idx_WH] == 0) return false;
			}
			if (j > 0)
			{
				idx_WH = idx_H - j;
				if (0 <= idx_WH < width * height)
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
				if (0 <= idx_WH < width * height)
				{
					if (source[idx_WH] == 0) return false;
				}
				if (j > 0)
				{
					idx_WH = idx_H - j;
					if (0 <= idx_WH < width * height)
					{
						if (source[idx_WH] == 0) return false;
					}
				}
			}
		}
	}
	return true;
}