using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    class ProjectionMatrix
    {
        public double[][] Matrix;
        
        public ProjectionMatrix(double distance)
        {
            Matrix = new double[4][];
            for (int i = 0; i < 4; i++)
                Matrix[i] = new double[4];

            Matrix[0][0] = Matrix[1][1] = Matrix[2][2] = Matrix[3][3] = 1;
            Matrix[3][2] = 1.0F / distance ;
        }

        public double[][] Set(double distance)
        {
            Matrix[3][2] = 1.0F / distance ;
            return Matrix;
        }

        public double[][] ToMatrix()
        {
            return Matrix;
        }

        public double[][] GetProjectionMatrixTranslated(double d)
        {



            return Matrix;
        }
    }
}
