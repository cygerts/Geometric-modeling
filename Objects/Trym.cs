using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNumerics;
using DotNumerics.LinearAlgebra;


namespace ModelowanieGeometyczne2
{
    public class Trymowanie
    {
        public BSplineSurface b1;
        public BSplineSurface b2;
        public ParameterMap m1;
        public ParameterMap m2;
        private Form1 form;
        public double size = 200;
        public double step = 0.05;
        public Boolean BigFlag = true;
        MyPoint x0 = new MyPoint();
        public List<MyPoint> points = new List<MyPoint>();
        public List<MyPoint> parameters = new List<MyPoint>();
        public Trymowanie(BSplineSurface bx, BSplineSurface by, double u, double v, double s, double t)
        {
            this.b1 = bx;
            this.b2 = by;
            x0 = FindCrossPoint(u, v, s, t);
            b1.CalculatePoints(step);
            b2.CalculatePoints(step);
        }

        public bool FindAllPoints(double delta2)
        {
            double[,] Jacobian = new double[4,4];
            double[,] F = new double[4, 1];
            MyPoint p1 = new MyPoint();
            MyPoint p2 = new MyPoint();
            double u,v,s,t;
            double delta1 = delta2;
            double delta = delta2;
        
            MyPoint T = new MyPoint();
            MyPoint P1u = new MyPoint();
            MyPoint P1v = new MyPoint();
            MyPoint P2u = new MyPoint();
            MyPoint P2v = new MyPoint();
            MyPoint P0u = new MyPoint();
            MyPoint P0v = new MyPoint();
            MyPoint P0s = new MyPoint();
            MyPoint P0t = new MyPoint();

            MyPoint k0 = new MyPoint();
            MyPoint k1 = new MyPoint();
            int counter2 = 0;
            bool smallFlag = true;
            int counter = 0;
            bool isInit = false;
            bool isInit2 = true;

            while (BigFlag)
            {

                if (isInit && isInit2)
                {
                    k0.SetCoordinatesParam(parameters.First());
                }
                else
                {
                    k0.SetCoordinatesParam(parameters.Last());
                }
                u = k0.x; v = k0.y; s = k0.z; t = k0.w;
           
                    //P0u.SetCoordinates(b1.ComputeDU(u, v));
                    //P0v.SetCoordinates(b1.ComputeDV(u, v));
                    //P0s.SetCoordinates(b2.ComputeDU(s, t));
                    //P0t.SetCoordinates(b2.ComputeDV(s, t));
                
                smallFlag = true;

                    P1u.SetCoordinates(b1.ComputeDU(u, v));
                    P1v.SetCoordinates(b1.ComputeDV(u, v));
                    P2u.SetCoordinates(b2.ComputeDU(s, t));
                    P2v.SetCoordinates(b2.ComputeDV(s, t));
                    T.SetCoordinates(MyMath.ScalarProduct(MyMath.ScalarProduct(P1u, P1v), MyMath.ScalarProduct(P2u, P2v)));
                    T.Normalized();
                while(smallFlag)
                {
                    //if (T.x != 0 || T.y != 0 || T.z != 0)
                    DotNumerics.LinearAlgebra.Matrix m = new DotNumerics.LinearAlgebra.Matrix(4, 4);
                   // MyPoint Tu, Tv, Ts, Tt;
                  //  Tu = MyMath
                    MyPoint start = new MyPoint();
                    start.SetCoordinates(b1.ComputePointForParameter(u, v));

               //     if(isI
                    k1.SetCoordinates(b2.ComputePointForParameter(s, t));
                    p1.SetCoordinates(b1.ComputePointForParameter(u, v));
                    p2.SetCoordinates(b2.ComputePointForParameter(s, t));

                    P1u.SetCoordinates(b1.ComputeDU(u, v));
                    P1v.SetCoordinates(b1.ComputeDV(u, v));
                    P2u.SetCoordinates(b2.ComputeDU(s, t));
                    P2v.SetCoordinates(b2.ComputeDV(s, t));
             
                    
                    F[0, 0] = p1.x - p2.x;
                    F[1, 0] = p1.y - p2.y;
                    F[2, 0] = p1.z - p2.z;
                    if (isInit && isInit2)
                    {
                        F[3, 0] = (k1.x - points.First().x) * T.x + (k1.y - points.First().y) * T.y + (k1.z - points.First().z) * T.z - delta;
                        isInit2 = false;
                    }
                    else
                        F[3, 0] = (k1.x - points.Last().x) * T.x + (k1.y - points.Last().y) * T.y + (k1.z - points.Last().z) * T.z - delta;

                    Jacobian[0, 0] = P1u.x;  //b1.ComputeDU(u, v).x;
                    Jacobian[0, 1] = P1v.x;   //b1.ComputeDV(u, v).x;
                    Jacobian[0, 2] = -P2u.x;// -b2.ComputeDU(s, t).x;
                    Jacobian[0, 3] = -P2v.x;// -b2.ComputeDV(s, t).x;
                    Jacobian[1, 0] = P1u.y;//    b1.ComputeDU(u, v).y;
                    Jacobian[1, 1] = P1v.y;//    b1.ComputeDV(u, v).y;
                    Jacobian[1, 2] = -P2u.y;// -b2.ComputeDU(s, t).y;
                    Jacobian[1, 3] = -P2v.y;// -b2.ComputeDV(s, t).y;
                    Jacobian[2, 0] = P1u.z;// b1.ComputeDU(u, v).z;
                    Jacobian[2, 1] = P1v.z;// b1.ComputeDV(u, v).z;
                    Jacobian[2, 2] = -P2u.z;// -b2.ComputeDU(s, t).z;
                    Jacobian[2, 3] = -P2v.z;// -b2.ComputeDV(s, t).z;
                    Jacobian[3, 0] = (P1u.x ) * T.x + (P1u.y) * T.y + (P1u.z) * T.z; // dodac odwrotnie 
                    Jacobian[3, 1] = (P1v.x) * T.x + (P1v.y) * T.y + (P1v.z ) * T.z;
                   
                    Jacobian[3, 2] = 0;//wartosc punktu * pochodna po T
                    Jacobian[3, 3] = 0;// (0 - P0t.x) * T.x + (0 - P0t.y) * T.y + (0 - P0t.z) * T.z;

                    try
                    {
                        for (int i = 0; i < 4; i++)
                            for (int j = 0; j < 4; j++)
                                m[i, j] = Jacobian[i, j];
                        m = m.Inverse();
                        double[,] l = new double[4, 4];
                        for (int i = 0; i < 4; i++)
                            for (int j = 0; j < 4; j++)
                                l[i, j] = m[i, j];
                        var res = MyMath.Multiply2(l, F);

                        k0.x -= res[0, 0];
                        k0.y -= res[1, 0];
                        k0.z -= res[2, 0];
                        k0.w -= res[3, 0];
                        k0.CheckParam();
                        u = k0.x; v = k0.y; s = k0.z; t = k0.w;
                    }
                    catch
                    {
                        smallFlag = false;
                        delta /= 2;
                        delta1 /= 2;
                        counter = 0;
                        if (Math.Abs(delta) < 0.00000000001)
                        {
                            if (isInit)
                            {
                                BigFlag = false;
                                CreateMap();
                                return true;
                            }
                            else
                            {
                                isInit = true;
     //                           delta = delta1;
       //                         delta1 = -delta;
                            }
                        }
                    }
                    
                    double dist = MyMath.distance(b1.ComputePointForParameter(u, v), start);
                    double dist1 = MyMath.distance(b2.ComputePointForParameter(s, t), start);
                    dist = Math.Min(dist, dist1);
                    dist1 = MyMath.distance(b1.ComputePointForParameter(u, v), b2.ComputePointForParameter(s, t));
                    dist = Math.Min(dist, dist1);

                    counter++;

                    //if (dist < 0.00000001)
                    //{
                    //    counter2++;
                    //    if (counter2 < 100 && smallFlag)
                    //        delta = 0.05;
                    //}

                    if (dist < Math.Max(0.001, 0.000000000001) && smallFlag &&( u >= 0 && u <= 1 && v >= 0 && v <=1 && s >= 0 && s <=1 && t >=0 && t <=1))
                    {
                        points.Add(new MyPoint());
                        points.Last().SetCoordinates(b1.ComputePointForParameter(u, v));
                        parameters.Add(new MyPoint(u, v, s, t));
                        smallFlag = false;
                        counter = 0;
                        //if (MyMath.distance(points.First(), points.Last()) < 0.5 * delta )
                        //{
                        //    BigFlag = false;
                        //    CreateMap();
                        //    return true;
                        //}
                    }
               //         if (points.Count > 200)
               //             delta *= 8;

                    if (points.Count > 1000 && isInit2)
                    {
                        isInit = true;
                       // delta = delta1;
                        //delta1 = -delta;
                        smallFlag = false;
                    }
                    if (points.Count > 1000)
                    {
                        BigFlag = false;
                        CreateMap();
                        return true;
                    }
                        if (Math.Abs(delta) < 0.0000000000000001)
                        {
                            if (isInit)
                            {
                                BigFlag = false;
                                CreateMap();
                                return true;
                            }
                            else
                            {
                                isInit = true;
                                //delta = delta1;
                                //delta1 = -delta;
                                smallFlag = false;
                            }
                        }
                    if (counter > 700 || (counter > 30 && dist > 20))
                    {
                        smallFlag = false;
                        delta /= 2;
                        BigFlag = false;
                        delta1 /= 2;
                        counter = 0;
                   //     if (delta < 0.001)
                   //         BigFlag = false;
                    }

                }
            }
            return false;
        }

        private void CreateMap()
        {
            m1 = new ParameterMap("uv", parameters, b1);
            m2 = new ParameterMap("st", parameters, b2);
        }

        public MyPoint FindCrossPoint(double u1, double v1, double s1, double t1)
        {
            points.Add(b1.ComputePointForParameter(u1, v1));
            points.Add(b2.ComputePointForParameter(s1, t1));

         /*   MyPoint grad = new MyPoint();
            grad.x = MyMath.DotProduct(MyMath.Difference(b1.ComputePointForParameter(u1, v1), b2.ComputePointForParameter(s1, t1)).Multiply(2), b1.ComputeDU(u1, v1));
            grad.y = MyMath.DotProduct(MyMath.Difference(b1.ComputePointForParameter(u1, v1), b2.ComputePointForParameter(s1, t1)).Multiply(2), b1.ComputeDV(u1, v1));
            grad.z = MyMath.DotProduct(MyMath.Difference(b1.ComputePointForParameter(u1, v1), b2.ComputePointForParameter(s1, t1)).Multiply(2), b2.ComputeDU(s1, t1));
            grad.w = MyMath.DotProduct(MyMath.Difference(b1.ComputePointForParameter(u1, v1), b2.ComputePointForParameter(s1, t1)).Multiply(2), b2.ComputeDV(s1, t1));

            double dist = MyMath.distance(b1.ComputePointForParameter(u1, v1), b2.ComputePointForParameter(s1,t1));
            double alfa = 0.1;
            u1 -= alfa*grad.x ;
            v1 -= alfa*grad.y; 
            s1 -= alfa*grad.z;
            t1 -= alfa * grad.w;
            dist = MyMath.distance(b1.ComputePointForParameter(u1, v1), b2.ComputePointForParameter(s1, t1));
        */
            double step = 0.05f;
            int counter = 0;
            double u,v,s,t;
            u = u1; v = v1; s = s1; t = t1;
            MyPoint pRight1 = new MyPoint();
            MyPoint pLeft1 = new MyPoint();
            double distance = MyMath.distance(points[0], points[1]);
            double newDistance = 0;
            int leftMax = -1;
            MyPoint[] pLeft = new MyPoint[4];
            
            for(int i = 0 ; i < 4 ; i++)
                pLeft[i] = new MyPoint();
            MyPoint[] pRight = new MyPoint[4];

            for (int i = 0; i < 4; i++)
                pRight[i] = new MyPoint();
            
            while (counter < 10)
            {
                leftMax = -1;
                Console.WriteLine(distance);

                pRight1.SetCoordinates(b2.ComputePointForParameter(s,t));
                 pLeft[0].SetCoordinates(b1.ComputePointForParameter(u + step, v));
                 pLeft[1].SetCoordinates(b1.ComputePointForParameter(u - step, v));
                 pLeft[2].SetCoordinates(b1.ComputePointForParameter(u , v + step));
                 pLeft[3].SetCoordinates(b1.ComputePointForParameter(u , v - step));
                 for (int i = 0; i < 4; i++)
                 {
                     if (MyMath.distance(pRight1, pLeft[i]) < distance)
                     {
                         distance = MyMath.distance(pRight1, pLeft[i]);
                         leftMax = i;
                     }
                 }

                 pLeft1.SetCoordinates(b1.ComputePointForParameter(u, v));
                 pRight[0].SetCoordinates(b2.ComputePointForParameter(s + step, t));
                 pRight[1].SetCoordinates(b2.ComputePointForParameter(s - step, t));
                 pRight[2].SetCoordinates(b2.ComputePointForParameter(s, t + step));
                 pRight[3].SetCoordinates(b2.ComputePointForParameter(s, t - step));
                 for (int i = 0; i < 4; i++)
                 {
                     if (MyMath.distance(pLeft1, pRight[i]) < distance)
                     {
                         distance = MyMath.distance(pLeft1, pRight[i]);
                         leftMax = 4+i;
                     }
                 }

                 switch (leftMax)
                 {
                     case 0: u += step;
                         break;
                     case 1: u -= step;
                         break;
                     case 2: v += step;
                         break;
                     case 3: v -= step;
                         break;
                     case 4: s += step;
                         break;
                     case 5: s -= step;
                         break;
                     case 6: t += step;
                         break;
                     case 7: t -= step;
                         break;
                     case -1:
                         step /= 2;
                         counter++;
                         break;
                 }
                 
                points[0].SetCoordinates(b1.ComputePointForParameter(u, v));
                points[1].SetCoordinates(b2.ComputePointForParameter(s, t));
            
                parameters.Clear(); 
                parameters.Add(new MyPoint(u, v, s, t));
            }
            points.RemoveAt(1);
            return points.First();
        }
    }
}
