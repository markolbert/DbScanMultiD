using Microsoft.Win32.SafeHandles;

namespace J4JSoftware.Data;

public static class Extensions
{
    public const double EarthRadiusMeters = 6378137;

    public static bool TryAddEuclideanPoint( this PointSet pointSet, double[] coordinates )
    {
        if( coordinates.Length != pointSet.Dimensionality )
        {
            pointSet.Logger?.InconsistentDimensionality( $"({string.Join( ", ", coordinates )})",
                                                         coordinates.Length,
                                                         pointSet.Dimensionality );
            return false;
        }

        pointSet._points.Add( new Point( pointSet, coordinates ) );

        return true;
    }

    public static double EuclideanDistance( IPoint a, IPoint b )
    {
        if( !ReferenceEquals( a.PointSet, b.PointSet ) )
            throw new ArgumentException(
                $"{nameof( EuclideanDistance )}: points must belong to the same {nameof( PointSet )}." );

        var sumOfSquares = 0.0;

        for( var idx = 0; idx < a.PointSet.Dimensionality; idx++ )
        {
            var delta = a.Coordinates[ idx ] - b.Coordinates[ idx ];
            sumOfSquares += delta * delta;
        }

        return Math.Sqrt( sumOfSquares );
    }

    public static bool AreEuclideanNeighbors( IPoint a, IPoint b )
    {
        if( !ReferenceEquals( a.PointSet, b.PointSet ) )
            throw new ArgumentException(
                $"{nameof( AreEuclideanNeighbors )}: points must belong to the same {nameof( PointSet )}." );

        var sumOfSquares = 0.0;

        for( var idx = 0; idx < a.PointSet.Dimensionality; idx++ )
        {
            var delta = a.Coordinates[ idx ] - b.Coordinates[ idx ];

            if( Math.Abs( delta ) >= a.PointSet.Epsilon )
                return false;

            sumOfSquares += delta * delta;
        }

        return sumOfSquares < a.PointSet.EpsilonSquared;
    }

    public static bool TryAddGeoPoint(
        this GeoPointSet pointSet,
        double latitude,
        double longitude,
        double elevation
    )
    {
        if( pointSet.Dimensionality != 3 )
        {
            pointSet.Logger?.InconsistentDimensionality( $"({latitude}, {longitude}, {elevation})",
                                                         3,
                                                         pointSet.Dimensionality );
            return false;
        }

        // coordinates are always stored as radians, so convert if necessary
        double[] coordinates =
        [
            pointSet.DegreeUnits == DegreeUnits.Degrees ? ToRadians( latitude ) : latitude,
            pointSet.DegreeUnits == DegreeUnits.Degrees ? ToRadians( longitude ) : longitude,
            elevation
        ];

        pointSet._points.Add( new Point( pointSet, coordinates ) );

        return true;

        double ToRadians( double degrees ) => degrees * Math.PI / 180.0;
    }

    public static double GeoLinearizedDistance( IPoint a, IPoint b )
    {
        if( !ReferenceEquals( a.PointSet, b.PointSet ) )
            throw new ArgumentException(
                $"{nameof( GeoLinearizedDistance )}: points must belong to the same {nameof( PointSet )}." );

        if( a.PointSet is not GeoPointSet || b.PointSet is not GeoPointSet )
            throw new ArgumentException(
                $"{nameof( GeoLinearizedDistance )}: one or both points do not belong to a {nameof( GeoPointSet )}" );

        var deltaLat = b.Coordinates[ 0 ] - a.Coordinates[ 0 ];
        var cosineLat = Math.Cos( a.Coordinates[ 0 ] );
        var deltaLong = b.Coordinates[ 1 ] - a.Coordinates[ 1 ];
        var deltaElevation = b.Coordinates[ 2 ] - a.Coordinates[ 2 ];

        var deltaX = EarthRadiusMeters * cosineLat * deltaLong;
        var deltaY = EarthRadiusMeters * deltaLat;

        return Math.Sqrt( deltaX * deltaX + deltaY * deltaY + deltaElevation * deltaElevation );
    }

    public static bool AreGeoLinearizedNeighbors( IPoint a, IPoint b )
    {
        if( !ReferenceEquals( a.PointSet, b.PointSet ) )
            throw new ArgumentException(
                $"{nameof( GeoLinearizedDistance )}: points must belong to the same {nameof( PointSet )}." );

        if( a.PointSet is not GeoPointSet || b.PointSet is not GeoPointSet )
            throw new ArgumentException(
                $"{nameof( GeoLinearizedDistance )}: one or both points do not belong to a {nameof( GeoPointSet )}" );

        return GeoLinearizedDistance( a, b ) < a.PointSet.Epsilon;
    }

    public static double GeoHaversineDistance(IPoint a, IPoint b)
    {
        if (!ReferenceEquals(a.PointSet, b.PointSet))
            throw new ArgumentException(
                $"{nameof(GeoLinearizedDistance)}: points must belong to the same {nameof(PointSet)}.");

        if (a.PointSet is not GeoPointSet || b.PointSet is not GeoPointSet)
            throw new ArgumentException(
                $"{nameof(GeoLinearizedDistance)}: one or both points do not belong to a {nameof(GeoPointSet)}");

        var sineLat1Lat2 = Math.Sin( a.Coordinates[ 0 ] ) * Math.Sin( b.Coordinates[ 0 ] );
        var cosLat1Lat2 = Math.Cos(a.Coordinates[0]) * Math.Cos(b.Coordinates[0]);
        var cosDeltaLon = Math.Cos( b.Coordinates[ 1 ] - a.Coordinates[ 1 ] );

        return Math.Acos( sineLat1Lat2 + cosLat1Lat2 * cosDeltaLon ) * EarthRadiusMeters;
    }

    public static bool AreHaversineNeighbors(IPoint a, IPoint b)
    {
        if (!ReferenceEquals(a.PointSet, b.PointSet))
            throw new ArgumentException(
                $"{nameof(GeoLinearizedDistance)}: points must belong to the same {nameof(PointSet)}.");

        if (a.PointSet is not GeoPointSet || b.PointSet is not GeoPointSet)
            throw new ArgumentException(
                $"{nameof(GeoLinearizedDistance)}: one or both points do not belong to a {nameof(GeoPointSet)}");

        return GeoHaversineDistance( a, b ) < a.PointSet.Epsilon;
    }

}
