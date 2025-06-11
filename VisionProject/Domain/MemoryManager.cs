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
        MemoryMappedFile m_MMF;
        MemoryMappedViewStream m_MMVS;
        public MemoryManager(int memoryW, int memoryH, long fGB)
        {
            long nPool = fGB * GB;
            MemoryW = memoryW;
            MemoryH = memoryH;
            m_MMF = MemoryMappedFile.CreateOrOpen("Memory", nPool);
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
