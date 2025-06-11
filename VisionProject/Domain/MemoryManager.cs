using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionProject
{
    class MemoryManager
    {
        const long GB = 1024 * 1024 * 1024;
        public IntPtr RPtr, GPtr, BPtr, TPtr;
        public long MemoryW;
        public long MemoryH;
        public bool IsColor = false;
        MemoryMappedFile m_MMF;
        MemoryMappedViewStream m_MMVS;
        public MemoryManager(int memoryW, int memoryH, bool bColor)
        {
            IsColor = bColor;
            MemoryW = memoryW;
            MemoryH = memoryH;
            long nPool = 0;
            if (IsColor)
            {
                nPool = MemoryW * MemoryH * 3;
            }
            else
            {
                nPool = MemoryW * MemoryH;
            }

            m_MMF = MemoryMappedFile.CreateOrOpen("VisionProjectMemory", nPool);
            unsafe
            {
                byte* p = null;
                m_MMF.CreateViewAccessor().SafeMemoryMappedViewHandle.AcquirePointer(ref p);
                RPtr = new IntPtr(p);
                GPtr = (IntPtr)((long)RPtr + MemoryW * MemoryH);
                BPtr = (IntPtr)((long)GPtr + MemoryW * MemoryH);
                TPtr = (IntPtr)((long)BPtr + MemoryW * MemoryH);
            }
        }
    }
}
