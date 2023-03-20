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

namespace VisionProject
{
    enum ColorMode
    {
        R,
        G,
        B,
        Color,
        Template
    }

    class MainWindowViewModel : INotifyPropertyChanged
    {
        private Stopwatch sw = new Stopwatch();
        MemoryMappedFile m_MMF;
        MemoryMappedViewStream m_MMVS;
        long m_Adress;
        double fGB = 10;
        double GB = 1024 * 1024 * 1024;
        long MemoryX = 10000;
        long MemoryY = 80000;
        long TempX = 273;
        long TempY = 252;
        int MapSizeX;
        int MapSizeY;
        int CanvasBit_Width = 800;
        ColorMode m_color = ColorMode.R;
        public ColorMode p_color
        {
            get => m_color;
            set 
            {
                if (m_color == value) return;
                m_color = value;
                OnPropertyChanged();
                ImageOpen();
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
                ImageOpen();
            }
        }
        public ObservableCollection<ColorMode> p_ColorList { get; set; }


        uint bfOffbits = 0;
        int Bit_Width = 0;
        int Bit_Height = 0;
        int nByte = 0;
        BitmapSource m_bitmapSource;
        IntPtr RPtr, GPtr, BPtr, TPtr;

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
                try
                {
                    unsafe
                    {
                        int SamplingRate_y = MapSizeY < p_CanvasHeight ? 1 : MapSizeY / p_CanvasHeight;
                        int SamplingRate_x = MapSizeX < p_CanvasWidth ? 1 : MapSizeX / p_CanvasWidth;
                        long idx1;
                        long idx2;
                        long idx3;
                        byte b1;
                        byte b2;
                        byte b3;
                        p_mouseMemX = p_mouseX * SamplingRate_x;
                        p_mouseMemY = p_mouseY * SamplingRate_y;

                        if (p_mouseMemX > MapSizeX)
                        {
                            p_pixelData1 = 0;
                            p_pixelData2 = 0;
                            p_pixelData3 = 0;
                        }
                        else if (p_mouseMemY > MapSizeY)
                        {
                            p_pixelData1 = 0;
                            p_pixelData2 = 0;
                            p_pixelData3 = 0;
                        }
                        else
                        {

                            byte* arrByte = (byte*)RPtr.ToPointer();

                            switch (m_color)
                            {
                                case ColorMode.R:
                                    idx1 = p_mouseMemX + ((long)p_mouseMemY * MemoryX);
                                    b1 = arrByte[idx1];
                                    p_pixelData1 = BitConverter.ToUInt16(new byte[2] { b1, 0 }, 0);
                                    p_pixelData2 = 0;
                                    p_pixelData3 = 0;
                                    break;
                                case ColorMode.G:
                                    idx2 = MemoryX * MemoryY + (p_mouseMemX + ((long)p_mouseMemY * MemoryX));
                                    b2 = arrByte[idx2];
                                    p_pixelData2 = BitConverter.ToUInt16(new byte[2] { b2, 0 }, 0);
                                    p_pixelData1 = 0;
                                    p_pixelData3 = 0;
                                    break;
                                case ColorMode.B:
                                    idx3 = 2 * MemoryX * MemoryY + (p_mouseMemX + ((long)p_mouseMemY * MemoryX));
                                    b3 = arrByte[idx3];
                                    p_pixelData3 = BitConverter.ToUInt16(new byte[2] { b3, 0 }, 0);
                                    p_pixelData1 = 0;
                                    p_pixelData2 = 0;
                                    break;
                                case ColorMode.Color:
                                    idx1 = p_mouseMemX + ((long)p_mouseMemY * MemoryX);
                                    b1 = arrByte[idx1];
                                    p_pixelData1 = BitConverter.ToUInt16(new byte[2] { b1, 0 }, 0);

                                    idx2 = MemoryX * MemoryY + (p_mouseMemX + ((long)p_mouseMemY * MemoryX));
                                    b2 = arrByte[idx2];
                                    p_pixelData2 = BitConverter.ToUInt16(new byte[2] { b2, 0 }, 0);

                                    idx3 = 2 * MemoryX * MemoryY + (p_mouseMemX + ((long)p_mouseMemY * MemoryX));
                                    b3 = arrByte[idx3];
                                    p_pixelData3 = BitConverter.ToUInt16(new byte[2] { b3, 0 }, 0);
                                    break;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
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

        public MainWindowViewModel()
        {
            try
            {
                long nPool = (long)Math.Ceiling(fGB * GB);
                m_MMF = MemoryMappedFile.CreateOrOpen("Memory", nPool);
                MapSizeX = 1000;
                MapSizeY = 1000;
                p_ColorList = new ObservableCollection<ColorMode>();
                p_ColorList.Add(ColorMode.R);
                p_ColorList.Add(ColorMode.G);
                p_ColorList.Add(ColorMode.B);
                p_ColorList.Add(ColorMode.Color);
                p_ColorList.Add(ColorMode.Template);
                unsafe
                {
                    byte* p = null;
                    m_MMF.CreateViewAccessor().SafeMemoryMappedViewHandle.AcquirePointer(ref p);
                    RPtr = new IntPtr(p);
                    GPtr = (IntPtr)((long)RPtr + (long)(MemoryX * MemoryY));
                    BPtr = (IntPtr)((long)GPtr + (long)(MemoryX * MemoryY));
                    TPtr = (IntPtr)((long)BPtr + (long)(MemoryX * MemoryY));
                }
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        public RelayCommand ImageLoadCommand
        {
            get => new RelayCommand(ImageLoad);
        }

        public RelayCommand ImageClearCommand
        {
            get => new RelayCommand(ImageClear);
        }

        public RelayCommand ImageSaveCommand
        {
            get => new RelayCommand(ImageSave);
        }

        public RelayCommand loadedCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }
        public RelayCommand UnloadedCommand
        {
            get => new RelayCommand(() =>
            {
                m_MMF.Dispose();
            });
        }

        public RelayCommand ThresholdCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    Threshold();
                }
                catch(Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand erodeCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    erode();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand dilateCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    dilate();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }
        public RelayCommand CV2_GaussianCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    CV2_Gaussian();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand AI_GaussianCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    AI_Gaussian();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand CV2_HequalCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    CV2_Hequal();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand AI_HequalCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                      AI_Hequal();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand CV2_OtsuCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    CV2_Otsu();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand AI_OtsuCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    AI_Otsu();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand CV2_LaplaceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    CV2_Laplace();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand AI_LaplaceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    AI_Laplace();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand AI_LPFCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    AI_FFT_LPF();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }

        public RelayCommand AI_TMCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    AI_TM();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
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
        private unsafe void ImageLoad()
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
                    MapSizeX = Bit_Width;
                    MapSizeY = Bit_Height;

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
                                    ptr = new IntPtr(RPtr.ToInt64() + ((long)i) * MemoryX * nByte);
                                    break;
                                case ColorMode.G:
                                    ptr = new IntPtr(GPtr.ToInt64() + ((long)i) * MemoryX * nByte);
                                    break;
                                case ColorMode.B:
                                    ptr = new IntPtr(BPtr.ToInt64() + ((long)i) * MemoryX * nByte);
                                    break;
                                case ColorMode.Color:
                                    ptr = new IntPtr(RPtr.ToInt64() + ((long)i) * MemoryX * nByte);
                                    break;
                                case ColorMode.Template:
                                    ptr = new IntPtr(TPtr.ToInt64() + ((long)i) * TempX * nByte);
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
                                Int64 idx = (Int64)yy * MemoryX + xx;
                                ((byte*)RPtr)[idx] = abuf[xx * nByte];
                                ((byte*)GPtr)[idx] = abuf[xx * nByte + 1];
                                ((byte*)BPtr)[idx] = abuf[xx * nByte + 2];
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

        private unsafe void ImageOpen()
        {
            try
            {
                switch (m_color)
                {
                    case ColorMode.R:
                        {
                            if (RPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                int SamplingRate_y = MapSizeY < p_CanvasHeight ? 1 : MapSizeY / p_CanvasHeight;
                                int SamplingRate_x = MapSizeX < p_CanvasWidth ? 1 : MapSizeX / p_CanvasWidth;
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = yy * SamplingRate_y;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = xx * SamplingRate_x;
                                        byte* arrByte = (byte*)RPtr;
                                        long idx = pix_x + (pix_y * MemoryX);
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
                            if (RPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                int SamplingRate_y = MapSizeY < p_CanvasHeight ? 1 : MapSizeY / p_CanvasHeight;
                                int SamplingRate_x = MapSizeX < p_CanvasWidth ? 1 : MapSizeX / p_CanvasWidth;
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = yy * SamplingRate_y;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = xx * SamplingRate_x;
                                        byte* arrByte = (byte*)GPtr;
                                        long idx = pix_x + (pix_y * MemoryX);
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
                            if (RPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                int SamplingRate_y = MapSizeY < p_CanvasHeight ? 1 : MapSizeY / p_CanvasHeight;
                                int SamplingRate_x = MapSizeX < p_CanvasWidth ? 1 : MapSizeX / p_CanvasWidth;
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = yy * SamplingRate_y;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = xx * SamplingRate_x;
                                        byte* arrByte = (byte*)BPtr;
                                        long idx = pix_x + (pix_y * MemoryX);
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
                        if (RPtr != IntPtr.Zero)
                        {
                            Image<Rgb, byte> view = new Image<Rgb, byte>(p_CanvasWidth, p_CanvasHeight);
                            int SamplingRate_y = MapSizeY < p_CanvasHeight ? 1 : MapSizeY / p_CanvasHeight;
                            int SamplingRate_x = MapSizeX < p_CanvasWidth ? 1 : MapSizeX / p_CanvasWidth;
                            byte[,,] viewPtr = view.Data;
                            byte* RByte = (byte*)RPtr.ToPointer();
                            byte* GByte = (byte*)GPtr.ToPointer();
                            byte* BByte = (byte*)BPtr.ToPointer();
                            Parallel.For(0, MapSizeY < p_CanvasHeight ? MapSizeY : p_CanvasHeight, (yy) =>
                            {
                                long pix_y = yy * SamplingRate_y;
                                for (int xx = 0; xx < (MapSizeX < p_CanvasWidth ? MapSizeX : p_CanvasWidth); xx++)
                                {
                                    long pix_x = xx * SamplingRate_x;
                                    long idx = pix_x + (pix_y * MemoryX);
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
                            if (TPtr != IntPtr.Zero)
                            {
                                Image<Gray, byte> view = new Image<Gray, byte>(p_CanvasWidth, p_CanvasHeight);
                                int SamplingRate_y = MapSizeY < p_CanvasHeight ? 1 : MapSizeY / p_CanvasHeight;
                                int SamplingRate_x = MapSizeX < p_CanvasWidth ? 1 : MapSizeX / p_CanvasWidth;
                                byte[,,] viewptr = view.Data;
                                Parallel.For(0, p_CanvasHeight, (yy) =>
                                {
                                    long pix_y = yy * SamplingRate_y;
                                    for (int xx = 0; xx < p_CanvasWidth; xx++)
                                    {
                                        long pix_x = xx * SamplingRate_x;
                                        byte* arrByte = (byte*)TPtr;
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

        private unsafe void ImageClear()
        {
            byte[] abuf;
            switch (m_color)
            {
                case ColorMode.R:
                    abuf = new byte[MemoryX * nByte];
                    for (int i = 0; i < MemoryY; i++)
                    {
                        IntPtr Rptr = new IntPtr(RPtr.ToInt64() + ((long)i) * MemoryX * nByte);
                        Marshal.Copy(abuf, 0, Rptr, abuf.Length);
                    }
                    break;
                case ColorMode.G:
                    abuf = new byte[MemoryX * nByte];
                    for (int i = 0; i < MemoryY; i++)
                    {
                        IntPtr Gptr = new IntPtr(GPtr.ToInt64() + ((long)i) * MemoryX * nByte);
                        Marshal.Copy(abuf, 0, Gptr, abuf.Length);
                    }
                    break;
                case ColorMode.B:
                    abuf = new byte[MemoryX * nByte];
                    for (int i = 0; i < MemoryY; i++)
                    {
                        IntPtr Bptr = new IntPtr(BPtr.ToInt64() + ((long)i) * MemoryX * nByte);
                        Marshal.Copy(abuf, 0, Bptr, abuf.Length);
                    }
                    break;
                case ColorMode.Template:
                    abuf = new byte[TempX * nByte];
                    for (int i = 0; i < MemoryY; i++)
                    {
                        IntPtr Tptr = new IntPtr(TPtr.ToInt64() + ((long)i) * TempX * nByte);
                        Marshal.Copy(abuf, 0, Tptr, abuf.Length);
                    }
                    break;
                default:
                    break;
            }
            System.Windows.Forms.MessageBox.Show("Image Clear Done");
        }

        private unsafe void ImageSave()
        {
            try
            {
                FileStream fs = null;
                BinaryWriter bw = null;
                SaveFileDialog dlg = new SaveFileDialog();
                byte[] abuf = new byte[MapSizeX];
                dlg.Filter = "Image Files|*.bmp";
                dlg.InitialDirectory = @"D:\Images";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    fs = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
                    bw = new BinaryWriter(fs);

                    switch (m_color)
                    {
                        case ColorMode.R:
                            if (!WriteBitmapFileHeader(bw, 1, MapSizeX, MapSizeY))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, MapSizeX, MapSizeY, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = MapSizeY - 1; i >= 0; i--)
                            {
                                IntPtr ptr = new IntPtr(RPtr.ToInt64() + ((long)i) * MemoryX);
                                Marshal.Copy(ptr, abuf, 0, abuf.Length);
                                bw.Write(abuf);
                            }
                            break;
                        case ColorMode.G:
                            if (!WriteBitmapFileHeader(bw, 1, MapSizeX, MapSizeY))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, MapSizeX, MapSizeY, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = MapSizeY; i >= 0; i--)
                            {
                                IntPtr ptr = new IntPtr(GPtr.ToInt64() + ((long)i) * MemoryX);
                                Marshal.Copy(ptr, abuf, 0, abuf.Length);
                                bw.Write(abuf);
                            }
                            break;
                        case ColorMode.B:
                            if (!WriteBitmapFileHeader(bw, 1, MapSizeX, MapSizeY))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, MapSizeX, MapSizeY, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = MapSizeY; i >= 0; i--)
                            {
                                IntPtr ptr = new IntPtr(BPtr.ToInt64() + ((long)i) * MemoryX);
                                Marshal.Copy(ptr, abuf, 0, abuf.Length);
                                bw.Write(abuf);
                            }
                            break;
                        case ColorMode.Color:
                            System.Windows.MessageBox.Show("미구현");
                            break;
                        case ColorMode.Template:
                            abuf = new byte[MapSizeX / 10];
                            if (!WriteBitmapFileHeader(bw, 1, MapSizeX / 10, MapSizeY / 10))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap FileHeader Error");
                                return;
                            }
                            if (!WriteBitmapInfoHeader(bw, 1, MapSizeX / 10, MapSizeY / 10, false))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            if (!WritePalette(bw))
                            {
                                System.Windows.MessageBox.Show("Write Bitmap InfoHeader Error");
                                return;
                            }
                            for (int i = MapSizeY; i >= MapSizeY / 10; i--)
                            {
                                IntPtr ptr = new IntPtr(TPtr.ToInt64() + ((long)i) * TempX);
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
            catch(Exception e)
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

        bool WriteBitmapInfoHeader(BinaryWriter bw, int nByte, int width, int height, bool isGrayScale )
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


        private void Threshold()
        {
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MemoryX * MemoryY ];
                    resultArray = new byte[MemoryX * MemoryY ];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY) );
                    CustomCV.Custom_Threshold(sourceArray, resultArray, MemoryX , MemoryY, 100, false);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MemoryX * MemoryY ];
                    resultArray = new byte[MemoryX * MemoryY ];
                    Marshal.Copy(GPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_Threshold(sourceArray, resultArray, MemoryX , MemoryY, 100, false);
                    Marshal.Copy(resultArray, 0, GPtr, resultArray.Length);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MemoryX * MemoryY ];
                    resultArray = new byte[MemoryX * MemoryY ];
                    Marshal.Copy(BPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    CustomCV.Custom_Threshold(sourceArray, resultArray, MemoryX , MemoryY, 100, false);
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
            System.Windows.MessageBox.Show("Threshold Done");
        }

        private void erode()
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
            System.Windows.MessageBox.Show("erode Done");
        }

        private void dilate()
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
            System.Windows.MessageBox.Show("dilate Done");
        }



        private void CV2_Gaussian()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task t = Task.Factory.StartNew(() => { TaskCpuUsage(10, cts); });
            sw.Start();
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
            m_lTime = sw.ElapsedMilliseconds;
            sw.Reset();
            cts.Cancel();
            t.Wait();
            System.Windows.MessageBox.Show("Gaussian Done : " + m_lTime.ToString() + "CPU Usage : " + p_fCpuUsage.ToString());
        }

        private void AI_Gaussian()
        {
            sw.Start();
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_GaussianFilter(sourceArray, resultArray, MapSizeX, MapSizeY,5,10);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_GaussianFilter(sourceArray, resultArray, MapSizeX, MapSizeY, 20, 1);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_GaussianFilter(sourceArray, resultArray, MapSizeX, MapSizeY, 20, 1);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
            System.Windows.MessageBox.Show("Gaussian Done" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
        }

        private void CV2_Hequal()
        {
            sw.Start();
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
            System.Windows.MessageBox.Show("Hequal Done" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
        }

        private void AI_Hequal()
        {
            sw.Start();
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_HistogramEqualization(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_HistogramEqualization(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_HistogramEqualization(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
            System.Windows.MessageBox.Show("Hequal Done" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
        }

        private void CV2_Otsu()
        {
            sw.Start();
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.CV2_OtsuThresholding(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i ++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
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
            System.Windows.MessageBox.Show("Otsu Done" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
        }

        private void AI_Otsu()
        {
            sw.Start();
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_OtsuThresholding(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_OtsuThresholding(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_OtsuThresholding(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
            System.Windows.MessageBox.Show("Otsu Done" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
        }

        private void CV2_Laplace()
        {
            sw.Start();
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
            System.Windows.MessageBox.Show("Laplace Done" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
        }

        private void AI_Laplace()
        {
            sw.Start();
            byte[] sourceArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(RPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_Laplacian(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(GPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_Laplacian(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), GPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(BPtr + (i * (int)MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                    CustomCV.AI_Laplacian(sourceArray, resultArray, MapSizeX, MapSizeY);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), BPtr + (i * (int)MemoryX), MapSizeX);
                    break;
            }
            System.Windows.MessageBox.Show("Laplace Done" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
        }

        private void AI_FFT_LPF()
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
            System.Windows.MessageBox.Show("HPF Done");
        }

        private void AI_TM()
        {
            byte[] sourceArray;
            byte[] templateArray;
            byte[] resultArray;
            switch (m_color)
            {
                case ColorMode.R:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    templateArray = new byte[TempX * TempY];
                    for (int i = 0; i < MapSizeY; i++)
                    {
                        Marshal.Copy(RPtr + (i * (int) MemoryX), sourceArray, (i * MapSizeX), MapSizeX);
                        Marshal.Copy(RPtr + (i * (int)MemoryX), resultArray, (i * MapSizeX), MapSizeX);
                    }

                    Marshal.Copy(TPtr, templateArray, 0, Convert.ToInt32(TempX * TempY));
                    CustomCV.AI_TemplateMatching(sourceArray, resultArray, MapSizeX, MapSizeY, templateArray, (int)TempX, (int)TempY, 5);
                    for (int i = 0; i < MapSizeY; i++) Marshal.Copy(resultArray, (i * MapSizeX), RPtr + (i * (int)MemoryX), MapSizeX);
                    break;
                case ColorMode.G:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    templateArray = new byte[TempX * TempY];
                    Marshal.Copy(GPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    Marshal.Copy(TPtr, templateArray, 0, Convert.ToInt32(TempX * TempY));
                    CustomCV.AI_TemplateMatching(sourceArray, resultArray, (int)MemoryX, (int)MemoryY, templateArray, (int)TempX, (int)TempY, 5);
                    Marshal.Copy(resultArray, 0, GPtr, resultArray.Length);
                    break;
                case ColorMode.B:
                    sourceArray = new byte[MapSizeX * MapSizeY];
                    resultArray = new byte[MapSizeX * MapSizeY];
                    templateArray = new byte[TempX * TempY];
                    Marshal.Copy(BPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY));
                    Marshal.Copy(TPtr, templateArray, 0, Convert.ToInt32(TempX * TempY));
                    CustomCV.AI_TemplateMatching(sourceArray, resultArray, (int)MemoryX, (int)MemoryY, templateArray, (int)TempX, (int)TempY, 5);
                    Marshal.Copy(resultArray, 0, BPtr, resultArray.Length);
                    break;
                case ColorMode.Color:
                    sourceArray = new byte[MemoryX * MemoryY * 3];
                    resultArray = new byte[MemoryX * MemoryY * 3];
                    templateArray = new byte[TempX * TempY];
                    Marshal.Copy(RPtr, sourceArray, 0, Convert.ToInt32(MemoryX * MemoryY * 3));
                    Marshal.Copy(TPtr, templateArray, 0, Convert.ToInt32(TempX * TempY * 3));
                    CustomCV.AI_TemplateMatching(sourceArray, resultArray, (int)MemoryX, (int)MemoryY, templateArray, (int)TempX, (int)TempY, 5);
                    Marshal.Copy(resultArray, 0, RPtr, resultArray.Length);
                    break;
            }
            System.Windows.MessageBox.Show("HPF Done");
        }

        public void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var pt = e.GetPosition(sender as IInputElement);
            p_mouseX = (int)pt.X;
            p_mouseY = (int)pt.Y;
        }

        public void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ImageOpen();
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
    }
    public class RelayCommand : ICommand
    {

        #region Declarations

        readonly Func<Boolean> _canExecute;
        readonly Action _execute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(Object parameter)
        {
            try
            {
                _execute();
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}

//AjinAxis를 제어하는 클래스 정의





