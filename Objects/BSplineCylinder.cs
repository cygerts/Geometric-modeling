using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class BSplineCylinder : BSplineSurface
    {
        public BSplineCylinder(int degree, int m, int n, int l)
            : base(degree, m, n, l)
        {
            isCylinder = true;
        }

        public override void AddDeBoorPoints()
        {
            for (int i = 0; i < this.mSize; i++)
            {
                double r = 25;
                double v = ((double)i) / ((double)this.mSize);
                v *= Math.PI * 2.0f;
                for (int j = 0; j < this.nSize; j++)
                {
                    DeBoorControlPoints[i, j] = new MyPoint(- nSize * r / 2 + r* j, Math.Cos(v) * 2*r, Math.Sin(v) * 2*r);
                }
            }
            CalculatePoints();
        }

        public override void CreateMain()
        {
            //m = 10 , n = 20    
            int breakinPart = 17;
            int brakinPart2 = 7;
           
            for (int i = 0; i < this.mSize; i++)
                {
                    double r = 25;
                    double r2 = 25;
                    double r3 = 7;    
                    double v = ((double)i) / ((double)this.mSize);
                    v *= Math.PI * 2.0f;
                    for (int j = 0; j < this.nSize; j++)
                    {
                        if (j < brakinPart2)
                        {
                            DeBoorControlPoints[i, j] = new MyPoint(-nSize * (r+2) / 2 + r * j, Math.Cos(v) * 2 * r3, Math.Sin(v) * 2 * r3);
                            r3 += 3.0f;
                        }
                        else if (j < breakinPart)
                            DeBoorControlPoints[i, j] = new MyPoint(-nSize * (r+2) / 2 + r * j, Math.Cos(v) * 2 * r, Math.Sin(v) * 2 * r);
                        else
                        {
                            r2 -= 8.0f;
                            DeBoorControlPoints[i, j] = new MyPoint(-nSize * (r+2) / 2 + r * breakinPart + (j - breakinPart) * (r / 2.0f), Math.Cos(v) * 2 * r2, Math.Sin(v) * 2 * r2);
                        }
                    
                    }
                }
            for (int i = 0; i < this.mSize ; i++)
            {
                if (i != 0 && i!=3 && i!=6 && i!=9)
                    continue;
                double r = 25;
                double r4 = 60;
                double r3 = 9;
                double v = ((double)i) / ((double)this.mSize);
                v *= Math.PI * 2.0f;
                for (int j = 0; j < 3; j++)
                {
                    DeBoorControlPoints[i, j] = new MyPoint(-nSize * r / 2 + r * j, Math.Cos(v) * 2 * r4, Math.Sin(v) * 2 * r4);
                    if (j  < 2)
                        DeBoorControlPoints[i, j].x -= r;
                    //       
           //         if (j == brakinPart2 + 4)
           //             DeBoorControlPoints[i, j].x += r4;
                }
            }

            MyPoint[,] Temp = new MyPoint[DeBoorControlPoints.GetLength(0), DeBoorControlPoints.GetLength(1)];
            for(int i = 0 ; i < DeBoorControlPoints.GetLength(0); i++)
                for (int j = 0; j < DeBoorControlPoints.GetLength(1); j++)
                {
                    Temp[i, j] = new MyPoint();
                    Temp[i, j].SetCoordinates(DeBoorControlPoints[i, j]);
                }

            for (int i = 0; i < DeBoorControlPoints.GetLength(0); i++)
                for (int j = 0; j < DeBoorControlPoints.GetLength(1); j++)
                    DeBoorControlPoints[i, j] = Temp[DeBoorControlPoints.GetLength(0) - 1 - i, j];

            CalculatePoints();
        }

        public override void CreateRoof()
        {
            //m = 6 , n = 12
            int changedPoints = 7;
            for (int i = 0; i < this.mSize; i++)
            {
                double r = 15;
                double r2 = 5;
                double v = ((double)i) / ((double)this.mSize);
                v *= Math.PI * 2.0f;
                for (int j = 0; j < this.nSize; j++)
                {
                        DeBoorControlPoints[i, j] = new MyPoint(5*r +  Math.Cos(v) * 2 * r, -r +  r * j, Math.Sin(v) * 2 * r);
                }
            }

            int temp = 2;
            for (int i = 0; i < this.mSize; i++)
            {
                double r = 15;
                double r2 = 30;
                double v = ((double)i) / ((double)this.mSize);
                v *= Math.PI * 2.0f;
                if (i == temp || i == temp + 1 || i == temp + 2 || i == temp + 6 || i == temp + 7 || i == temp + 8)
                    DeBoorControlPoints[i, changedPoints] = new MyPoint(5 * r + Math.Cos(v) * 2 * r2, -r + r * changedPoints, Math.Sin(v) * 2 * r2);
            }

            for (int i = 0; i < this.mSize; i++)
            {
                DeBoorControlPoints[i, changedPoints - 1].SetCoordinates(DeBoorControlPoints[i, changedPoints]);
                DeBoorControlPoints[i, changedPoints + 1].SetCoordinates(DeBoorControlPoints[i, changedPoints]);
            }
            CalculatePoints();
        }

        protected double Nik(double t, int i, int k)
        {

            if (k == 0)
            {
                if (t >= _knots[i] && t < _knots[i + 1])
                    return 1.0;
                else
                    return 0.0;
            }
            else
            {
                return ((t - _knots[i]) / (_knots[i + k] - _knots[i])) * Nik(t, i, k - 1) +
                            ((_knots[i + k + 1] - t) / (_knots[i + k + 1] - _knots[i + 1])) * Nik(t, i + 1, k - 1);
            }
        }
        
    }
}
