using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class ViewMatrix
    {
        private double[][] Matrix;

        public ViewMatrix()
        {
            Matrix = new double[4][];
            for (int i = 0; i < 4; i++)
                Matrix[i] = new double[4];

            Matrix[0][0] = Matrix[1][1] = Matrix[2][2] = Matrix[3][3] = 1;
        }

        public double[][] ToMatrix()
        {
            return Matrix;
        }

        public double[][] RotateX(double x)
        {
            Matrix = MyMath.Multiply(Matrix, CreateRotateXMatrix(x));
            return Matrix;
        }

        public double[][] CreateRotateXMatrix(double deg)
        {
            double angle = MyMath.Deg(deg);

            var result = new double[4][];
            for (int i = 0; i < 4; i++)
                result[i] = new double[4];

            result[0][0] = result[3][3] = 1;
            result[1][1] = result[2][2] = (double)Math.Cos(angle);
            result[1][2] = (double)-Math.Sin(angle);
            result[2][1] = (double)Math.Sin(angle);

            return result;
        }

    }
}
