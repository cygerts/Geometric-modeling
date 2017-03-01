using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    public class GregoryPatch
    {
        //public MyPoint[,] ControlPoints;
        public bool IsC1 = false;
        public BezierSurfaceGregoryExt[] BezierSurfaces = new BezierSurfaceGregoryExt[3];
        public GregorySubpatch[] gregorySubpatches = new GregorySubpatch[3]; 

        public GregoryPatch(BezierSurfaceGregoryExt b1, BezierSurfaceGregoryExt b2, BezierSurfaceGregoryExt b3)
        {
            b1.ControlPoints[0, 3] = b2.ControlPoints[0, 0];
            b2.ControlPoints[0, 3] = b3.ControlPoints[0, 0];
            b3.ControlPoints[0, 3] = b1.ControlPoints[0, 0];

            BezierSurfaces[0] = b1;
            BezierSurfaces[1] = b2;
            BezierSurfaces[2] = b3;
            
            gregorySubpatches[0] = new GregorySubpatch(b1, b2);
            gregorySubpatches[1] = new GregorySubpatch(b2, b3);
            gregorySubpatches[2] = new GregorySubpatch(b3, b1);
  
        }
        public void Update()
        {
            foreach (GregorySubpatch g in gregorySubpatches)
                g.CalculateControlPoints();
            MyPoint temp = MyMath.Medium(gregorySubpatches[0].ControlPoints[13], gregorySubpatches[1].ControlPoints[13], gregorySubpatches[2].ControlPoints[13]);
            MyPoint p0 = MyMath.AddDifference2(temp, gregorySubpatches[0].ControlPoints[13], 0.66);
            MyPoint p1 = MyMath.AddDifference2(temp, gregorySubpatches[1].ControlPoints[13], 0.66);
            MyPoint p2 = MyMath.AddDifference2(temp, gregorySubpatches[2].ControlPoints[13], 0.66);

            foreach (GregorySubpatch g in gregorySubpatches)
            {
                g.ControlPoints[13].SetCoordinates(temp);
            }
            gregorySubpatches[0].ControlPoints[14].SetCoordinates(p0);
            gregorySubpatches[1].ControlPoints[14].SetCoordinates(p1);
            gregorySubpatches[2].ControlPoints[14].SetCoordinates(p2);
            gregorySubpatches[0].ControlPoints[15] = gregorySubpatches[1].ControlPoints[14];
            gregorySubpatches[1].ControlPoints[15] = gregorySubpatches[2].ControlPoints[14];
            gregorySubpatches[2].ControlPoints[15] = gregorySubpatches[0].ControlPoints[14];

            for (int i = 0; i < 3; i++)
            {
                MyPoint a0 = MyMath.Difference(gregorySubpatches[i].ControlPoints[13], gregorySubpatches[i].ControlPoints[14]);
                MyPoint b0 = MyMath.Difference(gregorySubpatches[i ].ControlPoints[15], gregorySubpatches[i].ControlPoints[13]);
                MyPoint g0 = new MyPoint();
                g0.SetInterpolatedCoordinates(a0, b0, 0.5);
                MyPoint g3 = MyMath.Difference(gregorySubpatches[i].ControlPoints[5], gregorySubpatches[i].ControlPoints[6]);
                MyPoint g1 = new MyPoint();
                g1.SetInterpolatedCoordinates(g0, g3, 0.5);
                MyPoint temp2 = new MyPoint();
                temp2 = MyMath.BernstetInterpolation(g3, g0, g1, 0.66);
                gregorySubpatches[i].ControlPoints[16] = MyMath.AddVector(gregorySubpatches[i].ControlPoints[12], temp2);
                temp2 = MyMath.BernstetInterpolation(g0, g3, g1, 0.33);
                gregorySubpatches[i].ControlPoints[17] = MyMath.AddVector(gregorySubpatches[i].ControlPoints[15], temp2);

                a0 = MyMath.Difference(gregorySubpatches[i].ControlPoints[13], gregorySubpatches[(i+2)%3].ControlPoints[14]);
                b0 = MyMath.Difference(gregorySubpatches[i].ControlPoints[14], gregorySubpatches[i].ControlPoints[13]);
                g0.SetInterpolatedCoordinates(a0, b0, 0.5);
                g3 = MyMath.Difference(gregorySubpatches[i].ControlPoints[1], gregorySubpatches[i].ControlPoints[0]);
                g1.SetInterpolatedCoordinates(g0, g3, 0.5);
                
                temp2.SetInterpolatedCoordinates(g3, g0, 0.66);
                gregorySubpatches[i].ControlPoints[18] = MyMath.AddVector(gregorySubpatches[i].ControlPoints[7], temp2);
                temp2 = MyMath.BernstetInterpolation(g0, g3, g1, 0.33);
                gregorySubpatches[i].ControlPoints[19] = MyMath.AddVector(gregorySubpatches[i].ControlPoints[14], temp2);

                gregorySubpatches[i].UpdateControlTable();
            }
        }
       }
}
