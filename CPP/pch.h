// pch.h: �̸� �����ϵ� ��� �����Դϴ�.
// �Ʒ� ������ ������ �� ���� �����ϵǾ�����, ���� ���忡 ���� ���� ������ ����մϴ�.
// �ڵ� ������ �� ���� �ڵ� �˻� ����� �����Ͽ� IntelliSense ���ɿ��� ������ ��Ĩ�ϴ�.
// �׷��� ���⿡ ������ ������ ���� �� ������Ʈ�Ǵ� ��� ��� �ٽ� �����ϵ˴ϴ�.
// ���⿡ ���� ������Ʈ�� ������ �߰����� ������. �׷��� ������ ���ϵ˴ϴ�.

#ifndef PCH_H
#define PCH_H

// ���⿡ �̸� �������Ϸ��� ��� �߰�
typedef unsigned char byte;

//3D


#include <time.h>
#include <math.h>
#include <basetsd.h>

#include "windows.h"

#define BLOCK_SIZE 1000
#define MAX_SAME_FRAME_COUNT 10000

typedef unsigned long       DWORD;
typedef int                 BOOL;
typedef unsigned char       BYTE;
typedef unsigned short      WORD;
typedef float               FLOAT;

typedef BYTE* LPBYTE;

class CCSize
{
public:
	CCSize() {}
	CCSize(int _x, int _y)
	{
		cx = _x;
		cy = _y;
	}
	int cx;
	int cy;
};
class CCPoint
{
public:
	CCPoint() {}
	CCPoint(int _x, int _y)
	{
		x = _x;
		y = _y;
	}
	int x;
	int y;
};

class CCRect
{
public:
	CCRect() {}
	CCRect(int _x, int _y, int _cx, int _cy)
	{
		x = _x;
		y = _y;
		cy = _cy;
		cx = _cx;
	}
	int x;
	int y;
	int cx;
	int cy;
};

#endif //PCH_H
