using CLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisionProject
{
    public class ImageProcess
    {

        public void Threshold(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_Threshold(sourceArray, resultArray, MemoryX, MemoryY, 100, false);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(GPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_Threshold(sourceArray, resultArray, MemoryX, MemoryY, 100, false);
                    Marshal.Copy(resultArray, 0, GPtr, resultArray.Length);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(BPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_Threshold(sourceArray, resultArray, MemoryX, MemoryY, 100, false);
                    Marshal.Copy(resultArray, 0, BPtr, resultArray.Length);
                    break;
                case ColorMode.Color:
                    sourceArray = new byte[MemoryX * MemoryY * 3];
                    resultArray = new byte[MemoryX * MemoryY * 3];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY * 3));
                    CustomCV.Custom_Threshold(sourceArray, resultArray, MemoryX * 3, MemoryY, 100, false);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
            }
        }

        public void erode(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_erode(sourceArray, resultArray, MemoryX, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(GPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_erode(sourceArray, resultArray, MemoryX, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, GPtr, resultArray.Length);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(BPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_erode(sourceArray, resultArray, MemoryX, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, BPtr, resultArray.Length);
                    break;
                case ColorMode.Color:
                    sourceArray = new byte[MemoryX * MemoryY * 3];
                    resultArray = new byte[MemoryX * MemoryY * 3];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY * 3));
                    CustomCV.Custom_erode(sourceArray, resultArray, MemoryX * 3, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
            }
        }

        public void dilate(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_dilate(sourceArray, resultArray, MemoryX, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(GPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_dilate(sourceArray, resultArray, MemoryX, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, GPtr, resultArray.Length);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MemoryX * MemoryY];
                    resultArray = new byte[MemoryX * MemoryY];
                    Marshal.Copy(BPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_dilate(sourceArray, resultArray, MemoryX, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, BPtr, resultArray.Length);
                    break;
                case ColorMode.Color:
                    sourceArray = new byte[MemoryX * MemoryY * 3];
                    resultArray = new byte[MemoryX * MemoryY * 3];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY * 3));
                    CustomCV.Custom_dilate(sourceArray, resultArray, MemoryX * 3, MemoryY, 3);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
            }
        }



        public void CV2_Gaussian(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int MapSizeX, int MapSizeY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_GaussianFilter(sourceArray, resultArray, MapSizeX, MapSizeY, 5, 10);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_GaussianFilter(sourceArray, resultArray, MapSizeX, MapSizeY, 20, 1);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_GaussianFilter(sourceArray, resultArray, MapSizeX, MapSizeY, 20, 1);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
        }

        public void CV2_Hequal(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int MapSizeX, int MapSizeY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_HistogramEqualization(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_HistogramEqualization(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_HistogramEqualization(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
        }

        public void CV2_Otsu(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int MapSizeX, int MapSizeY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_OtsuThresholding(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_OtsuThresholding(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_OtsuThresholding(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
        }

        public void CV2_Laplace(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int MapSizeX, int MapSizeY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_Laplacian(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_Laplacian(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_Laplacian(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
        }
        public void AI_FFT_LPF(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int MapSizeX, int MapSizeY)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_FFTLowPassFiltering(sourceArray, resultArray, MapSizeX, MapSizeY, 200);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_FFTLowPassFiltering(sourceArray, resultArray, MapSizeX, MapSizeY, 50);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_FFTLowPassFiltering(sourceArray, resultArray, MapSizeX, MapSizeY, 50);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
        }
    }
}
