using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace VisionProject.Domain
{
    public class CPoint : IConvertible
    {
        int _x;
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        int _y;
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public CPoint()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Left/Top Point 생성
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public CPoint(Rect rect)
        {
            X = (int)(rect.Left);
            Y = (int)(rect.Top);
        }

        public CPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public CPoint(CPoint p)
        {
            X = p.X;
            Y = p.Y;
        }

        public CPoint(Point pos)
        {
            X = (int)pos.X;
            Y = (int)pos.Y;
        }

        public void Set(CPoint cp)
        {
            X = cp.X;
            Y = cp.Y;
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ")";
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CPoint cp = (CPoint)obj;
            if (cp.X != X)
            {
                return false;
            }

            if (cp.Y != Y)
            {
                return false;
            }

            return true;
        }

        public static bool operator ==(CPoint cp0, CPoint cp1)
        {
            if (cp0 is null && cp1 is null)
            {
                return true;
            }

            cp0 = cp0 ?? new CPoint();
            return cp0.Equals(cp1);
        }

        public static bool operator !=(CPoint cp0, CPoint cp1)
        {
            if (cp0 is null && cp1 is null)
            {
                return true;
            }

            cp0 = cp0 ?? new CPoint();
            return !cp0.Equals(cp1);
        }

        public static CPoint operator +(CPoint cp0, CPoint cp1)
        {
            return new CPoint(cp0.X + cp1.X, cp0.Y + cp1.Y);
        }

        public static CPoint operator -(CPoint cp0, CPoint cp1)
        {
            return new CPoint(cp0.X - cp1.X, cp0.Y - cp1.Y);
        }

        public static CPoint operator *(CPoint cp0, double f)
        {
            return new CPoint((int)Math.Round(cp0.X * f), (int)Math.Round(cp0.Y * f));
        }

        public static CPoint operator /(CPoint cp0, double f)
        {
            return new CPoint((int)Math.Round(cp0.X / f), (int)Math.Round(cp0.Y / f));
        }

        public bool IsInside(CPoint cp)
        {
            if (cp.X > X)
            {
                return false;
            }

            if (cp.Y > Y)
            {
                return false;
            }

            if (cp.X < 0)
            {
                return false;
            }

            if (cp.Y < 0)
            {
                return false;
            }

            return true;
        }
        public double Distance(CPoint cp)
        {
            return Math.Sqrt((X - cp.X) * (X - cp.X) + (Y - cp.Y) * (Y - cp.Y));
        }
        public void Transpose()
        {
            int n = X;
            X = Y;
            Y = n;
        }

        public static CPoint Parse(string v)
        {
            var arr = v.Replace("(", "").Replace(")", "").Split(',');
            if (arr.Length != 2)
            {
                return new CPoint(-1, -1);
            }
            try
            {
                return new CPoint(int.Parse(arr[0]), int.Parse(arr[1]));
            }
            catch (Exception ex)
            {
                return new CPoint(-1, -1);
            }
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public string ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this, conversionType);
        }
    }
    public class CRect
    {
        int _x;
        [XmlIgnore]
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        int _y;
        [XmlIgnore]
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        int _left;
        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }
        int _top;
        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }
        int _right;
        public int Right
        {
            get { return _right; }
            set { _right = value; }
        }
        int _bottom;
        public int Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        [XmlIgnore]
        public int Width
        {
            get
            {
                return Math.Abs(Right - Left);
            }
            set
            {
                Right = Math.Abs(value + Left);
            }
        }
        [XmlIgnore]
        public int Height
        {
            get
            {
                return Math.Abs(Bottom - Top);
            }
            set
            {
                Bottom = Math.Abs(value + Top);
            }
        }
        public CRect()
        {
            Left = 0;
            Top = 0;
            Right = 0;
            Bottom = 0;
            X = 0;
            Y = 0;
        }
        public CRect(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            Left = (int)startPoint.X;
            Top = (int)startPoint.Y;
            Right = (int)endPoint.X;
            Bottom = (int)endPoint.Y;

            X = ((Right - Left) / 2);
            Y = ((Bottom - Top) / 2);
        }
        public CRect(CPoint startPoint, CPoint endPoint)
        {
            Left = startPoint.X;
            Top = startPoint.Y;
            Right = endPoint.X;
            Bottom = endPoint.Y;

            X = ((Right - Left) / 2);
            Y = ((Bottom - Top) / 2);
        }
        public CRect(int cenx, int ceny, int size)
        {
            X = cenx;
            Y = ceny;
            Left = X - size / 2;
            Right = X + size / 2;
            Top = Y - size / 2;
            Bottom = Y + size / 2;
        }

        public CRect(CPoint p, int width, int height)
        {
            X = p.X;
            Y = p.Y;
            Left = X - width / 2;
            Right = X + width / 2;
            Top = Y - height / 2;
            Bottom = Y + height / 2;
        }
        public CRect(int l, int t, int r, int b)
        {
            Left = l;
            Right = r;
            Top = t;
            Bottom = b;

            if (l > r)
            {
                Left = r;
                Right = l;
            }
            if (b < t)
            {
                Bottom = t;
                Top = b;
            }
            X = ((Right - Left) / 2);
            Y = ((Bottom - Top) / 2);

        }
        /// <summary>
        /// Top,Left,Bottom,Right를 위치에 맞게 재정렬한다
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static CRect ReAllocate(CRect rect)
        {
            CRect result = new CRect();

            result.Left = rect.Left;
            result.Right = rect.Right;
            result.Top = rect.Top;
            result.Bottom = rect.Bottom;

            if (rect.Left > rect.Right)
            {
                result.Left = rect.Right;
                result.Right = rect.Left;
            }
            if (rect.Bottom < rect.Top)
            {
                result.Bottom = rect.Top;
                result.Top = rect.Bottom;
            }
            result.X = ((result.Right - result.Left) / 2);
            result.Y = ((result.Bottom - result.Top) / 2);

            return result;
        }

        public CRect(CRect rect)
        {
            rect = ReAllocate(rect);

            this.Top = rect.Top;
            this.Bottom = rect.Bottom;
            this.Right = rect.Right;
            this.Left = rect.Left;
            this.X = rect.Left + rect.Width / 2;
            this.Y = rect.Top + rect.Height / 2;
        }

        public void Set(CRect rt)
        {
            X = rt.X;
            Y = rt.Y;
            Left = rt.Left;
            Right = rt.Right;
            Top = rt.Top;
            Bottom = rt.Bottom;
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ", " + Left.ToString() + ", " + Top.ToString() + ", " + Right.ToString() + ", " + Bottom.ToString() + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CRect rt = (CRect)obj;
            if (rt.X != X)
            {
                return false;
            }

            if (rt.Y != Y)
            {
                return false;
            }

            if (rt.Left != Left)
            {
                return false;
            }

            if (rt.Top != Top)
            {
                return false;
            }

            if (rt.Right != Right)
            {
                return false;
            }

            if (rt.Bottom != Bottom)
            {
                return false;
            }

            return true;
        }

        public static bool operator ==(CRect cp0, CRect cp1)
        {
            return cp0.Equals(cp1);
        }

        public static bool operator !=(CRect cp0, CRect cp1)
        {
            return !cp0.Equals(cp1);
        }

        public static CRect operator +(CRect rt, CPoint cp)
        {
            return new CRect(rt.Left + cp.X, rt.Top + cp.Y, rt.Right + cp.X, rt.Bottom + cp.Y);
        }

        public static CRect operator -(CRect rt, CPoint cp)
        {
            return new CRect(rt.Left - cp.X, rt.Top - cp.Y, rt.Right - cp.X, rt.Bottom - cp.Y);
        }
        public bool IsInside(CPoint rt)
        {
            if (rt.X > Right)
            {
                return false;
            }

            if (rt.Y > Bottom)
            {
                return false;
            }

            if (rt.X < Left)
            {
                return false;
            }

            if (rt.Y < Top)
            {
                return false;
            }

            return true;
        }
        public CPoint Center()
        {
            return new CPoint(Left + Width / 2, Top + Height / 2);
            //return new CPoint(X, Y);
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
