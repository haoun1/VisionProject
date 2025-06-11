using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionProject.MVVM;

namespace VisionProject.View
{
    public class ImageSaveROI_ViewModel : ObservableObject
    {
        int m_startX;
        int m_startY;
        int m_Width;
        int m_Height;
        public int p_startX
        {
            get => m_startX;
            set
            {
                SetProperty(ref m_startX, value);
            }
        }
        public int p_startY
        {
            get => m_startY;
            set
            {
                SetProperty(ref m_startY, value);
            }
        }
        public int p_Width
        {
            get => m_Width;
            set
            {
                SetProperty(ref m_Width, value);
            }
        }
        public int p_Height
        {
            get => m_Height;
            set
            {
                SetProperty(ref m_Height, value);
            }
        }
    }
}
