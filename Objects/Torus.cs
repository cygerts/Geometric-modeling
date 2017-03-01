using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    class Torus : Object
    {
        //private Form1.ObjectType objectType = Form1.ObjectType.InterpolationCurve;
        private readonly int r;
        private readonly int R;
        double betaShift, alfaShift; 
            //angleShift;
        
        public Torus(int r1, int R1, int div)
        {
            r = r1;
            R = R1;
            UpdatePoints(div);
        }

        public override List<MyPoint> UpdatePoints(int div)
        {
            Points.Clear();
            betaShift = 360 / (double)div;
            alfaShift = 360 / (double)div;
            for (int i = 0; i < div; i++)
                for (int j = 0; j < div; j++)
                {
                    Points.Add(new MyPoint((R + r * (double)Math.Cos(MyMath.Deg((j * betaShift)))) * (double)Math.Cos(MyMath.Deg((i * alfaShift))), (R + r * (double)Math.Cos(MyMath.Deg((j * betaShift)))) * (double)Math.Sin(MyMath.Deg((i * alfaShift))), r * (double)Math.Sin(MyMath.Deg((j * betaShift)))));
                    Points.Last().Multiply(10);
                }
            return Points;
        }
    }
}
