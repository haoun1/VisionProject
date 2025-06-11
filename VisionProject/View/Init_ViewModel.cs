using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionProject.MVVM;

namespace VisionProject
{
    public class Init_ViewModel : ObservableObject
    {
        bool m_bColor = false;
        int m_memoryW;
        int m_memoryH;
        public bool p_bColor
        {
            get => m_bColor;
            set
            {
                SetProperty(ref m_bColor, value);
            }
        }
        public int p_memoryW
        {
            get => m_memoryW;
            set
            {
                SetProperty(ref m_memoryW, value);
            }
        }
        public int p_memoryH
        {
            get => m_memoryH;
            set
            {
                SetProperty(ref m_memoryH, value);
            }
        }
    }
}
