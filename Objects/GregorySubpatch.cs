using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class GregorySubpatch
    {
        BezierSurfaceGregoryExt left;
        private int Degree = 3;
        BezierSurfaceGregoryExt right;
        public MyPoint[] ControlPoints = new MyPoint[20];
        public MyPoint[,] ControlP = new MyPoint[4,6];
        protected BernsteinPolynomial x, y, z, tx, ty, tz;
        public int divisions = 10;
        public List<List<MyPoint>> points;
        public GregorySubpatch(BezierSurfaceGregoryExt b1, BezierSurfaceGregoryExt b2)
        {
            left = b1;
            right = b2;
            for (int i = 0; i < 20; i++)
                ControlPoints[i] = new MyPoint();
            for (int i = 0; i < 4; i++)
                for(int j = 0 ; j < 6 ; j++)
                ControlP[i, j] = new MyPoint();
            points = new List<List<MyPoint>>();
            x = new BernsteinPolynomial(3);
            y = new BernsteinPolynomial(3);
            z = new BernsteinPolynomial(3);
            tx = new BernsteinPolynomial(3);
            ty = new BernsteinPolynomial(3);
            tz = new BernsteinPolynomial(3);
        }

        public void CalculateControlPoints()
        {
            if (left.VerticalSubControlPoints == null || right.VerticalSubControlPoints == null)
                return;
            for (int i = 0; i < 4; i++)
                ControlPoints[i].SetCoordinates(left.SubControlPoints[0, 3 + i]);
            for (int j = 0; j < 3; j++)
                ControlPoints[j + 4].SetCoordinates(right.SubControlPoints[0, j + 1]);
            for (int i = 0; i < 3; i++)
            {
                ControlPoints[i + 7].SetCoordinates(MyMath.AddDifference(left.SubControlPoints[0,3+i], left.SubControlPoints[1, 3+i]));
            }
            for (int i = 0; i < 3; i++)
            {
                ControlPoints[i + 10].SetCoordinates(MyMath.AddDifference(right.SubControlPoints[0, 1 + i], right.SubControlPoints[1, 1 + i]));
            }
            ControlPoints[13].SetCoordinates(MyMath.AddDifference(ControlPoints[7], ControlPoints[0], 0.5));
        }

        public  void UpdateControlTable()
        {
            ControlP[0, 0].SetCoordinates(ControlPoints[13]);
            ControlP[0, 1].SetCoordinates(ControlPoints[15]);
            ControlP[0, 2].SetCoordinates(ControlPoints[12]);
            ControlP[0, 3].SetCoordinates(ControlPoints[6]);
            ControlP[1, 0].SetCoordinates(ControlPoints[14]);
            ControlP[1, 1].SetCoordinates(ControlPoints[18]);
            ControlP[1, 2].SetCoordinates(ControlPoints[16]);
            ControlP[1, 3].SetCoordinates(ControlPoints[5]);
            ControlP[1, 4].SetCoordinates(ControlPoints[17]); //*
            ControlP[1, 5].SetCoordinates(ControlPoints[11]); //**
            ControlP[2, 0].SetCoordinates(ControlPoints[7]);
            ControlP[2, 1].SetCoordinates(ControlPoints[19]);
            ControlP[2, 2].SetCoordinates(ControlPoints[9]);
            ControlP[2, 3].SetCoordinates(ControlPoints[4]);
            ControlP[2, 4].SetCoordinates(ControlPoints[8]); //*
            ControlP[2, 5].SetCoordinates(ControlPoints[10]); //**
            ControlP[3, 0].SetCoordinates(ControlPoints[0]);
            ControlP[3, 1].SetCoordinates(ControlPoints[1]);
            ControlP[3, 2].SetCoordinates(ControlPoints[2]);
            ControlP[3, 3].SetCoordinates(ControlPoints[3]);
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            points.Clear();
            double du = (double)1 / (divisions - 1);
            double dv = (double)1 / (divisions - 1);
            List<MyPoint> subList = new List<MyPoint>();
                    for (int i = 0; i < divisions; i++)
                    {  //adaptacyjna liczba podzialow!!
                        double u = du * i;
                        for (int j = 0; j < divisions; j++)
                        {
                            double v = dv * j;
                            subList.Add(Calculate(u, v, 0, 0));  //q,w
                        }
                        points.Add(subList);
                        subList = new List<MyPoint>();
                    }
          //  }
        }


        public MyPoint Calculate(double u, double v, int indexX, int indexY)
        {
            for (int i = 0; i <= Degree; ++i)
            {
                for (int j = 0; j <= Degree; ++j)
                {
                    if (i == 1 && j == 1)
                    {
                        tx[j] = (u * ControlP[i, j].x + (1 - v) * ControlP[i, j + 3].x) / (1 + u - v);
                        ty[j] = (u * ControlP[i, j].y + (1 - v) * ControlP[i, j + 3].y) / (u + 1 - v);
                        tz[j] = (u * ControlP[i, j].z + (1 - v) * ControlP[i, j + 3].z) / (u + 1 - v);
                    }
                    else if (i == 2 && j == 1)
                    {
                        if (1 - u + v == 0)
                        {
                            tx[j] = ControlP[i, j].x;// + v * ControlP[i, j + 3].x) / (1 - u + v);
                            ty[j] = ControlP[i, j].y;// + v * ControlP[i, j + 3].y) / (1 - u + v);
                            tz[j] = ControlP[i, j].z;// + v * ControlP[i, j + 3].z) / (1 - u + v);
                     
                        }
                        else
                        {
                            tx[j] = ((1 - u) * ControlP[i, j].x + v * ControlP[i, j + 3].x) / (1 - u + v);
                            ty[j] = ((1 - u) * ControlP[i, j].y + v * ControlP[i, j + 3].y) / (1 - u + v);
                            tz[j] = ((1 - u) * ControlP[i, j].z + v * ControlP[i, j + 3].z) / (1 - u + v);
                        }
                    }
                    else if (i == 2 && j == 2)
                    {
                        tx[j] = ((1 - u) * ControlP[i, j].x + (1 - v) * ControlP[i, j + 3].x) / (2 - u - v);
                        ty[j] = ((1 - u) * ControlP[i, j].y + (1 - v) * ControlP[i, j + 3].y) / (2 - u - v);
                        tz[j] = ((1 - u) * ControlP[i, j].z + (1 - v) * ControlP[i, j + 3].z) / (2 - u - v);
                    }
                    else
                    {
                        tx[j] = ControlP[i + Degree * indexX, j + Degree * indexY].x; 
                        ty[j] = ControlP[i + Degree * indexX, j + Degree * indexY].y;
                        tz[j] = ControlP[i + Degree * indexX, j + Degree * indexY].z;
                    }
                }
                x[i] = tx.Calculate(v);
                y[i] = ty.Calculate(v);
                z[i] = tz.Calculate(v);
            }

            return new MyPoint(x.Calculate(u), y.Calculate(u), z.Calculate(u));
        }
    }
}
