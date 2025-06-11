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
using VisionProject.MVVM;
using VisionProject.View;
using VisionProject.Domain;

namespace VisionProject
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MemoryManager p_memoryManager { get; set; }
        public ImageView p_imageView { get; set; }
        public ImageProcess p_imageProcess { get; set; }
        public MainWindowViewModel(int initCanvasW, int initCanvasH, int memoryW, int memoryH, bool bColor)
        {
            p_memoryManager = new MemoryManager(memoryW, memoryH, bColor);
            p_imageView = new ImageView(p_memoryManager, initCanvasW, initCanvasH);
            p_imageProcess = new ImageProcess();
        }
        public RelayCommand ThresholdCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    p_imageProcess.Threshold(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH);
                }
                catch (Exception e)
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
                    p_imageProcess.erode(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH);
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
                    p_imageProcess.dilate(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH);
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
                    p_imageProcess.CV2_Gaussian(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH, (int)p_imageView.p_bitmapSource.Width, (int)p_imageView.p_bitmapSource.Height);
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
                    p_imageProcess.CV2_Hequal(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH, (int)p_imageView.p_bitmapSource.Width, (int)p_imageView.p_bitmapSource.Height);
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
                    p_imageProcess.CV2_Otsu(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH, (int)p_imageView.p_bitmapSource.Width, (int)p_imageView.p_bitmapSource.Height);
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
                    p_imageProcess.CV2_Laplace(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH, (int)p_imageView.p_bitmapSource.Width, (int)p_imageView.p_bitmapSource.Height);
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
                    p_imageProcess.AI_FFT_LPF(p_imageView.p_color, p_memoryManager.RPtr, p_memoryManager.GPtr, p_memoryManager.BPtr, p_memoryManager.MemoryW, p_memoryManager.MemoryH, (int)p_imageView.p_bitmapSource.Width, (int)p_imageView.p_bitmapSource.Height);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }
        public RelayCommand ImageLoadCommand
        {
            get => new RelayCommand(p_imageView.ImageLoad);
        }

        public RelayCommand ImageClearCommand
        {
            get => new RelayCommand(p_imageView.ImageClear);
        }

        public RelayCommand ImageSaveCommand
        {
            get => new RelayCommand(()=> {
                ImageSaveROI_ViewModel vm = new ImageSaveROI_ViewModel();
                ImageSaveROI_PopUp ui = new ImageSaveROI_PopUp();
                ui.DataContext = vm;
                ui.ShowDialog();
                if (vm.p_Width <= 0 || vm.p_Height <= 0)
                {
                    MessageBox.Show("width or height <= 0");
                    return;
                }
                p_imageView.ImageSave(new Domain.CRect(new CPoint(vm.p_startX, vm.p_startY), vm.p_Width, vm.p_Height)); 
            });
        }
    }
}