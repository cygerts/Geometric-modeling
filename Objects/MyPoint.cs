using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class MyPoint
    {
        public double x, y, z, w;
        public double u, v;
        //private double Xscreen, YScreen;
        private double[][] vector;
        public MyPoint(double x1, double y1, double z1)
        {
            x = x1;
            y = y1;
            z = z1;
            w = 1.0F;
        }

        public MyPoint(double u, double v, double s, double t)
        {
            x = u;
            y = v;
            z = s;
            w = t;
        }

        public MyPoint(double[][] vector)
        {
            x = vector[0][0];
            y = vector[1][0];
            z = vector[2][0];
            w = vector[3][0];
        }

        public void Normalized()
        {
            double det = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            x /= det;
            y /= det;
            z /= det;
           // w /= det;
        }

        public MyPoint()
        {
            x = y = z = 0;
            w = 1.0F;

        /*    vector = new double[4][];
            for(int i = 0 ; i < 4; i++)
                vector[i] = new double[4];*/
       
        }

        public void SetCoordinates(ref double x1, ref double y1, ref double z1)
        {
            x = x1;
            y = y1;
            z = z1;
        }

        public void SetInterpolatedCoordinates(MyPoint p1, MyPoint p2, double factor1)
        {
            x = factor1 * p1.x + (1.0f-factor1) * p2.x;
            y = factor1 * p1.y + (1.0f - factor1) * p2.y;
            z = factor1 * p1.z + (1.0f - factor1) * p2.z;
        }

        public void SetCoordinates( double x1,  double y1,  double z1)
        {
            x = x1;
            y = y1;
            z = z1;
        }

        public void CheckParam()
        {
            //x = x > 1.0 ? x - 1.0 : x < 0.0 ? x + 1.0 : x;
            //y = y > 1.0 ? y- 1.0 : y < 0.0 ? y + 1.0 : y;
            //z = z > 1.0 ? z - 1.0 : z < 0.0 ? z + 1.0 : z;
            //w = w > 1.0 ? w - 1.0 : w < 0.0 ? w + 1.0 : w;
            x = x > 1.0 ? 0.0 : x < 0.0 ? 1.0 : x;
            y = y > 1.0 ? 0.0 : y < 0.0 ? 1.0 : y;
            z = z > 1.0 ? 0.0 : z < 0.0 ? 1.0 : z;
            w = w > 1.0 ? 0.0 : w < 0.0 ? 1.0 : w;
     
        }
        
        public void SetCoordinates(MyPoint point)
        {
            x = point.x;
            y = point.y;
            z = point.z;
        }

        public void SetCoordinatesParam(MyPoint point)
        {
            x = point.x;
            y = point.y;
            z = point.z;
            w = point.w;
        }


        public void Add( MyPoint point)
        {
            x += point.x;
            y += point.y;
            z += point.z;
        }

        public MyPoint Multiply(double val)
        {
            x *= val;
            y *= val;
            z *= val;
            return this;
        }

        public double[][] ToVector()
        {
            vector = new double[4][];
            for(int i = 0 ; i < 4; i++)
                vector[i] = new double[4];

            vector[0][0] = x;
            vector[1][0] = y;
            vector[2][0] = z;
            vector[3][0] = w;

            return vector;
        }

        public MyPoint Normalize()
        {
            double normalizer = 1 / w;
            x *= normalizer;
            y *= normalizer;
            z *= normalizer;
            w *= normalizer;
            
            return this;
        }
    }
}
