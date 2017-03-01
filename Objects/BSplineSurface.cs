using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class BSplineSurface
    {
        public int Degree { get; protected set; }
        public bool isCylinder;
        public bool[,] Map;
        public bool isTrymmed = false;
        public int mSize { get; protected set; }
        public List<double> _knots = new List<double>();
        public List<double> _knots2 = new List<double>();
        public List<double> _knotsDeriv = new List<double>();
        public MyPoint selectedPoint;
        public int nSize { get; protected set; }
        protected int _selectedPoint = 0;
        protected int _selectedBoth = 0;
        protected bool both = false;
        public MyPoint[,] DeBoorControlPoints { get; protected set; }
        public MyPoint[,] DerivativeControlPoints { get; protected set; }
        public List<List<MyPoint>> points = new List<List<MyPoint>>();
        //public List<List<MyPoint>> points2 = new List<List<MyPoint>>();
        
        public int samplesNumber = 20;

        public BSplineSurface(int degree, int m, int n, int l)
        {
            Degree = degree;
            mSize = m; //rows
            nSize = n; //columns
            DeBoorControlPoints = new MyPoint[this.mSize, this.nSize];
            
            CalculatePoints();
            isCylinder = false;
        }

        public void UpdateMap(bool[,] m1)
        {
            if (Map == null)
                Map = m1;
            else
            for (int i = 0; i < Map.GetLength(0); i++)
                for (int j = 0; j < Map.GetLength(1); j++)
                    Map[i, j] = m1[i, j] && Map[i,j];
        }

        public int mTableSize() //# of rows
        {
            return Degree + 1 + (mSize - 1) * 3;
        }

        public int nTableSize() //# of columns
        {
            return Degree + 1 + (nSize - 1) * 3;
        }

        public virtual void AddDeBoorPoints()
        {
            for (int i = 0; i < this.mSize; i++)
                for (int j = 0; j < this.nSize; j++)
                    DeBoorControlPoints[i, j] = new MyPoint(i * 250, j * 250, 0);
            CalculatePoints();
        }

        public virtual void CreateMain()
        {

        }

        public virtual void CreateRoof()
        {

        }

        public void MoveDeBoorPoint(double x, double y, double z)
        {
            selectedPoint.SetCoordinates(x, y, z);
            CalculatePoints();
        }

        public virtual MyPoint ComputeDU(double u1, double v1)
        {
            int l = this.isCylinder ? 3 : 0;

            double step1, step2;
            int m = DeBoorControlPoints.GetLength(0) + Degree + 1 + l;
            m = DeBoorControlPoints.GetLength(1) + Degree + 1;
            //step1 = (_knots2[DeBoorControlPoints.GetLength(1)] - _knots2[Degree]) / (double)(samplesNumber - 1);
            //step2 = (_knots[DeBoorControlPoints.GetLength(0) + l] - _knots[Degree]) / (double)(samplesNumber - 1);
            double u, v;
            List<MyPoint> qPoints = new List<MyPoint>();
            v = _knots2[Degree-1]; //N
            double a, b, c,x,z,y;
            x = 0; y = 0; z = 0; a = 0; b = 0; c = 0;
            v = u1;
            double delta = 0.001;
            if (v < _knots2[Degree-1])
                return new MyPoint();
            if (v - delta <= _knots2[DeBoorControlPoints.GetLength(1)])
            {
                for (int i = 0; i < mSize + l; i++)
                {
                    x = y = z = 0;
                    for (int j = 0; j < nSize; j++)
                    {
                        x += (DeBoorControlPoints[i % mSize, j].x * Nik2Derivative(v, j, Degree-1));
                        y += (DeBoorControlPoints[i % mSize, j].y * Nik2Derivative(v, j, Degree-1));
                        z += (DeBoorControlPoints[i % mSize, j].z * Nik2Derivative(v, j, Degree-1));
                    }
                    qPoints.Add(new MyPoint(x, y, z));
                }

                List<MyPoint> subList = new List<MyPoint>();
                u = _knots[Degree];
                u = v1;
     
                if (u - delta <= _knots[DeBoorControlPoints.GetLength(0) + l])
                {
                    a = b = c = 0;
                    for (int p = 0; p < mSize + l; p++)
                    {
                        a += (qPoints[p].x * Nik(u, p, Degree));
                        b += (qPoints[p].y * Nik(u, p, Degree));
                        c += (qPoints[p].z * Nik(u, p, Degree));
                    }
                    return new MyPoint(a, b, c);
                    //subList.Add(new MyPoint(a, b, c));
                    //u += step2;
                }
                return new MyPoint();
            }
            return new MyPoint();
        }

        public virtual MyPoint ComputeDV(double u1, double v1)
        {
            
            int l = this.isCylinder ? 3 : 0;

            double step1, step2;
            int m = DeBoorControlPoints.GetLength(0) + Degree + 1 + l;
            m = DeBoorControlPoints.GetLength(1) + Degree + 1;
            double u, v;
            List<MyPoint> qPoints = new List<MyPoint>();
            v = _knots2[Degree]; //N
            double a, b, c,x,y,z;
            x = 0; y = 0; z = 0; a = 0; b = 0; c = 0;
            v = u1;
            double delta = 0.001;
            if (v < _knots2[Degree])
                return new MyPoint();
            if (v - delta <= _knots2[DeBoorControlPoints.GetLength(1)])
            {
                for (int i = 0; i < mSize + l; i++)
                {
                    x = y = z = 0;
                    for (int j = 0; j < nSize; j++)
                    {
                        x += (DeBoorControlPoints[i % mSize, j].x * Nik2(v, j, Degree));
                        y += (DeBoorControlPoints[i % mSize, j].y * Nik2(v, j, Degree));
                        z += (DeBoorControlPoints[i % mSize, j].z * Nik2(v, j, Degree));
                    }
                    qPoints.Add(new MyPoint(x, y, z));
                }

                List<MyPoint> subList = new List<MyPoint>();
                u = _knots[Degree];
                u = v1;
     
                if (u - delta <= _knots[DeBoorControlPoints.GetLength(0) + l])
                {
                    a = b = c = 0;
                    for (int p = 0; p < mSize + l; p++)
                    {
                        a += (qPoints[p].x * NikDerivative(u, p, Degree));
                        b += (qPoints[p].y * NikDerivative(u, p, Degree));
                        c += (qPoints[p].z * NikDerivative(u, p, Degree));
                    }
                    return new MyPoint(a, b, c);
                    //subList.Add(new MyPoint(a, b, c));
                    //u += step2;
                }
                return new MyPoint();
            }
            return new MyPoint();
        }

        public virtual List<MyPoint> getP(double u1, double v1)
        {
             double step = 0.05;
            MyPoint p1 = new MyPoint();
            MyPoint p2 = new MyPoint();
            MyPoint p3 = new MyPoint();
            p1.SetCoordinates(this.ComputePointForParameter(u1, v1));
            p2.SetCoordinates(this.ComputePointForParameter(u1, v1 + step));
            p3.SetCoordinates(MyMath.Substract(p2, p1));
            p3.Normalized();
            
            //points.Clear();
            //_knots.Clear();
            double delta = 0.001f;
            //_knots2.Clear();
            int l = this.isCylinder ? 3 : 0;

            double step1, step2;
            int m = DeBoorControlPoints.GetLength(0) + Degree + 1 + l;
            //for (int i = 0; i < m; i++)
            //    _knots.Add((double)i / (double)(m - 1));
            //m = DeBoorControlPoints.GetLength(1) + Degree + 1;
            //for (int i = 0; i < m; i++)
            //    _knots2.Add((double)i / (double)(m - 1));
            step1 = (_knots2[DeBoorControlPoints.GetLength(1)] - _knots2[Degree]) / (double)(samplesNumber - 1);
            step2 = (_knots[DeBoorControlPoints.GetLength(0) + l] - _knots[Degree]) / (double)(samplesNumber - 1);
            double u, v;
            List<MyPoint> qPoints = new List<MyPoint>();
            v = _knots2[Degree]; //N
            double x = 0, y = 0, z = 0, a = 0, b = 0, c = 0;
            while (v - delta <= _knots2[DeBoorControlPoints.GetLength(1)])
            {
                for (int i = 0; i < mSize + l; i++)
                {
                    x = y = z = 0;
                    for (int j = 0; j < nSize; j++)
                    {
                        x += (DeBoorControlPoints[i % mSize, j].x * Nik2(v, j, Degree));
                        y += (DeBoorControlPoints[i % mSize, j].y * Nik2(v, j, Degree));
                        z += (DeBoorControlPoints[i % mSize, j].z * Nik2(v, j, Degree));
                    }
                    qPoints.Add(new MyPoint(x, y, z));
                    v += step1;
                }
            }

            List<MyPoint> qPointsDer = new List<MyPoint>();
            qPointsDer.Add(new MyPoint());
            for (int i = 0; i < qPoints.Count-1; i++)
            {
                qPointsDer.Add(new MyPoint());
                qPointsDer[i].SetCoordinates(MyMath.Difference(qPoints[i + 1], qPoints[i]));
            }
            //qPointsDer[0].SetCoordinates(MyMath.AddDifference(qPointsDer[0], qPointsDer[1]));
            //policzyc krzywa stalego parametru
            //Qi = (delta P)
            _knotsDeriv.Clear();
            m = qPointsDer.Count + Degree -1 + 1; //stopień = 2
            for (int i = 0; i < m; i++)
                _knotsDeriv.Add((double)i / (double)(m - 1));
            double t;
            t = u1;
    //        while (t < _knotsDeriv[qPointsDer.Count])
     //       {
                x = y = z = 0;
                for (int i = 0; i < qPointsDer.Count; i++)
                {
                    x += (qPointsDer[i].x * Nik3(t, i, Degree-1));
                    y += (qPointsDer[i].y * Nik3(t, i, Degree - 1));
                    z += (qPointsDer[i].z * Nik3(t, i, Degree - 1));
                }
    //        }
            //Ci = suma i = 0 .. n-1 Ni,p-1(u),Qi
return qPointsDer;
//            return p3;
        }

        public virtual MyPoint ComputePointForParameter(double u1, double v1){

            if (DeBoorControlPoints[0, 0] == null)
                return new MyPoint();

            _knots.Clear();
            double delta = 0.001f;
            _knots2.Clear();
            int l = this.isCylinder ? 3 : 0;

            double step1, step2;
            int m = DeBoorControlPoints.GetLength(0) + Degree + 1 + l;
            int from = 3;
            int to = DeBoorControlPoints.GetLength(0) + l;
            int numOfPoints = to - from + 1;
            double cos1 = (double)1 / (double)(numOfPoints-1);
            double start = -3*cos1;
            for (int i = 0; i < m; i++)
                _knots.Add(start + i * cos1);
            m = DeBoorControlPoints.GetLength(1) + Degree + 1;
            from = 3;
            to = DeBoorControlPoints.GetLength(1);
            numOfPoints = to - from + 1;
            cos1 = (double)1 / (double)(numOfPoints - 1);
            start = -3 * cos1;
            for (int i = 0; i < m; i++)
                _knots2.Add(start + i * cos1);
            
            double u, v;
            List<MyPoint> qPoints = new List<MyPoint>();
          
            double x = 0, y = 0, z = 0, a = 0, b = 0, c = 0;
            v = u1;
            if  (v - delta < 1)
            {
                for (int i = 0; i < mSize + l; i++)
                {
                    x = y = z = 0;
                    for (int j = 0; j < nSize; j++)
                    {
                        x += (DeBoorControlPoints[i % mSize, j].x * Nik2(v, j, Degree));
                        y += (DeBoorControlPoints[i % mSize, j].y * Nik2(v, j, Degree));
                        z += (DeBoorControlPoints[i % mSize, j].z * Nik2(v, j, Degree));
                    }
                    qPoints.Add(new MyPoint(x, y, z));
                    }
                List<MyPoint> subList = new List<MyPoint>();
                u = v1;
                if(u - delta <= 1)
                {
                    a = b = c = 0;
                    for (int p = 0; p < mSize + l; p++)
                    {
                        a += (qPoints[p].x * Nik(u, p, Degree));
                        b += (qPoints[p].y * Nik(u, p, Degree));
                        c += (qPoints[p].z * Nik(u, p, Degree));
                    }
                    return new MyPoint(a, b, c);
                }
                return new MyPoint();
            }
            return new MyPoint();
        }


        public double ComputeStepU()
        {
            return (_knots2[DeBoorControlPoints.GetLength(1)] - _knots2[Degree]) / (double)(samplesNumber - 1);
        }

        public double ComputeStepV()
        {
            int l = this.isCylinder ? 3 : 0; 

            return (_knots[DeBoorControlPoints.GetLength(0) + l] - _knots[Degree]) / (double)(samplesNumber - 1);
        }

        public virtual void CalculatePoints(double step)
        {
            if (DeBoorControlPoints[0, 0] == null)
                return;

            //points2.Clear();
            _knots.Clear();
            double delta = 0.001f;
            _knots2.Clear();
            int l = this.isCylinder ? 3 : 0;

            int m = DeBoorControlPoints.GetLength(0) + Degree + 1 + l;
            int from = 3;
            int to = DeBoorControlPoints.GetLength(0) + l;
            int numOfPoints = to - from + 1;
            double cos1 = (double)1 / (double)(numOfPoints - 1);
            double start = -3 * cos1;
            for (int i = 0; i < m; i++)
                _knots.Add(start + i * cos1);
            m = DeBoorControlPoints.GetLength(1) + Degree + 1;
            from = 3;
            to = DeBoorControlPoints.GetLength(1);
            numOfPoints = to - from + 1;
            cos1 = (double)1 / (double)(numOfPoints - 1);
            start = -3 * cos1;
            for (int i = 0; i < m; i++)
                _knots2.Add(start + i * cos1);
            
            double step1, step2;
            step1 = (_knots2[DeBoorControlPoints.GetLength(1)] - _knots2[Degree]) / (double)(samplesNumber - 1);
            step2 = (_knots[DeBoorControlPoints.GetLength(0) + l] - _knots[Degree]) / (double)(samplesNumber - 1);
            //   int counter1 = 0;
            //   int counter2 = 0;
           // step1 = step;
           // step2 = step;
            double u, v;
            List<MyPoint> qPoints = new List<MyPoint>();
            v = _knots2[Degree]; //N
            double x = 0, y = 0, z = 0, a = 0, b = 0, c = 0;
            while (v - delta <= _knots2[DeBoorControlPoints.GetLength(1)])
            {
                for (int i = 0; i < mSize + l; i++)
                {
                    x = y = z = 0;
                    for (int j = 0; j < nSize; j++)
                    {
                        x += (DeBoorControlPoints[i % mSize, j].x * Nik2(v, j, Degree));
                        y += (DeBoorControlPoints[i % mSize, j].y * Nik2(v, j, Degree));
                        z += (DeBoorControlPoints[i % mSize, j].z * Nik2(v, j, Degree));
                    }
                    qPoints.Add(new MyPoint(x, y, z));
                }

                List<MyPoint> subList = new List<MyPoint>();
                //     counter2 = 0;
                u = _knots[Degree];
                while (u - delta <= _knots[DeBoorControlPoints.GetLength(0) + l])
                {
                    a = b = c = 0;
                    for (int p = 0; p < mSize + l; p++)
                    {
                        a += (qPoints[p].x * Nik(u, p, Degree));
                        b += (qPoints[p].y * Nik(u, p, Degree));
                        c += (qPoints[p].z * Nik(u, p, Degree));
                    }
                    subList.Add(new MyPoint(a, b, c));
                    subList.Last().u = u;
                    subList.Last().v = v;
                    u += step2;
                }
                //points2.Add(subList);
                
                v += step1;// _knots2[Degree] + step1 * (double)counter1;
                qPoints.Clear();
            }
        }
        
        public virtual void CalculatePoints()
        {
            
            if (DeBoorControlPoints[0, 0] == null || isTrymmed)
                return;

            points.Clear();
            _knots.Clear();
            double delta = 0.001f;
            _knots2.Clear();
            int l = this.isCylinder ? 3 : 0; 

            double step1, step2; 
            int m = DeBoorControlPoints.GetLength(0) + Degree + 1 + l;
            for (int i = 0; i < m; i++)
                _knots.Add((double)i / (double)(m - 1));
            m = DeBoorControlPoints.GetLength(1) + Degree + 1;
            for (int i = 0; i < m; i++)
                _knots2.Add((double)i / (double)(m - 1));
         //   int counter1 = 0;
         //   int counter2 = 0;
            step1 = (_knots2[DeBoorControlPoints.GetLength(1)] - _knots2[Degree] ) / (double)(samplesNumber-1) ;
            step2 = (_knots[DeBoorControlPoints.GetLength(0) + l] - _knots[Degree] ) / (double)(samplesNumber-1);
            double u, v;
            List<MyPoint> qPoints = new List<MyPoint>();
            v = _knots2[Degree]; //N
            double x = 0, y = 0, z = 0, a = 0, b = 0, c = 0;
            while (v - delta  <= _knots2[DeBoorControlPoints.GetLength(1)] )
            {
                for (int i = 0; i < mSize + l; i++)
                {
                    x = y = z = 0;
                    for (int j = 0; j < nSize; j++)
                    {
                        x += (DeBoorControlPoints[i%mSize, j].x * Nik2(v, j, Degree));
                        y += (DeBoorControlPoints[i%mSize, j].y * Nik2(v, j, Degree));
                        z += (DeBoorControlPoints[i%mSize, j].z * Nik2(v, j, Degree));
                    }
                    qPoints.Add(new MyPoint(x, y, z));
                }

                List<MyPoint> subList = new List<MyPoint>();
           //     counter2 = 0;
                u = _knots[Degree];
               while (u  - delta <= _knots[DeBoorControlPoints.GetLength(0) + l])
                {
                    a = b = c = 0;
                    for (int p = 0; p < mSize+ l; p++)
                    {
                        a += (qPoints[p].x * Nik(u, p, Degree));
                        b += (qPoints[p].y * Nik(u, p, Degree));
                        c += (qPoints[p].z * Nik(u, p, Degree));
                    }
                    subList.Add(new MyPoint(a, b, c));
                    u += step2;
                }
                points.Add(subList);
                //counter1++;
                v += step1;// _knots2[Degree] + step1 * (double)counter1;
                qPoints.Clear();
            }
        }

        public void TranslateX(int shift)
        {
            for(int i = 0 ; i <DeBoorControlPoints.GetLength(0) ; i++)
                for (int j = 0; j < DeBoorControlPoints.GetLength(1); j++)
                {
                    DeBoorControlPoints[i,j].x += (double)shift;
                }
        }

        public void TranslateY(int shift)
        {
            for (int i = 0; i < DeBoorControlPoints.GetLength(0); i++)
                for (int j = 0; j < DeBoorControlPoints.GetLength(1); j++)
                {
                    DeBoorControlPoints[i,j].y += (double)shift;
                }
        }

        public void TranslateZ(int shift)
        {
            for (int i = 0; i < DeBoorControlPoints.GetLength(0); i++)
                for (int j = 0; j < DeBoorControlPoints.GetLength(1); j++)
                {
                    DeBoorControlPoints[i,j].z += (double)shift;
                }
        }

        protected double Nik(double t, int i, int k)
        {
                if (k == 0)
                {
                    if (t >= _knots[i] && t < _knots[i + 1])
                        return 1.0;
                    else
                        return 0.0;
                }
                else
                {
                    return ((t - _knots[i]) / (_knots[i + k] - _knots[i])) * Nik(t, i, k - 1) +
                                ((_knots[i + k + 1] - t) / (_knots[i + k + 1] - _knots[i + 1])) * Nik(t, i + 1, k - 1);
                }
        }

        protected double Nik3(double t, int i, int k)
        {
            if (k == 0)
            {
                if (t >= _knotsDeriv[i] && t < _knotsDeriv[i + 1])
                    return 1.0;
                else
                    return 0.0;
            }
            else
            {
                return ((t - _knotsDeriv[i]) / (_knotsDeriv[i + k] - _knotsDeriv[i])) * Nik3(t, i, k - 1) +
                            ((_knotsDeriv[i + k + 1] - t) / (_knotsDeriv[i + k + 1] - _knotsDeriv[i + 1])) * Nik3(t, i + 1, k - 1);
            }
        }


             protected double Nik2Derivative(double t, int i, int k)
        {
                if (k == 0)
                {
                    if (t >= _knots2[i] && t < _knots2[i + 1])
                        return 1.0;
                    else
                        return 0.0;
                }
                else
                {
                    return Nik2(t, i, k - 1) / (_knots2[i + 1] - _knots2[i]) - Nik2(t, i + 1, k - 1) / (_knots2[i + 2] - _knots2[i + 1]); 
            }
    }

             protected double NikDerivative(double t, int i, int k)
             {
                 if (k == 0)
                 {
                     if (t >= _knots[i] && t < _knots[i + 1])
                         return 1.0;
                     else
                         return 0.0;
                 }
                 else
                 {
                     return Nik(t, i, k - 1) / (_knots[i + 1] - _knots[i]) - Nik(t, i + 1, k - 1) / (_knots[i + 2] - _knots[i + 1]);
                 }
             }


        protected double Nik2(double t, int i, int k)
        {
                if (k == 0)
                {
                    if (t >= _knots2[i] && t < _knots2[i + 1])
                        return 1.0;
                    else
                        return 0.0;
                }
                else
                {
                    return ((t - _knots2[i]) / (_knots2[i + k] - _knots2[i])) * Nik2(t, i, k - 1) +
                                ((_knots2[i + k + 1] - t) / (_knots2[i + k + 1] - _knots2[i + 1])) * Nik2(t, i + 1, k - 1);
                }
            }
    }
}
