using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class BernsteinPolynomial : Polynomial
    {
        public BernsteinPolynomial(int degree) : base(degree) { }

        public double Calculate(double t)
        {
            double OneMinusT = 1 - t;
            double[] beta = (double[])_coefficient.Clone();
            for (int max = _Degree - 1; max >= 0; --max)
                for (int i = 0; i <= max; ++i)
                    beta[i] = beta[i] * OneMinusT + beta[i + 1] * t;
            return beta[0];
        }
    }
}
