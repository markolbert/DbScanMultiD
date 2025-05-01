using System.Collections;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Data;

public class PointSet : IEnumerable<Point>
{
    internal readonly ILogger? Logger;
    internal readonly HashSet<Point> _points = [];

    public PointSet(
        int dimensionality,
        double epsilon,
        int minNeighborsForCore,
        ILoggerFactory? loggerFactory
    )
    {
        Logger = loggerFactory?.CreateLogger<PointSet>();

        IsValid = true;

        if (dimensionality <= 2)
        {
            Logger?.InvalidDimensionality(dimensionality);
            IsValid = false;
        }

        if (epsilon <= 0.0)
        {
            Logger?.InvalidEpsilon(epsilon);
            IsValid = false;
        }
        else EpsilonSquared = epsilon * epsilon;

        if( minNeighborsForCore >= 1 )
            return;

        Logger?.InvalidMinNeighborsForCore(minNeighborsForCore);
        IsValid = false;
    }
    
    public bool IsValid { get; }

    public int Dimensionality { get; private set; }
    public double Epsilon { get; private set; }
    public double EpsilonSquared { get; private set; }
    public int MinNeighborsForCore { get; private set; }

    public List<ICluster>? Analyze()
    {
        if( !IsValid )
        {
            Logger?.InvalidPointSet();
            return null;
        }

        return null;
    }
    
    public IEnumerator<Point> GetEnumerator() => _points.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
