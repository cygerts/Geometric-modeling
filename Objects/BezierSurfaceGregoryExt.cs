using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class BezierSurfaceGregoryExt : BezierSurface
    {
        public MyPoint[,] VerticalSubControlPoints;
        public bool isC1 = false;
        public MyPoint[,] SubControlPoints;
        public BezierSurfaceGregoryExt(MyPoint start, MyPoint end, int dir)
            : base(start, end, dir)
        {
            SubControlPoints = new MyPoint[7,7];
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                    SubControlPoints[i, j] = new MyPoint();
      
            VerticalSubControlPoints = new MyPoint[4, 7];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 7; j++)
                    VerticalSubControlPoints[i, j] = new MyPoint();
        }

        public void UpdateMainSurface()
        {
            return;
            MyPoint[,] temp = new MyPoint[7, 4];
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 4; j++)
                    temp[i, j] = new MyPoint();
            
            MyPoint[] beta = new MyPoint[7];
            for (int i = 0; i < 7; i++){
                beta[i] = new MyPoint();
            }

            for (int m = 0; m < 7; m++)
            {
                for (int v = 0; v < 7; v++)
                    beta[v].SetCoordinates(SubControlPoints[m, v]);
                temp[m, 0].SetCoordinates(SubControlPoints[m, 0]);
                temp[m, 3].SetCoordinates(SubControlPoints[m, 6]);
                double t = 0.5;
                double OneMinusT = 0.5;
                for (int max = 6 - 1; max >= 0; --max)
                {
                    for (int j = 0; j <= max; ++j)
                        beta[j].SetInterpolatedCoordinates(beta[j], beta[j + 1], t);// = beta[j] * OneMinusT + beta[j + 1] * t;
                    if (max == 2)
                    {
                        temp[m, 1].SetCoordinates(beta[0]);
                        temp[m, 2].SetCoordinates(beta[2]);
                    }
                }
            }

            for (int n = 0; n < 4; n++)
            {
                for (int v = 0; v < 7; v++)
                    beta[v].SetCoordinates(temp[v, n]);
                ControlPoints[0, n].SetCoordinates(beta[0]);
                ControlPoints[3, n].SetCoordinates(beta[6]);
                double t = 0.5;
                double OneMinusT = 0.5;
                for (int max = 6 - 1; max >= 0; --max)
                {
                    for (int j = 0; j <= max; ++j)
                        beta[j].SetInterpolatedCoordinates(beta[j], beta[j + 1], t);// = beta[j] * OneMinusT + beta[j + 1] * t;
                    if (max == 2)
                    {
                        ControlPoints[1, n].SetCoordinates(beta[0]);
                        ControlPoints[2, n].SetCoordinates(beta[2]);
                    }
                }
            }
        }

        private void CalculateSubSurfaces()
        {
            if (VerticalSubControlPoints == null)
                return;
            MyPoint[] beta = new MyPoint[4];
            for (int i = 0; i < 4; i++)
                beta[i] = new MyPoint();
            for (int i = 0; i < 4; i++)
            {
                beta[0].SetCoordinates(ControlPoints[i, 0]);
                beta[1].SetCoordinates(ControlPoints[i, 1]);
                beta[2].SetCoordinates(ControlPoints[i, 2]);
                beta[3].SetCoordinates(ControlPoints[i, 3]);
                VerticalSubControlPoints[i, 0].SetCoordinates(beta[0]);
                VerticalSubControlPoints[i, 6].SetCoordinates(beta[3]);
                
                double t = 0.5;
                double OneMinusT = 0.5;
                for (int max = 3 - 1; max >= 0; --max)
                {
                    for (int j = 0; j <= max; ++j)
                        beta[j].SetInterpolatedCoordinates(beta[j], beta[j+1], t);// = beta[j] * OneMinusT + beta[j + 1] * t;
                    if (max == 2)
                    {
                        VerticalSubControlPoints[i, 1].SetCoordinates(beta[0]);
                        VerticalSubControlPoints[i, 5].SetCoordinates(beta[2]);
                    }
                    else if (max == 1)
                    {
                        VerticalSubControlPoints[i, 2].SetCoordinates(beta[0]);
                        VerticalSubControlPoints[i, 4].SetCoordinates(beta[1]);
                    }
                    else
                        VerticalSubControlPoints[i, 3].SetCoordinates (beta[0]);
                }
            }

            for (int i = 0; i < 7; i++)
            {
                beta[0].SetCoordinates(VerticalSubControlPoints[0, i]);
                beta[1].SetCoordinates(VerticalSubControlPoints[1,i]);
                beta[2].SetCoordinates(VerticalSubControlPoints[2,i]);
                beta[3].SetCoordinates(VerticalSubControlPoints[3,i]);
                SubControlPoints[0,i].SetCoordinates(beta[0]);
                SubControlPoints[6,i].SetCoordinates(beta[3]);
                
                double t = 0.5;
                double OneMinusT = 0.5;
                for (int max = 3 - 1; max >= 0; --max)
                {
                    for (int j = 0; j <= max; ++j)
                        beta[j].SetInterpolatedCoordinates(beta[j], beta[j+1], t);// = beta[j] * OneMinusT + beta[j + 1] * t;
                    if (max == 2)
                    {
                        SubControlPoints[1,i].SetCoordinates(beta[0]);
                        SubControlPoints[5, i].SetCoordinates(beta[2]);
                    }
                    else if (max == 1)
                    {
                        SubControlPoints[2, i].SetCoordinates(beta[0]);
                        SubControlPoints[4, i].SetCoordinates(beta[1]);
                    }
                    else
                        SubControlPoints[3, i].SetCoordinates(beta[0]);
                }
            }
            UpdateMainSurface();
        }

        public override void CalculatePoints()
        {
            base.CalculatePoints();
            CalculateSubSurfaces();
        }
    }
}
