using CLR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisionProject.Domain;

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



        public void CV2_Gaussian(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int roiW, int roiH)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_GaussianFilter(sourceArray, resultArray, roiW, roiH, 5, 10);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), RPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_GaussianFilter(sourceArray, resultArray, roiW, roiH, 20, 1);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), GPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_GaussianFilter(sourceArray, resultArray, roiW, roiH, 20, 1);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), BPtr + (i * (int)MemoryX), roiW);
                    break;
            }
        }

        public void CV2_Hequal(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int roiW, int roiH)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_HistogramEqualization(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), RPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_HistogramEqualization(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), GPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_HistogramEqualization(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), BPtr + (i * (int)MemoryX), roiW);
                    break;
            }
        }

        public void CV2_Otsu(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int roiW, int roiH)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_OtsuThresholding(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), RPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_OtsuThresholding(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), GPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_OtsuThresholding(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), BPtr + (i * (int)MemoryX), roiW);
                    break;
            }
        }

        public void CV2_Laplace(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int roiW, int roiH)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_Laplacian(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), RPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_Laplacian(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), GPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_Laplacian(sourceArray, resultArray, roiW, roiH);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), BPtr + (i * (int)MemoryX), roiW);
                    break;
            }
        }
        public void AI_FFT_LPF(ColorMode m_color, IntPtr RPtr, IntPtr GPtr, IntPtr BPtr, long MemoryX, long MemoryY, int roiW, int roiH)
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_FFTLowPassFiltering(sourceArray, resultArray, roiW, roiH, 200);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), RPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_FFTLowPassFiltering(sourceArray, resultArray, roiW, roiH, 50);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), GPtr + (i * (int)MemoryX), roiW);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[roiW * roiH];
                    resultArray = new byte[roiW * roiH];
                    for (int i = 0; i < roiH; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * roiW), roiW);
                    CustomCV.CV2_FFTLowPassFiltering(sourceArray, resultArray, roiW, roiH, 50);
                    for (int i = 0; i < roiH; i++) Marshal.Copy(resultArray, (i * roiW), BPtr + (i * (int)MemoryX), roiW);
                    break;
            }
        }
        public void SplitTileSave(IntPtr startPtr, string waferId, int memoryW, int memoryH, int tileW, int tileH)
        {
            List<Tile> tiles = SplitTile(startPtr, memoryW, memoryH, tileW, tileH, memoryW);
            Parallel.For(0, tiles.Count, i => {
                Tile tile = tiles[i];
                TileSaveBMP($@"D:\tiles\{waferId}${i}.bmp", tile);
            });
        }
        List<Tile> SplitTile(IntPtr startPtr, int memoryW, int memoryH, int tileW, int tileH, int stride)
        {
            List<Tile> tiles = new List<Tile>();
            int currentTileW;
            int currentTileH;
            IntPtr offsetPtr;
            for (int i = 0; i < memoryW; i += tileW)
            {
                currentTileW = Math.Min(tileW, memoryW - i); // 남은 너비 처리
                for (int j = 0; j < memoryH; j += tileH)
                {
                    offsetPtr = new IntPtr(startPtr.ToInt64() + (j * stride + i));
                    currentTileH = Math.Min(tileH, memoryH - j); // 남은 높이 처리
                    tiles.Add(new Tile(offsetPtr, i, j, currentTileW, currentTileH, 1, 1, stride));
                }
            }
            return tiles;
        }
        public string TileSaveBMP(string sFile, Tile tile)
        {
            string filePath = Path.GetDirectoryName(sFile);
            if (!string.IsNullOrEmpty(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            else return "filePath NullOrEmpty";

            FileStream fs = null;
            try
            {
                fs = new FileStream(sFile, FileMode.Create, FileAccess.Write);
            }
            catch (Exception)
            {
                return "Can't create FileStream";
            }

            BinaryWriter bw = null;
            try
            {
                bw = new BinaryWriter(fs);
            }
            catch (Exception)
            {
                fs.Close();
                return "Can't create BinaryWriter";
            }

            try
            {
                // Bitmap File Header
                if (WriteBitmapFileHeader(bw, tile.nByte, tile.width, tile.height) == false)
                    return "Occured error writing bitmap file header";

                // Bitmap Info Header
                if (WriteBitmapInfoHeader(bw, tile.width, tile.height, true, tile.nByte, tile.nCount) == false)
                    return "Occured error writing bitmap info header";

                // Palette (if it's 1byte gray image)
                if (tile.nByte == 1)
                    WritePalette(bw);

                // Pixel data
                int rowSize = (tile.width * tile.nByte + 3) & ~3;
                byte[] aBuf = new byte[(long)rowSize];
                IntPtr ptr = tile.ptr;
                long idx;
                if (ptr != IntPtr.Zero)
                {
                    if (tile.nByte == tile.nByte)
                    {
                        unsafe
                        {
                            byte* rowPtr;
                            byte* basePtr = (byte*)ptr.ToPointer();
                            Stream stream = bw.BaseStream;

                            for (int j = tile.height - 1; j >= 0; j--)
                            {
                                rowPtr = basePtr + (j * tile.stride * tile.nByte);

                                for (int i = 0; i < rowSize; i++)
                                {
                                    stream.WriteByte(rowPtr[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = tile.height - 1; j >= 0; j--)
                        {
                            Array.Clear(aBuf, 0, rowSize);

                            idx = (long)j * tile.width * tile.nByte;

                            Parallel.For(0, tile.width, new ParallelOptions { MaxDegreeOfParallelism = 12 }, (i) =>
                            {
                                unsafe
                                {
                                    byte* arrByte = (byte*)ptr.ToPointer();

                                    if (tile.nByte == 1)         // 2byte -> 1byte
                                    {
                                        aBuf[i] = arrByte[idx + i * tile.nByte + 0];
                                    }
                                    else if (tile.nByte == 2)    // 1byte -> 2byte
                                    {
                                        int val = arrByte[idx + i * tile.nByte];
                                        val = (int)((val / 255.0) * (Math.Pow(2, 8 * tile.nByte) - 1));
                                        aBuf[i * tile.nByte + 0] = (byte)((val & 0xFF00) >> 8);
                                        aBuf[i * tile.nByte + 1] = (byte)((val & 0x00FF));
                                    }
                                }
                            });

                            bw.Write(aBuf);
                        }
                    }
                }
                else
                    return "Cannot read addresss in MemoryData";
            }
            catch (Exception ex)
            {
                return "Occured error writing BMP file";
            }
            finally
            {
                bw.Close();
                fs.Close();
            }
            return "OK";
        }
        bool WriteBitmapFileHeader(BinaryWriter bw, int nByte, int width, int height)
        {
            if (bw == null)
                return false;

            int rowSize = (width * nByte + 3) & ~3;
            int paletteSize = (nByte == 1 ? (256 * 4) : 0);

            int size = 14 + 40 + paletteSize + rowSize * height;
            int offbit = 14 + 40 + (nByte == 1 ? (256 * 4) : 0);

            bw.Write(Convert.ToUInt16(0x4d42));                     // bfType;
            bw.Write(Convert.ToUInt32((uint)size));                 // bfSize
            bw.Write(Convert.ToUInt16(0));                          // bfReserved1
            bw.Write(Convert.ToUInt16(0));                          // bfReserved2
            bw.Write(Convert.ToUInt32(offbit));                     // bfOffbits

            return true;
        }
        bool WriteBitmapInfoHeader(BinaryWriter bw, int width, int height, bool isGrayScale, int nByte, int nCount)
        {
            if (bw == null)
                return false;

            int biBitCount;
            if (isGrayScale)
            {
                biBitCount = nByte * 8;
            }
            else
            {
                biBitCount = nByte * nCount * 8;
            }
            //int biBitCount = (isGrayScale ? nByte : p_nByte) * p_nCount * 8;

            bw.Write(Convert.ToUInt32(40));                         // biSize
            bw.Write(Convert.ToInt32(width));                       // biWidth
            bw.Write(Convert.ToInt32(height));                      // biHeight
            bw.Write(Convert.ToUInt16(1));                          // biPlanes
            bw.Write(Convert.ToUInt16(biBitCount));                  // biBitCount
            bw.Write(Convert.ToUInt32(0));                          // biCompression
            bw.Write(Convert.ToUInt32(0));                          // biSizeImage
            bw.Write(Convert.ToInt32(0));                           // biXPelsPerMeter
            bw.Write(Convert.ToInt32(0));                           // biYPelsPerMeter
            bw.Write(Convert.ToUInt32((isGrayScale == true) ? 256 : 0));   // biClrUsed
            bw.Write(Convert.ToUInt32((isGrayScale == true) ? 256 : 0));   // biClrImportant

            return true;
        }
        bool WritePalette(BinaryWriter bw)
        {
            if (bw == null)
                return false;

            for (int i = 0; i < 256; i++)
            {
                bw.Write(Convert.ToByte(i));
                bw.Write(Convert.ToByte(i));
                bw.Write(Convert.ToByte(i));
                bw.Write(Convert.ToByte(255));
            }

            return true;
        }
    }
}
