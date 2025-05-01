using Microsoft.Extensions.Logging;

namespace J4JSoftware.Data;

public class GeoPointSet( DegreeUnits degreeUnits, ILoggerFactory? loggerFactory ) : PointSet( loggerFactory )
{
    public DegreeUnits DegreeUnits { get; } = degreeUnits;
}
