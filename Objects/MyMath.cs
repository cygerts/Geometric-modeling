using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    static class MyMath
    {
        public static double Deg(double angle)
        {
            return (double)Math.PI * angle / 180.0F;
        }

        public static MyPoint ScalarProduct(MyPoint p1, MyPoint p2)
        {
            return new MyPoint(p1.y * p2.z - p1.z*p2.y, -p1.x * p2.z + p2.x * p1.z, p1.x * p2.y - p2.x * p1.y);
        }

        public static MyPoint Substract(MyPoint p1, MyPoint p2)
        {
            return new MyPoint(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
        }

        public static MyPoint Add(MyPoint p1, MyPoint p2)
        {
            return new MyPoint(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
        }

        public static double distance(MyPoint p1,  double x2,  double y2,  double z2)
        {
            return Math.Sqrt(Math.Pow(p1.x - x2,2) + Math.Pow(p1.y - y2,2) + Math.Pow(p1.z - z2,2));
        }

        public static double distance(MyPoint p1, MyPoint p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2) + Math.Pow(p1.z - p2.z, 2));
        }

        public static double[][] Multiply(double[][] m1, double[][] m2)
        {
            int Acolumns = m1[0].Length;
            int ARows = m1.Length;
            int BRows = m2.Length;
            int BColumns = m2[0].Length;
            var result = new double[ARows][];
            for (int i = 0; i < ARows; i++)
                result[i] = new double[BColumns];

            for (int i = 0; i < ARows; i++)
            {
                for (int j = 0; j < BColumns; j++)
                {
                    result[i][j] = 0;
                    for (int k = 0; k < Acolumns; k++)
                        result[i][j] += m1[i][k] * m2[k][j];
                }
            }
            return result;
        }

        public static double[,] Multiply2(double[,] m1, double[,] m2)
        {
            int Acolumns = m1.GetLength(1);
            int ARows = m1.GetLength(0);
            int BRows = m2.GetLength(0);
            int BColumns = m2.GetLength(1);
            var result = new double[ARows, BColumns];
         
            for (int i = 0; i < ARows; i++)
            {
                for (int j = 0; j < BColumns; j++)
                {
                    result[i,j] = 0;
                    for (int k = 0; k < Acolumns; k++)
                        result[i,j] += m1[i,k] * m2[k,j];
                }
            }
            return result;
        }

        public static double[][] InverseMatrix(double[][] matrix)

        {
            var result = new double[matrix.Length][];
            for(int i = 0 ; i < matrix.Length; i++)
                matrix[i] = new double[matrix[0].Length];


            return result;
        }

        public static double[][] Transpose(double[][] m1)
        {
            var result = new double[m1[0].Length][];
            for (int i = 0; i < m1[0].Length; i++)
                result[i] = new double[m1.Length];
            for (int i = 0; i < m1[0].Length; i++)
                for (int j = 0; j < m1.Length; j++)
                    result[i][j] = m1[j][i];

            return result;
        }

        public static double[][] Opposite(double [][] m1)
        {
            var result = new double[m1[0].Length][];
            for (int i = 0; i < m1[0].Length; i++)
                result[i] = new double[m1.Length];

            for (int i = 0; i < m1[0].Length; i++)
                for (int j = 0; j < m1.Length; j++)
                    result[i][j] = -m1[i][j];
            result[3][3] = m1[3][3];
            return result;
        }

        public static double DotProduct(MyPoint p1, MyPoint p2)
        {
            return p1.x * p2.x + p1.y * p2.y + p1.z * p2.z;
        }

        public static double[][] invertedProjection(double[][] m1)
        {
            var result = new double[m1[0].Length][];
            for (int i = 0; i < m1[0].Length; i++)
                result[i] = new double[m1.Length];

            for (int i = 0; i < m1[0].Length; i++)
                for (int j = 0; j < m1.Length; j++)
                {
                    if (m1[i][j] != 0)
                        result[i][j] = 1.0F/m1[i][j];
                    else result[i][j] = 0;
                }

       
    return result;
        }

        public static MyPoint Medium(MyPoint p1, MyPoint p2, MyPoint p3)
        {
            return new MyPoint((p1.x + p2.x + p3.x) / 3, (p1.y + p2.y + p3.y) / 3, (p1.z + p2.z + p3.z) / 3);
        }

        public static MyPoint AddDifference(MyPoint start, MyPoint end)
        {
            MyPoint temp = Difference(start, end);
            return new MyPoint(start.x + temp.x, start.y + temp.y, start.z + temp.z);
        }

        public static MyPoint AddVector(MyPoint begin, MyPoint vec)
        {
            return new MyPoint(begin.x + vec.x, begin.y + vec.y, begin.z + vec.z);
        }

        public static MyPoint AddDifference2(MyPoint start, MyPoint end, double factor)
        {
            MyPoint temp = Difference2(start, end);
            return new MyPoint(start.x + factor * temp.x, start.y + factor * temp.y, start.z + factor * temp.z);
        }
        public static MyPoint AddDifference(MyPoint start, MyPoint end, double factor)
        {
            MyPoint temp = Difference(start, end);
            return new MyPoint(start.x + factor*temp.x, start.y + factor*temp.y, start.z + factor*temp.z);
        }


        public static MyPoint Difference(MyPoint from, MyPoint to)
        {
            return new MyPoint(from.x - to.x, from.y - to.y, from.z - to.z);
        }

        public static MyPoint Difference2(MyPoint from, MyPoint to)
        {
            return new MyPoint(to.x - from.x, to.y - from.y, to.z - from.z);
        }

        public static double CalculateBernstein3(double t, int number)
        {
            switch (number)
            {
                case 0:
                    return Math.Pow(1 - t, 3);
                case 1:
                    return 3*Math.Pow(1 - t, 2)*t;
                case 2:
                    return 3*(1 - t)*t*t;
                case 3:
                    return Math.Pow(t, 3);
                default:
                    return 0;
            }
        }

        public static MyPoint BernstetInterpolation(MyPoint g0, MyPoint g3, MyPoint g1, double t)
        {
            double x, y, z;
            x = CalculateBernstein2(t, 2) * g0.x;
            x = x + CalculateBernstein2(t, 1) * g1.x;
            x = x + CalculateBernstein2(t, 2) * g3.x;
            y = CalculateBernstein2(t, 2) * g0.y;
            y = y + CalculateBernstein2(t, 1) * g1.y;
            y = y + CalculateBernstein2(t, 2) * g3.y;
            z = CalculateBernstein2(t, 2) * g0.z;
            z = z + CalculateBernstein2(t, 1) * g1.z;
            z = z + CalculateBernstein2(t, 2) * g3.z;
            return new MyPoint(x, y, z);


        }

        public static double CalculateBernstein2(double t, int number)
        {
            switch (number)
            {
                case 0:
                    return Math.Pow(1 - t, 2);
                case 1:
                    return 2 * (1-t) * t;
                case 2:
                    return t * t;
                default:
                    return 0;
            }
        }
    }
}
