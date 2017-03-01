using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelowanieGeometyczne2
{
    
    public abstract class Object
    {

        public List<MyPoint> Points = new List<MyPoint>();

        public abstract List<MyPoint> UpdatePoints(int div);


    }
}
