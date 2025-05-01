using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace J4JSoftware.Data;

public interface IPoint
{
    PointSet PointSet { get; }
    double[] Coordinates { get; }
    PointType PointType { get; }
    ICluster? Cluster { get; set; }
}