using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionProject.Domain
{
    public class Tile
    {
        public IntPtr ptr;
        public int startX;
        public int startY;
        public int width;
        public int height;
        public int nByte;
        public int nCount;
        public int stride;
        public Tile(IntPtr _ptr, int _startX, int _startY, int _width, int _height, int _nByte, int _nCount, int _stride)
        {
            ptr = _ptr;
            nByte = _nByte;
            nCount = _nCount;
            stride = _stride;
            startX = _startX;
            startY = _startY;
            width = _width;
            height = _height;
        }
    }
}
