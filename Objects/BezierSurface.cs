using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class BezierSurface
    {
        public BezierSurface(int degree, int m, int n, int l)
        {
            Degree = degree;
            x = new BernsteinPolynomial(degree);
            y = new BernsteinPolynomial(degree);
            z = new BernsteinPolynomial(degree);
            tx = new BernsteinPolynomial(degree);
            ty = new BernsteinPolynomial(degree);
            tz = new BernsteinPolynomial(degree);
            mSize = m; //columns
            nSize = n; //lines    
            points = new List<List<MyPoint>>();
     
            ControlPoints = new MyPoint[this.mTableSize(), this.nTableSize()];
            CreateControlPoints(l);
        }
        public int divisions = 10;
        public List<List<MyPoint>> points;
        public BezierSurface(MyPoint start, MyPoint end, int dir)
        {
            Degree = 3;
            x = new BernsteinPolynomial(Degree);
            y = new BernsteinPolynomial(Degree);
            z = new BernsteinPolynomial(Degree);
            tx = new BernsteinPolynomial(Degree);
            ty = new BernsteinPolynomial(Degree);
            tz = new BernsteinPolynomial(Degree);
            mSize = 1;
            nSize = 1;

            points = new List<List<MyPoint>>();
            ControlPoints = new MyPoint[4, 4];
            CreateControlPoints(start, end, dir);
       
        }

        public void CreateControlPoints(MyPoint start, MyPoint end, int dir)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    ControlPoints[i, j] = new MyPoint();
            ControlPoints[0, 0].SetCoordinates(start);
            ControlPoints[0, 3].SetCoordinates(end);
            ControlPoints[0, 1].SetInterpolatedCoordinates(ControlPoints[0, 0], ControlPoints[0, 3], 0.66);
            ControlPoints[0, 2].SetInterpolatedCoordinates(ControlPoints[0, 0], ControlPoints[0, 3], 0.33);
     
            double distance = MyMath.distance(start, end);
            if (dir == 0)
            {
                for(int i = 1 ; i < 4 ; i++)
                    for(int j = 0 ; j < 4 ; j++)
                        ControlPoints[i, j].SetCoordinates(ControlPoints[i - 1, j].x, ControlPoints[i - 1, j].y - 0.33 * distance, ControlPoints[i - 1, j].z); 
            }
            else
                if (dir == 1)
                {
                    for (int i = 1; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            ControlPoints[i, j].SetCoordinates(ControlPoints[i - 1, j].x + 0.2 * distance, ControlPoints[i - 1, j].y + 0.2 * distance, ControlPoints[i - 1, j].z);
                }
                else
                {
                    for (int i = 1; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            ControlPoints[i, j].SetCoordinates(ControlPoints[i - 1, j].x - 0.2 * distance, ControlPoints[i - 1, j].y + 0.2 * distance, ControlPoints[i - 1, j].z);
     
                }
            CalculatePoints();
 }

        public int Degree { get; private set; }
        public int mSize { get; private set; }
        public int nSize { get; private set; }
        protected int _selectedPoint = 0;
        protected int _selectedBoth = 0;
        protected bool both = false;
        protected BernsteinPolynomial x, y, z, tx, ty, tz;
        public List<MyPoint> verticalDeBoorPoints = new List<MyPoint>();
        public List<MyPoint> horizontalDeBoorPoints = new List<MyPoint>();

        public MyPoint[,] ControlPoints { get; private set; }

        public virtual MyPoint Calculate(double u, double v, int indexX, int indexY)
        {
            for (int i = 0; i <= Degree; ++i)
            {
                for (int j = 0; j <= Degree; ++j)
                {
                    tx[j] = ControlPoints[i + Degree * indexX, j + Degree * indexY].x;
                    ty[j] = ControlPoints[i + Degree * indexX, j + Degree * indexY].y;
                    tz[j] = ControlPoints[i + Degree * indexX, j + Degree * indexY].z;
                }

                x[i] = tx.Calculate(v);
                y[i] = ty.Calculate(v);
                z[i] = tz.Calculate(v);
            }

            return new MyPoint(x.Calculate(u), y.Calculate(u), z.Calculate(u));
        }

        public virtual void CalculatePoints()
        {
            points.Clear();
            int mSize = this.mSize;
            int nSize = this.nSize;
            double du = (double)1 / (divisions - 1);
            double dv = (double)1 / (divisions - 1);
            List<MyPoint> subList = new List<MyPoint>();
            for (int q = 0; q < mSize; q++)
            {
                for (int w = 0; w < nSize; w++)
                    for (int i = 0; i < divisions; ++i)
                    {  //adaptacyjna liczba podzialow!!
                        double u = du * i;
                        for (int j = 0; j < divisions; ++j)
                        {
                            double v = dv * j;
                            subList.Add(Calculate(u, v, q, w));  //
                        }
                        points.Add(subList);
                        subList = new List<MyPoint>();
                    }
            }
        }

     

        public void ClearSelectedPoint()
        {
            _selectedPoint = 0;
        }

        public int mTableSize() //# of columns
        {
            return Degree + 1 + (mSize - 1) * 3;
        }

        public int nTableSize() //# of lines
        {
            return Degree + 1 + (nSize - 1) * 3;
        }

        public int numOfVerticalDeBoorPoints()
        {
            return 4 + nSize - 1;
        }

        public int numOfHorizontalDeBoorPoints()
        {
            return 4 + mSize - 1;
        }

        protected int DeBoorControl(int index)
        {
            return 1 + (index - 2) * 3;
        }

        protected virtual void CreateControlPoints(int l)
        {
            if (l != 1)
                for (int i = 0; i < this.mTableSize(); i++)
                    for (int j = 0; j < this.nTableSize(); j++)
                        ControlPoints[i, j] = new MyPoint(i * 30, j * 30, 0);
            else
                for (int i = 0; i < this.mTableSize(); i++)
                    for (int j = 0; j < this.nTableSize(); j++)
                        ControlPoints[i, j] = new MyPoint(0, j * 30, i * 30);
            CalculatePoints();

        }

      }
}
