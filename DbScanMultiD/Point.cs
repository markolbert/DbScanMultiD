using System.Reflection.Metadata.Ecma335;

namespace J4JSoftware.Data;

public class Point : IPoint
{
    internal Point(
        PointSet pointSet,
        params double[] coordinates
    )
    {
        PointSet = pointSet;
        Coordinates = coordinates;
    }
    
    public PointSet PointSet { get; }
    
    public double[] Coordinates { get; }
    public HashSet<Point> Neighbors { get; } = [];

    public PointType PointType
    {
        get
        {
            var count = Neighbors.Count;
            
            if( count >= PointSet.MinNeighborsForCore )
                return PointType.Core;

            return count > 0 ? PointType.Border : PointType.Noise;
        }
    }
    
    public ICluster? Cluster { get; set; }
}
