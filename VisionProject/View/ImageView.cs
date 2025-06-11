using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CLR;
using System.Collections.ObjectModel;
using System.Threading;
using MessageBox = System.Windows.MessageBox;
using VisionProject.Domain;

namespace VisionProject
{
    public enum ColorMode
    {
        R,
        G,
        B,
        Color,
        Template
    }

    class ImageView : INotifyPropertyChanged
    {
        private Stopwatch sw = new Stopwatch();

        MemoryManager memoryManager;
        long TempX = 273;
        long TempY = 252;
        int CanvasBit_Width = 800;
        ColorMode m_color = ColorMode.R;
        private System.Drawing.Rectangle _View_Rect = new System.Drawing.Rectangle();
        public System.Drawing.Rectangle p_View_Rect
        {
            get
            {
                return _View_Rect;
            }
            set
            {
                if (_View_Rect == value) return;
                _View_Rect = value;
                OnPropertyChanged();
            }
        }
        private double _Zoom = 1;
        public double p_Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                //if (_Zoom == value) return;
                _Zoom = value;
                OnPropertyChanged();
                SetRoiRect();
            }
        }
        public ColorMode p_color
        {
            get => m_color;
            set
            {
                if (m_color == value) return;
                m_color = value;
                OnPropertyChanged();
                SetRoiRect();
            }
        }
        public int p_CanvasWidth
        {
            get => CanvasBit_Width;
            set
            {
                CanvasBit_Width = value;
                OnPropertyChanged();
            }
        }
        int CanvasBit_Height = 450;
        public int p_CanvasHeight
        {
            get => CanvasBit_Height;
            set
            {
                CanvasBit_Height = value;
                OnPropertyChanged();
                SetRoiRect();
            }
        }
        public ObservableCollection<ColorMode> p_ColorList { get; set; }


        uint bfOffbits = 0;
        int Bit_Width = 0;
        int Bit_Height = 0;
        int nByte = 0;
        BitmapSource m_bitmapSource;
        CPoint m_ptViewBuffer = new CPoint();
        CPoint m_ptMouseBuffer = new CPoint();
        int m_mouseX;
        public int p_mouseX
        {
            get => m_mouseX;
            set
            {
                m_mouseX = value;
                OnPropertyChanged();
            }
        }

        int m_mouseY;
        public int p_mouseY
        {
            get => m_mouseY;
            set
            {
                if (p_CanvasWidth != 0 && p_CanvasHeight != 0)
                {
                    if (p_mouseX < p_bitmapSource.Width && p_mouseY < p_bitmapSource.Height)
                    {
                        byte[] pixel = new byte[1];
                        switch (p_color)
                        {
                            case ColorMode.R:
                                p_bitmapSource.CopyPixels(new Int32Rect(p_mouseX, p_mouseY, 1, 1), pixel, 1, 0);
                                p_pixelData1 = pixel[0];
                                break;
                            case ColorMode.G:
                                p_bitmapSource.CopyPixels(new Int32Rect(p_mouseX, p_mouseY, 1, 1), pixel, 1, 0);
                                p_pixelData2 = pixel[0];
                                break;
                            case ColorMode.B:
                                p_bitmapSource.CopyPixels(new Int32Rect(p_mouseX, p_mouseY, 1, 1), pixel, 1, 0);
                                p_pixelData3 = pixel[0];
                                break;
                            case ColorMode.Color:
                                System.Windows.Media.Color c_Pixel = GetPixelColor(p_bitmapSource, p_mouseX, p_mouseY);
                                p_pixelData1 = c_Pixel.R;
                                p_pixelData2 = c_Pixel.G;
                                p_pixelData3 = c_Pixel.B;
                                break;
                            case ColorMode.Template:
                                break;
                            default:
                                break;
                        }
                        p_mouseMemY = p_View_Rect.Y + p_mouseY * p_View_Rect.Height / p_CanvasHeight;
                        p_mouseMemX = p_View_Rect.X + p_mouseX * p_View_Rect.Width / p_CanvasWidth;
                    }
                    else
                    {

                    }
                }
                m_mouseY = value;
                OnPropertyChanged();
            }
        }

        int m_mouseMemX;
        public int p_mouseMemX
        {
            get => m_mouseMemX;
            set
            {
                m_mouseMemX = value;
                OnPropertyChanged();
            }
        }

        int m_mouseMemY;
        public int p_mouseMemY
        {
            get => m_mouseMemY;
            set
            {
                m_mouseMemY = value;
                OnPropertyChanged();
            }
        }

        int m_pixelData1;
        public int p_pixelData1
        {
            get => m_pixelData1;
            set
            {
                m_pixelData1 = value;
                OnPropertyChanged();
            }
        }

        int m_pixelData2;
        public int p_pixelData2
        {
            get => m_pixelData2;
            set
            {
                m_pixelData2 = value;
                OnPropertyChanged();
            }
        }

        int m_pixelData3;
        public int p_pixelData3
        {
            get => m_pixelData3;
            set
            {
                m_pixelData3 = value;
                OnPropertyChanged();
            }
        }

        public BitmapSource p_bitmapSource
        {
            get => m_bitmapSource;
            set
            {
                m_bitmapSource = value;
                OnPropertyChanged();
            }
        }

        public ImageView(MemoryManager _memoryManager, int initCanvasW, int initCanvasH)
        {
            try
            {
                memoryManager = _memoryManager;
                p_ColorList = new ObservableCollection<ColorMode>();
                p_ColorList.Add(ColorMode.R);
                p_ColorList.Add(ColorMode.G);
                p_ColorList.Add(ColorMode.B);
                p_ColorList.Add(ColorMode.Color);
                p_ColorList.Add(ColorMode.Template);
                p_CanvasWidth = initCanvasW;
                p_CanvasHeight = initCanvasH;
                SetRoiRect();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// 이미지 샘플링을 위한 비트맵소스 읽어오는 함수
        /// </summary>
        /// <list type="table">
        /// <listheader>
        ///    <term>22-09-16</term>
        ///    <term>이하운</term>
        ///    <term>다이얼로그에서 읽어옵니다</term>
        ///    <term>비고</term>
        ///    </listheader>
        /// <item>
        ///    <term>2022-09-16</term>
        ///    <term>이하운</term>
        ///    <term>생성</term>
        ///    <term>-</term>
        /// </item>
        /// </list>
        /// <param name="args">없음</param>
        /// <returns> 없음 </returns>
        public unsafe void ImageLoad()
        {
            FileStream fs;
            BinaryReader br;
            byte[] abuf;
            int fileRowSize;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.bmp";
            dlg.InitialDirectory = @"D:\Images";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read, 32768, true);
                br = new BinaryReader(fs);
                try
                {

                    if (!ReadBitmapFileHeader(br, ref bfOffbits)) return;
                    if (!ReadBitmapInfoHeader(br, ref Bit_Width, ref Bit_Height, ref nByte)) return;

                    if (nByte == 1)
                    {
                        fileRowSize = (Bit_Width * nByte + 3) & ~3; // 파일 내 하나의 열당 너비 사이즈(4의 배수)
                        Rectangle rect = new Rectangle(0, 0, Bit_Width, Bit_Height);
                        abuf = new byte[rect.Width * nByte];

                        // 픽셀 데이터 존재하는 부분으로 Seek
                        fs.Seek(bfOffbits, SeekOrigin.Begin);

                        //for(int i = rect.Bottom -1; i>=rect.Top; i--)
                        for (int i = rect.Bottom - 1; i >= rect.Top; i--)
                        {
                            Array.Clear(abuf, 0, rect.Width * nByte);
                            fs.Seek(rect.Left * nByte, SeekOrigin.Current); // Offset이 없으면 주석처리가능
                            fs.Read(abuf, 0, rect.Width * nByte);
                            IntPtr ptr = new IntPtr();
                            switch (m_color)
                            {
                                case ColorMode.R:
                                    ptr = new IntPtr(memoryManager.RPtr.ToInt64() + ((long)i) * memoryManager.MemoryW * nByte);
                                    break;
                                case ColorMode.G:
                                    ptr = new IntPtr(memoryManager.GPtr.ToInt64() + ((long)i) * memoryManager.MemoryW * nByte);
                                    break;
                                case ColorMode.B:
                                    ptr = new IntPtr(memoryManager.BPtr.ToInt64() + ((long)i) * memoryManager.MemoryW * nByte);
                                    break;
                                case ColorMode.Color:
                                    ptr = new IntPtr(memoryManager.RPtr.ToInt64() + ((long)i) * memoryManager.MemoryW * nByte);
                                    break;
                                case ColorMode.Template:
                                    ptr = new IntPtr(memoryManager.TPtr.ToInt64() + ((long)i) * TempX * nByte);
                                    break;
                                default:
                                    break;
                            }
                            Marshal.Copy(abuf, 0, ptr, abuf.Length);
                            fs.Seek(fileRowSize - rect.Right * nByte, SeekOrigin.Current); // Offset이 없으면 주석처리가능
                        }
                        System.Windows.Forms.MessageBox.Show("Image Load Done");
                    }
                    else
                    {
                        fileRowSize = (Bit_Width * nByte + 3) & ~3;
                        abuf = new byte[Bit_Width * nByte];
                        Rectangle rect = new Rectangle(0, 0, Bit_Width, Bit_Height);
                        fs.Seek(bfOffbits, SeekOrigin.Begin);
                        for (int yy = rect.Bottom - 1; yy >= rect.Top; yy--)
                        {
                            Array.Clear(abuf, 0, Bit_Width * nByte);
                            //fs.Seek(Bit_Width * nByte, SeekOrigin.Current);
                            fs.Read(abuf, 0, Bit_Width * nByte);
                            Parallel.For(0, Bit_Width, new ParallelOptions { MaxDegreeOfParallelism = 12 }, (xx) =>
                            {
                                Int64 idx = (Int64)yy * memoryManager.MemoryW + xx;
                                ((byte*)memoryManager.RPtr)[idx] = abuf[xx * nByte];
                                ((byte*)memoryManager.GPtr)[idx] = abuf[xx * nByte + 1];
                                ((byte*)memoryManager.BPtr)[idx] = abuf[xx * nByte + 2];
                            });
                            fs.Seek(fileRowSize - rect.Right * nByte, SeekOrigin.Current);
                        }
                        System.Windows.Forms.MessageBox.Show("Image Load Done");
                    }

                }
                catch (Exception e)
                {
                    return;
                }
                finally
                {
                    fs.Close();
                    br.Close();
                }

            }
        }

        public unsafe void ImageOpen()
        {
            try
            {
                switch (m_color)
                {
                    case ColorMode.R:
                        {
                            if (memoryManager.RPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = p_View_Rect.Y + yy * p_View_Rect.Height / p_CanvasHeight;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = p_View_Rect.X + xx * p_View_Rect.Width / p_CanvasWidth;
                                        byte* arrByte = (byte*)memoryManager.RPtr;
                                        long idx = pix_x + (pix_y * memoryManager.MemoryW);
                                        byte pixel = arrByte[idx];
                                        viewptr[yy, xx, 0] = pixel;
                                    }
                                });
                                p_bitmapSource = ToBitmapSource(view);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("INTPTR Zero");
                            }
                        }
                        break;
                    case ColorMode.G:
                        {
                            if (memoryManager.RPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = p_View_Rect.Y + yy * p_View_Rect.Height / p_CanvasHeight;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = p_View_Rect.X + xx * p_View_Rect.Width / p_CanvasWidth;
                                        byte* arrByte = (byte*)memoryManager.GPtr;
                                        long idx = pix_x + (pix_y * memoryManager.MemoryW);
                                        byte pixel = arrByte[idx];
                                        viewptr[yy, xx, 0] = pixel;
                                    }
                                });
                                p_bitmapSource = ToBitmapSource(view);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("INTPTR Zero");
                            }
                        }
                        break;
                    case ColorMode.B:
                        {
                            if (memoryManager.RPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = p_View_Rect.Y + yy * p_View_Rect.Height / p_CanvasHeight;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = p_View_Rect.X + xx * p_View_Rect.Width / p_CanvasWidth;
                                        byte* arrByte = (byte*)memoryManager.BPtr;
                                        long idx = pix_x + (pix_y * memoryManager.MemoryW);
                                        byte pixel = arrByte[idx];
                                        viewptr[yy, xx, 0] = pixel;
                                    }
                                });
                                p_bitmapSource = ToBitmapSource(view);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("INTPTR Zero");
                            }
                        }
                        break;
                    case ColorMode.Color:
                        if (memoryManager.RPtr != IntPtr.Zero)
                        {
                            Image<Rgb, byte> view = new Image<Rgb, byte>(p_CanvasWidth, p_CanvasHeight);
                            byte[,,] viewPtr = view.Data;
                            byte* RByte = (byte*)memoryManager.RPtr.ToPointer();
                            byte* GByte = (byte*)memoryManager.GPtr.ToPointer();
                            byte* BByte = (byte*)memoryManager.BPtr.ToPointer();
                            Parallel.For(0, p_View_Rect.Height < p_CanvasHeight ? p_View_Rect.Height : p_CanvasHeight, (yy) =>
                            {
                                long pix_y = p_View_Rect.Y + yy * p_View_Rect.Height / p_CanvasHeight;
                                for (int xx = 0; xx < (p_View_Rect.Width < p_CanvasWidth ? p_View_Rect.Width : p_CanvasWidth); xx++)
                                {
                                    long pix_x = p_View_Rect.X + xx * p_View_Rect.Width / p_CanvasWidth;
                                    long idx = pix_x + (pix_y * memoryManager.MemoryW);
                                    viewPtr[yy, xx, 0] = RByte[idx];
                                    viewPtr[yy, xx, 1] = GByte[idx];
                                    viewPtr[yy, xx, 2] = BByte[idx];
                                }
                            });
                            p_bitmapSource = ToBitmapSource(view);
                        }
                        break;
                    case ColorMode.Template:
                        {
                            if (memoryManager.TPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = p_View_Rect.Y + yy * p_View_Rect.Height / p_CanvasHeight;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = p_View_Rect.X + xx * p_View_Rect.Width / p_CanvasWidth;
                                        byte* arrByte = (byte*)memoryManager.TPtr;
                                        long idx = pix_x + (pix_y * TempX);
                                        byte pixel = arrByte[idx];
                                        viewptr[yy, xx, 0] = pixel;
                                    }
                                });
                                p_bitmapSource = ToBitmapSource(view);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("INTPTR Zero");
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public unsafe void ImageClear()
        {
            byte[] abuf;
            switch (m_color)
            {
                case ColorMode.R:
                    abuf = new byte[memoryManager.MemoryW * nByte];
                    for (int i = 0; i < memoryManager.MemoryH; i++)
                    {
                        Marshal.Copy(abuf, 0, memoryManager.RPtr, abuf.Length);
                    }
                    break;
                case ColorMode.G:
                    abuf = new byte[memoryManager.MemoryW * nByte];
                    for (int i = 0; i < memoryManager.MemoryH; i++)
                    {
                        Marshal.Copy(abuf, 0, memoryManager.GPtr, abuf.Length);
                    }
                    break;
                case ColorMode.B:
                    abuf = new byte[memoryManager.MemoryW * nByte];
                    for (int i = 0; i < memoryManager.MemoryH; i++)
                    {
                        Marshal.Copy(abuf, 0, memoryManager.BPtr, abuf.Length);
                    }
                    break;
                case ColorMode.Template:
                    abuf = new byte[TempX * nByte];
                    for (int i = 0; i < memoryManager.MemoryH; i++)
                    {
                        Marshal.Copy(abuf, 0, memoryManager.TPtr, abuf.Length);
                    }
                    break;
                default:
                    break;
            }
            System.Windows.Forms.MessageBox.Show("Image Clear Done");
        }

        public unsafe void ImageSave(CRect rect)
        {
            try
            {
                FileStream fs = null;
                BinaryWriter bw = null;
                SaveFileDialog dlg = new SaveFileDialog();
                byte[] abuf = new byte[rect.Width];
                dlg.Filter = "Image Files|*.bmp";
                dlg.InitialDirectory = @"D:\Images";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    fs = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
                    bw = new BinaryWriter(fs);

                    switch (m_color)
                    {
                        case ColorMode.R:
                            if (!WriteBitmapFileHeader(bw, 1, rect.Width, rect.Height))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, rect.Width, rect.Height, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = rect.Height - 1; i >= 0; i--)
                            {
                                IntPtr ptr = new IntPtr(memoryManager.RPtr.ToInt64() + ((long)i) * memoryManager.MemoryW);
                                Marshal.Copy(ptr, abuf, 0, abuf.Length);
                                bw.Write(abuf);
                            }
                            break;
                        case ColorMode.G:
                            if (!WriteBitmapFileHeader(bw, 1, rect.Width, rect.Height))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, rect.Width, rect.Height, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = rect.Height; i >= 0; i--)
                            {
                                IntPtr ptr = new IntPtr(memoryManager.GPtr.ToInt64() + ((long)i) * memoryManager.MemoryW);
                                Marshal.Copy(ptr, abuf, 0, abuf.Length);
                                bw.Write(abuf);
                            }
                            break;
                        case ColorMode.B:
                            if (!WriteBitmapFileHeader(bw, 1, rect.Width, rect.Height))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, rect.Width, rect.Height, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = rect.Height; i >= 0; i--)
                            {
                                IntPtr ptr = new IntPtr(memoryManager.BPtr.ToInt64() + ((long)i) * memoryManager.MemoryW);
                                Marshal.Copy(ptr, abuf, 0, abuf.Length);
                                bw.Write(abuf);
                            }
                            break;
                        case ColorMode.Color:
                            System.Windows.MessageBox.Show("미구현");
                            break;
                        case ColorMode.Template:
                            abuf = new byte[rect.Width / 10];
                            if (!WriteBitmapFileHeader(bw, 1, rect.Width / 10, rect.Height / 10))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, rect.Width / 10, rect.Height / 10, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = rect.Height; i >= rect.Height / 10; i--)
                            {
                                IntPtr ptr = new IntPtr(memoryManager.TPtr.ToInt64() + ((long)i) * TempX);
                                Marshal.Copy(ptr, abuf, 0, abuf.Length);
                                bw.Write(abuf);
                            }
                            break;
                        default:
                            break;
                    }

                }
                else
                {

                }
                fs.Close();
                bw.Close();
                System.Windows.MessageBox.Show("Image Save Done");
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        private bool ReadBitmapFileHeader(BinaryReader br, ref uint bfOffbits)
        {
            if (br == null) return false;

            ushort bfType;
            uint bfSize;

            bfType = br.ReadUInt16(); // isBitmap or Not
            bfSize = br.ReadUInt32(); // FileSize
            br.ReadUInt16(); //Reserved1 (NotUsed)
            br.ReadUInt16(); //Reserved2 (NotUsed)
            bfOffbits = br.ReadUInt32(); // biOffset

            if (bfType != 0x4d42) return false;

            return true;

        }


        private bool ReadBitmapInfoHeader(BinaryReader br, ref int Bit_Width, ref int Bit_Height, ref int nByte)
        {
            if (br == null) return false;

            uint biSize;
            ushort biPlane;

            biSize = br.ReadUInt32();     // biSize
            Bit_Width = br.ReadInt32();       // biBit_Width
            Bit_Height = br.ReadInt32();      // biBit_Height
            biPlane = br.ReadUInt16();              // biPlanes
            nByte = br.ReadUInt16() / 8;  // biBitcount
            br.ReadUInt32();              // biCompression
            br.ReadUInt32();              // biSizeImage
            br.ReadInt32();               // biXPelsPerMeter
            br.ReadInt32();               // biYPelsPerMeter
            br.ReadUInt32();              // biClrUsed
            br.ReadUInt32();              // biClrImportant

            return true;
        }

        private bool WriteBitmapFileHeader(BinaryWriter bw, int nByte, int width, int height)
        {
            if (bw == null) return false;
            int rowSize = (width * nByte + 3) & ~3;
            int paletteSize = (nByte == 1) ? (256 * 4) : 0;

            int size = 14 + 40 + paletteSize + rowSize * height;
            int offbit = 14 + 40 + ((nByte == 1) ? (256 * 4) : 0);

            bw.Write(Convert.ToUInt16(0x4d42)); // bfType;
            bw.Write(Convert.ToUInt32((uint)size)); // bfSize
            bw.Write(Convert.ToUInt16(0));// bfReserved1
            bw.Write(Convert.ToUInt16(0)); // bfReserved2
            bw.Write(Convert.ToUInt32(offbit));// bfOffbits

            return true;
        }

        bool WriteBitmapInfoHeader(BinaryWriter bw, int nByte, int width, int height, bool isGrayScale)
        {
            if (bw == null)
                return false;

            int biBitCount = nByte * 8;

            bw.Write(Convert.ToUInt32(40));                         // biSize
            bw.Write(Convert.ToInt32(width));                       // biWidth
            bw.Write(Convert.ToInt32(height));                      // biHeight
            bw.Write(Convert.ToUInt16(1));                          // biPlanes
            bw.Write(Convert.ToUInt16(biBitCount));                 // biBitCount
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

        private BitmapSource ToBitmapSource(Image<Gray, byte> img)
        {
            using (Bitmap source = img.ToBitmap())
            {
                var bitmapData = source.LockBits(
                    new Rectangle(0, 0, source.Width, source.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    source.PixelFormat);
                BitmapSource result = BitmapSource.Create(source.Width, source.Height, 96, 96,
                    PixelFormats.Gray8, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
                source.UnlockBits(bitmapData);
                return result;
            }
        }

        private BitmapSource ToBitmapSource(Image<Rgb, byte> img)
        {
            using (Bitmap source = img.ToBitmap())
            {
                var bitmapData = source.LockBits(
                    new Rectangle(0, 0, source.Width, source.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    source.PixelFormat);
                BitmapSource result = BitmapSource.Create(source.Width, source.Height, 96, 96,
                    PixelFormats.Rgb24, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
                source.UnlockBits(bitmapData);
                return result;
            }
        }

        public void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var pt = e.GetPosition(sender as IInputElement);
            p_mouseX = (int)pt.X;
            p_mouseY = (int)pt.Y;
            //isMove = false;
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    CanvasMovePoint_Ref(m_ptViewBuffer, m_ptMouseBuffer.X - p_mouseX, m_ptMouseBuffer.Y - p_mouseY);
                    //isMove = true;
                    return;
                }
            }
        }
        public void MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            m_ptViewBuffer.X = p_View_Rect.X;
            m_ptViewBuffer.Y = p_View_Rect.Y;
            m_ptMouseBuffer.X = p_mouseX;
            m_ptMouseBuffer.Y = p_mouseY;
        }
        public void CanvasMovePoint_Ref(CPoint point, int nX, int nY)
        {
            CPoint MovePoint = new CPoint();
            MovePoint.X = point.X + p_View_Rect.Width * nX / p_CanvasWidth;
            MovePoint.Y = point.Y + p_View_Rect.Height * nY / p_CanvasHeight;

            if (MovePoint.X < 0)
            {
                MovePoint.X = 0;
            }
            else if (MovePoint.X > memoryManager.MemoryW - p_View_Rect.Width)
            {
                MovePoint.X = (int)(memoryManager.MemoryW - p_View_Rect.Width);
            }

            if (MovePoint.Y < 0)
            {
                MovePoint.Y = 0;
            }
            else if (MovePoint.Y > memoryManager.MemoryH - p_View_Rect.Height)
            {
                MovePoint.Y = (int)(memoryManager.MemoryH - p_View_Rect.Height);
            }
            SetViewRect(MovePoint);
        }
        void SetViewRect(CPoint StartPt)
        {
            bool bRatio_WH = (double)memoryManager.MemoryW / p_CanvasWidth < (double)memoryManager.MemoryH / p_CanvasHeight;
            if (bRatio_WH)
            { //세로가 길어
                p_View_Rect = new System.Drawing.Rectangle(StartPt.X, StartPt.Y, Convert.ToInt32(memoryManager.MemoryW * p_Zoom), Convert.ToInt32(memoryManager.MemoryW * p_Zoom * p_CanvasHeight / p_CanvasWidth));
            }
            else
            {
                p_View_Rect = new System.Drawing.Rectangle(StartPt.X, StartPt.Y, Convert.ToInt32(memoryManager.MemoryH * p_Zoom * p_CanvasWidth / p_CanvasHeight), Convert.ToInt32(memoryManager.MemoryH * p_Zoom));
            }
            if (p_View_Rect.Height % 2 != 0)
            {
                _View_Rect.Height += 1;
            }
            ImageOpen();
        }

        public void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                var viewer = (System.Windows.Controls.Grid)sender;
                viewer.Focus();
                try
                {
                    int lines = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
                    double zoom = _Zoom;

                    if (lines < 0)
                    {
                        zoom *= 1.4F;
                    }
                    if (lines > 0)
                    {
                        zoom *= 0.6F;
                    }
                    if (zoom > 1)
                    {
                        zoom = 1;
                    }
                    else if (p_Zoom < 0.0001)
                    {
                        zoom = 0.0001;
                    }
                    p_Zoom = zoom;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("MouseWheel Exception : " + ee.Message);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        double m_fCpuUsage;
        public double p_fCpuUsage
        {
            get => m_fCpuUsage;
            set
            {
                m_fCpuUsage = value;
                OnPropertyChanged();
            }
        }

        long m_lTime;
        void TaskCpuUsage(int Interval, CancellationTokenSource cts)
        {
            Task.Run(() =>
            {
                int CoreCount = Environment.ProcessorCount;
                PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
                float totalCpuLoad = 0;
                float cpuLoad = 0;
                int count = 0;
                cpuLoad = cpuCounter.NextValue(); //10~20ms정도의 기본 딜레이 가지고있음
                Thread.Sleep(1000);
                do
                {
                    cpuLoad = cpuCounter.NextValue();
                    Thread.Sleep(Interval);
                    if (cpuLoad != 0)
                    {
                        totalCpuLoad += cpuLoad;
                        count++;
                    }
                } while (!cts.Token.IsCancellationRequested);
                p_fCpuUsage = (totalCpuLoad / count) / CoreCount;
                p_fCpuUsage = Math.Round(p_fCpuUsage, 2);
            }, cts.Token);
        }
        System.Windows.Media.Color GetPixelColor(BitmapSource source, int x, int y)
        {
            System.Windows.Media.Color c = Colors.White;
            if (source != null)
            {
                try
                {
                    CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
                    var pixels = new byte[4];
                    cb.CopyPixels(pixels, 4, 0);
                    c = System.Windows.Media.Color.FromRgb(pixels[2], pixels[1], pixels[0]);
                }
                catch (Exception) { }
            }
            return c;
        }
        void SetRoiRect()
        {
            CPoint StartPt = GetStartPoint_Center((int)memoryManager.MemoryW, (int)memoryManager.MemoryH);
            bool bRatio_WH = (double)memoryManager.MemoryW / p_CanvasWidth < (double)memoryManager.MemoryH / p_CanvasHeight;
            if (bRatio_WH)
            { //세로가 길어
                p_View_Rect = new System.Drawing.Rectangle(StartPt.X, StartPt.Y, Convert.ToInt32(memoryManager.MemoryW * p_Zoom), Convert.ToInt32(memoryManager.MemoryW * p_Zoom * p_CanvasHeight / p_CanvasWidth));
            }
            else
            {
                p_View_Rect = new System.Drawing.Rectangle(StartPt.X, StartPt.Y, Convert.ToInt32(memoryManager.MemoryH * p_Zoom * p_CanvasWidth / p_CanvasHeight), Convert.ToInt32(memoryManager.MemoryH * p_Zoom));
            }
            if (p_View_Rect.Height % 2 != 0)
            {
                _View_Rect.Height += 1;
            }
            ImageOpen();
        }
        CPoint GetStartPoint_Center(int nImgWidth, int nImgHeight)
        {
            bool bRatio_WH = (double)memoryManager.MemoryW / p_CanvasWidth < (double)memoryManager.MemoryH / p_CanvasHeight;
            int viewrectwidth = 0;
            int viewrectheight = 0;
            int nX = 0;
            int nY = 0;
            if (bRatio_WH)
            { //세로가 길어
              //nX = p_View_Rect.X + Convert.ToInt32(p_View_Rect.Width - nImgWidth * p_Zoom) /2; 기존 중앙기준으로 확대/축소되는 코드. 
                nX = p_View_Rect.X + Convert.ToInt32(p_View_Rect.Width - nImgWidth * p_Zoom) * p_mouseX / p_CanvasWidth; // 마우스 커서기준으로 확대/축소
                nY = p_View_Rect.Y + Convert.ToInt32(p_View_Rect.Height - nImgWidth * p_Zoom * p_CanvasHeight / p_CanvasWidth) * p_mouseY / p_CanvasHeight;
                viewrectwidth = Convert.ToInt32(nImgWidth * p_Zoom);
                viewrectheight = Convert.ToInt32(nImgWidth * p_Zoom * p_CanvasHeight / p_CanvasWidth);
            }
            else
            {
                nX = p_View_Rect.X + Convert.ToInt32(p_View_Rect.Width - nImgHeight * p_Zoom * p_CanvasWidth / p_CanvasHeight) * p_mouseX / p_CanvasWidth;
                nY = p_View_Rect.Y + Convert.ToInt32(p_View_Rect.Height - nImgHeight * p_Zoom) * p_mouseY / p_CanvasHeight;
                viewrectwidth = Convert.ToInt32(nImgHeight * p_Zoom * p_CanvasWidth / p_CanvasHeight);
                viewrectheight = Convert.ToInt32(nImgHeight * p_Zoom);
            }

            if (nX < 0)
            {
                nX = 0;
            }
            else if (nX > nImgWidth - viewrectwidth)
            {
                nX = nImgWidth - viewrectwidth;
            }

            if (nY < 0)
            {
                nY = 0;
            }
            else if (nY > nImgHeight - viewrectheight)
            {
                nY = nImgHeight - viewrectheight;
            }

            return new CPoint(nX, nY);
        }
    }    
}





