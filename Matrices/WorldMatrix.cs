using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    class WorldMatrix
    {
        private double[][] Matrix;
        public double[][] TrMatrix;
        public double[][] ScMatrix;
        public double[][] RotMatrix; 


        public WorldMatrix()
        {
            Matrix = new double[4][];
            TrMatrix = new double[4][];
            ScMatrix = new double[4][];
            RotMatrix = new double[4][];
            for (int i = 0; i < 4; i++)
            {
                Matrix[i] = new double[4];
                TrMatrix[i] = new double[4];
                ScMatrix[i] = new double[4];
                RotMatrix[i] = new double[4];
                
                TrMatrix[i][i] = 1;
                RotMatrix[i][i] = 1;
                ScMatrix[i][i] = 1;
                Matrix[i][i] = 1;
            }
            TrMatrix[3][3] = 1;
        }

        public double[][] CreateTranslateMatrix(double x, double y, double z)
        {
            var TranslateMatrix = new double[4][];
            for (int i = 0; i < 4; i++)
            {
                TranslateMatrix[i] = new double[4];
                TranslateMatrix[i][i] = 1;
            }

            TranslateMatrix[0][3] = x;
            TranslateMatrix[1][3] = y;
            TranslateMatrix[2][3] = z;
            return TranslateMatrix;
        }

        protected double[][] CreateScaleMatrix(double x, double y, double z)
        {
            var ScaleMatrix = new double[4][];
            for (int i = 0; i < 4; i++)
            {
                ScaleMatrix[i] = new double[4];
            }

            ScaleMatrix[0][0] = x;
            ScaleMatrix[1][1] = y;
            ScaleMatrix[2][2] = z;
            ScaleMatrix[3][3] = 1;
            return ScaleMatrix;
        }

        public double[][] Scale(double x, double y, double z)
        {
            Matrix = MyMath.Multiply(Matrix, CreateScaleMatrix(x, y, z));
            ScMatrix = MyMath.Multiply(ScMatrix, Matrix);
            return Matrix;
        }

        public double[][] Translate(double x, double y, double z)
        {
            Matrix =  MyMath.Multiply(Matrix, CreateTranslateMatrix(x, y, z));
            TrMatrix = MyMath.Multiply(TrMatrix, Matrix);
            return Matrix;
        }

        public double[][] ToMatrix()
        {
            return Matrix;
        }

        public double[][] RotateX(double x)
        {
            Matrix = MyMath.Multiply(CreateRotateXMatrix(x),Matrix);
            RotMatrix = MyMath.Multiply(RotMatrix, Matrix);
            return Matrix;
        }

        public double[][] RotateY(double x)
        {
            Matrix = MyMath.Multiply(CreateRotateYMatrix(x),Matrix);
            RotMatrix = MyMath.Multiply(RotMatrix, Matrix);
            return Matrix;
        }

        public double[][] RotateZ(double x)
        {
            Matrix = MyMath.Multiply(CreateRotateZMatrix(x),Matrix);
            RotMatrix = MyMath.Multiply(RotMatrix, Matrix);
            return Matrix;
        }

        private double[][] CreateRotateXMatrix(double deg)
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

        private double[][] CreateRotateYMatrix(double deg)
        {
            double angle = MyMath.Deg(deg);

            var result = new double[4][];
            for (int i = 0; i < 4; i++)
                result[i] = new double[4];

            result[0][0] = (double)Math.Cos(angle);
            result[0][2] = (double)Math.Sin(angle);
            result[1][1] = 1;
            result[2][0] = (double)-Math.Sin(angle);
            result[2][2] = (double)Math.Cos(angle);
            result[3][3] = 1;

            return result;
        }

        private double[][] CreateRotateZMatrix(double deg)
        {
            double angle = MyMath.Deg(deg);

            var result = new double[4][];
            for (int i = 0; i < 4; i++)
                result[i] = new double[4];

            result[0][0] = (double)Math.Cos(angle);
            result[0][1] = (double)-Math.Sin(angle);
            result[1][0] = (double)Math.Sin(angle);
            result[1][1] = (double)Math.Cos(angle);
            result[2][2] = 1;
            result[3][3] = 1;

            return result;
        }

    }
}
