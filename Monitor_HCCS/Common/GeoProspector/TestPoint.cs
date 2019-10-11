using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoProspector
{
    class TestPoint
    {
        public int pointId;
        public Single[] points;

        public TestPoint()
        {
            pointId = -1;
            points = null;
        }

        public TestPoint(int pointId, Single[] points)
        {
            this.pointId = pointId;
            this.points = points;
        }
    }
}
