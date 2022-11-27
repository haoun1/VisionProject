#pragma once
#include "pch.h"
#include "windows.h"
#include "memoryapi.h"
#include <stdio.h>
#include <list>
#include "..\CPP\\CPP.h"
#include <iostream>

using namespace System::Collections::Generic;

namespace CLR
{
	public ref class CLR_IP
	{
	protected:
	public:
		static void CPP_Threshold(array<byte>^ pSrcImg, array<byte>^ pDstImg, int nMemW, int nMemH, bool bDark, int nThresh);
	};
}