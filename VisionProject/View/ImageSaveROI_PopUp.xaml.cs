﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VisionProject.View
{
    /// <summary>
    /// ImageSaveROI_PopUp.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageSaveROI_PopUp : Window
    {
        public ImageSaveROI_PopUp()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
