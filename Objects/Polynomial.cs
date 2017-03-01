using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class Polynomial
    {
        protected int _Degree;
        protected double[] _coefficient;

        public Polynomial(int n1)
        {
            _Degree = n1;
            _coefficient = new double[n1+1];
        }

        public double this[int index] { get { return _coefficient[index]; } set { _coefficient[index] = value; } }

    }
}
