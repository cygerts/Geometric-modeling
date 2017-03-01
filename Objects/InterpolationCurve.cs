using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using MatrixLibrary;
using DotNumerics;
using DotNumerics.LinearAlgebra;

namespace ModelowanieGeometyczne2
{
    class InterpolationCurve
    {
        //private Form1.ObjectType objectType = Form1.ObjectType.InterpolationCurve;
        public List<MyPoint> ControlPoints = new List<MyPoint>();
        private List<MyPoint> _points = new List<MyPoint>(); 
        private const int N = 3;
        public List<Polynomial> _polyX = new List<Polynomial>();
        public List<Polynomial> _polyY = new List<Polynomial>();
        public List<Polynomial> _polyZ = new List<Polynomial>();
        
        public List<double> chords = new List<double>();

        public void AddPoints(MyPoint p)
        {
       //     if (CheckIfIsNotDublated(p))
       //         return;
            ControlPoints.Add(p);
            UpdateChords();
            UpdateCurve();
        }

        public void Update()
        {
            UpdateChords();
            UpdateCurve();
        }

        private void UpdateChords()
        {
            chords.Clear();
            for (int i = 0; i < ControlPoints.Count - 1; i++)
            {
                chords.Add(MyMath.distance(ControlPoints[i], ControlPoints[i + 1]));
                if (chords.Last() == 0)
                    chords[chords.Count - 1] = 0.0001F;
            }
        }

        private void UpdateCurve()
        {
            int l = ControlPoints.Count;
            if (l > 2)
            {
                var MatrixA = new BandMatrix(4*(l-1), 4*(l-1), 2, 2);  //zamienic na 2x2
                var MatrixBx = new DotNumerics.LinearAlgebra.Matrix(4 * (l-1), 1);
                var MatrixBy = new DotNumerics.LinearAlgebra.Matrix(4 * (l - 1), 1);
                var MatrixBz = new DotNumerics.LinearAlgebra.Matrix(4 * (l - 1), 1);
          
                for (int i = 0; i < l; i++)
                {
                    if (i == 0)  //pierwszy element
                    {
                        MatrixA[0,2] = 2;
                        //MatrixA[0,3] = 6 * chords[i]; //blad
                        MatrixA[1,0] = 1;
                        //MatrixA[1,1] = chords[i];
                        //MatrixA[1,2] = Math.Pow(chords[i], 2);
                        //MatrixA[1,3] = Math.Pow(chords[i], 3);
                        MatrixBx[1,0] = ControlPoints[0].x;
                        MatrixBx[0,0] = 0;
                        MatrixBy[1, 0] = ControlPoints[0].y;
                        MatrixBy[0, 0] = 0;
                        MatrixBz[1, 0] = ControlPoints[0].z;
                        MatrixBz[0, 0] = 0;
                    }
                    else if (i == ControlPoints.Count - 1)
                    {
                        //ostatni element
                        MatrixA[4 * (l - 1) - 2,4 * (l-1) - 4] = 1;
                        MatrixA[4 * (l - 1) -2, 4 * (l-1)-3] = chords[i-1];
                        MatrixA[4 * (l - 1) -2, 4 * (l-1)-2] = Math.Pow(chords[i-1], 2);
                        MatrixA[4 * (l - 1) -2, 4 * (l-1)-1] = Math.Pow(chords[i-1], 3);
                        MatrixA[4 * (l - 1) - 1,4 * (l-1) - 2] = 2;

                        MatrixBx[4 * (l - 1) - 2, 0] = ControlPoints[i].x;
                        MatrixBx[4 * (l - 1) - 1, 0] = 0;
                        MatrixBy[4 * (l - 1) - 2, 0] = ControlPoints[i].y;
                        MatrixBy[4 * (l - 1) - 1, 0] = 0;
                        MatrixBz[4 * (l - 1) - 2, 0] = ControlPoints[i].z;
                        MatrixBz[4 * (l - 1) - 1, 0] = 0;
                    }
                    //pozostale elementy
                    else {
                        //C0 z poprzednim
                        MatrixA[2 + 4 * (i - 1),4 * (i-1)] = 1;
                        MatrixA[2 + 4 * (i - 1), 4 * (i - 1) + 1] = chords[i - 1];
                        MatrixA[2 + 4 * (i - 1), 4 * (i - 1) + 2] = Math.Pow(chords[i - 1], 2);
                        MatrixA[2 + 4 * (i - 1), 4 * (i - 1) + 3] = Math.Pow(chords[i - 1], 3);
                        MatrixBx[2 + 4 * (i - 1), 0] = ControlPoints[i].x;
                        MatrixBy[2 + 4 * (i - 1), 0] = ControlPoints[i].y;
                        MatrixBz[2 + 4 * (i - 1), 0] = ControlPoints[i].z;

                        //ciaglosc C1
                        MatrixA[2 + 4 * (i - 1) + 1, 4 * (i - 1) + 1] = -1;
                        MatrixA[2 + 4 * (i - 1) + 1,4 * (i - 1)+ 2] = -2 * chords[i - 1];
                        MatrixA[2 + 4 * (i - 1) + 1,4 * (i - 1) + 3] = -3 *  Math.Pow(chords[i - 1],2);
                        MatrixA[2 + 4 * (i - 1) + 1,4 * (i) + 1] = 1;
                        MatrixBx[2 + 4 * (i - 1)+1, 0] = 0;
                        MatrixBy[2 + 4 * (i - 1)+1, 0] = 0;
                        MatrixBz[2 + 4 * (i - 1)+1, 0] = 0;
                        
                        //ciaglosc C2
                        MatrixA[2 + 4 * (i - 1) + 2, 4 * (i - 1) + 2] = -2;
                        MatrixA[2 + 4 * (i - 1) + 2, 4 * (i - 1) + 3] = -6 * chords[i - 1];
                        MatrixA[2 + 4 * (i - 1) + 2, 4 * (i) + 2] = 2;
                        MatrixBx[2 + 4 * (i - 1) + 2, 0] = 0;
                        MatrixBy[2 + 4 * (i - 1) + 2, 0] = 0;
                        MatrixBz[2 + 4 * (i - 1) + 2, 0] = 0;

                        //C0 z nastepnym
                        MatrixA[2 + 4 * (i - 1) + 3, 4 * i] = 1;
                        MatrixBx[2 + 4 * (i - 1) + 3, 0] = ControlPoints[i].x;
                        MatrixBy[2 + 4 * (i - 1) + 3, 0] = ControlPoints[i].y;
                        MatrixBz[2 + 4 * (i - 1) + 3, 0] = ControlPoints[i].z;
                    }
                }

                LinearEquations leq = new LinearEquations();
                var x = leq.Solve(MatrixA, MatrixBx);
                var y = leq.Solve(MatrixA, MatrixBy);
                var z = leq.Solve(MatrixA, MatrixBz);
                _polyX.Clear();
                _polyY.Clear();
                _polyZ.Clear();
                for (int i = 0; i < l - 1; i++)
                {
                    _polyX.Add(new Polynomial(3));
                    _polyX[i][0] = x[4 * i , 0];
                    _polyX[i][1] = x[4 * i + 1, 0];
                    _polyX[i][2] = x[4 * i + 2, 0];
                    _polyX[i][3] = x[4 * i + 3, 0];

                    _polyY.Add(new Polynomial(3));
                    _polyY[i][0] = y[4 * i, 0];
                    _polyY[i][1] = y[4 * i + 1, 0];
                    _polyY[i][2] = y[4 * i + 2, 0];
                    _polyY[i][3] = y[4 * i + 3, 0];

                    _polyZ.Add(new Polynomial(3));
                    _polyZ[i][0] = z[4 * i, 0];
                    _polyZ[i][1] = z[4 * i + 1, 0];
                    _polyZ[i][2] = z[4 * i + 2, 0];
                    _polyZ[i][3] = z[4 * i + 3, 0];

                }
            }
        }
    }
}
