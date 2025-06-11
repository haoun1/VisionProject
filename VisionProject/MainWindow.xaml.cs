using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisionProject.View;

namespace VisionProject
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init_ViewModel vm = new Init_ViewModel();
            Init_PopUp ui = new Init_PopUp();
            ui.DataContext = vm;
            ui.ShowDialog();
            if (vm.p_memoryW <= 0 || vm.p_memoryH <= 0)
            {
                MessageBox.Show("Memory width or height <= 0");
                return;
            }
            DataContext = new MainWindowViewModel((int)Width, (int)Height, vm.p_memoryW, vm.p_memoryH, vm.p_bColor);
        }
    }
}
