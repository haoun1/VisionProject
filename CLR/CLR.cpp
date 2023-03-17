#include "pch.h"
#include "CLR.h"
#include <msclr\marshal_cppstd.h>
#pragma warning(disable: 4244)
#pragma warning(disable: 4267)
#pragma warning(disable: 4793)

namespace CLR
{
	void CustomCV::Custom_Threshold(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int nThresh, bool bDark)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::Custom_Threshold(src, dst, nW, nH, nThresh, bDark);
	}
	void CustomCV::Custom_erode(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::Custom_erode(src, dst, nW, nH, Kernel_Size);
	}
	void CustomCV::Custom_dilate(array<BYTE>^ source, array<BYTE>^ destination, long long nW, long long nH, int Kernel_Size)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::Custom_dilate(src, dst, nW, nH, Kernel_Size);
	}

	void CustomCV::CV2_HistogramEqualization(array<BYTE>^ source, array<BYTE>^ destination, int width, int height)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::CV2_HistogramEqualization(src, dst, width, height);
	}

	void CustomCV::AI_HistogramEqualization(array<BYTE>^ source, array<BYTE>^ destination, int width, int height)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::AI_HistogramEqualization(src, dst, width, height);
	}

	void CustomCV::CV2_OtsuThresholding(array<BYTE>^ source, array<BYTE>^ destination, int width, int height)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::CV2_OtsuThresholding(src, dst, width, height);
	}

	void CustomCV::AI_OtsuThresholding(array<BYTE>^ source, array<BYTE>^ destination, int width, int height)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::AI_OtsuThresholding(src, dst, width, height);
	}

	void CustomCV::CV2_GaussianFilter(array<BYTE>^ source, array<BYTE>^ destination, int width, int height, int size, double sigma)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::CV2_GaussianFilter(src, dst, width, height, size, sigma);
	}

	void CustomCV::AI_GaussianFilter(array<BYTE>^ source, array<BYTE>^ destination, int width, int height, int size, double sigma)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::AI_GaussianFilter(src, dst, width, height, size, sigma);
	}

	void CustomCV::CV2_Laplacian(array<BYTE>^ source, array<BYTE>^ destination, int width, int height)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::CV2_Laplacian(src, dst, width, height);
	}

	void CustomCV::AI_Laplacian(array<BYTE>^ source, array<BYTE>^ destination, int width, int height)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::AI_Laplacian(src, dst, width, height);
	}

	void CustomCV::CV2_FFTLowPassFiltering(array<BYTE>^ source, array<BYTE>^ destination, int width, int height, int radius)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		Tool::CV2_FFTLowPassFiltering(src, dst, width, height, radius);
	}

	void CustomCV::AI_TemplateMatching(array<BYTE>^ source, array<BYTE>^ destination, int width, int height, array<BYTE>^ templateImg, int templateWidth, int templateHeight, int method)
	{
		pin_ptr<BYTE> src = &source[0];
		pin_ptr<BYTE> dst = &destination[0];
		pin_ptr<BYTE> temp = &templateImg[0];
		Tool::AI_TemplateMatching(src, dst, width, height, temp, templateWidth, templateHeight, method);
	}

	array<DataLabel^>^ CustomCV::CV2_Labeling(array<BYTE>^ source, array<BYTE>^ mask, int width, int height, bool bDark)
	{
		pin_ptr<BYTE> pin_src = &source[0];
		pin_ptr<BYTE> pin_mask = &mask[0];

		std::vector<LabeledData> vec;

		Tool::CV2_Labeling(pin_src, pin_mask, vec, width, height, bDark);

		array<DataLabel^>^ res = gcnew array<DataLabel^>(vec.size());

		for (int i = 0; i < res->Length; i++)
		{
			res[i]->area = vec[i].area;
			res[i]->boundTop = vec[i].bound.top;
			res[i]->boundBottom = vec[i].bound.bottom;
			res[i]->boundLeft = vec[i].bound.bottom;
			res[i]->boundRight = vec[i].bound.right;
			res[i]->width = vec[i].width;
			res[i]->height = vec[i].height;
			res[i]->value = vec[i].value;
			res[i]->centerX = vec[i].centerX;
			res[i]->centerY = vec[i].centerY;
		}

		return res;
	}
}