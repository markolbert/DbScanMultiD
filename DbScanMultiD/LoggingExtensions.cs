using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Data;

internal static partial class LoggingExtensions
{
    [LoggerMessage(LogLevel.Critical, "{caller}: point data must have at least 2 dimensions but has {dimensionality}")]
    public static partial void InvalidDimensionality(
        this ILogger logger,
        int dimensionality,
        [ CallerMemberName ] string caller = ""
    );

    [LoggerMessage(LogLevel.Critical, "{caller}: epsilon must be > 0 but is {epsilon}")]
    public static partial void InvalidEpsilon(
        this ILogger logger,
        double epsilon,
        [CallerMemberName] string caller = ""
    );

    [LoggerMessage(LogLevel.Critical, "{caller}: minimum neighbors for core must be > 1 but is {minNeighborsForCore}")]
    public static partial void InvalidMinNeighborsForCore(
        this ILogger logger,
        int minNeighborsForCore,
        [CallerMemberName] string caller = ""
    );

    [LoggerMessage(LogLevel.Error, "{caller}: point ({coordText}) has {dimensions} but should have {required}")]
    public static partial void InconsistentDimensionality(
        this ILogger logger,
        string coordText,
        int dimensions,
        int required,
        [CallerMemberName] string caller = ""
    );

    [LoggerMessage(LogLevel.Error, "{caller}: PointSet is invalid")]
    public static partial void InvalidPointSet(
        this ILogger logger,
        [CallerMemberName] string caller = ""
    );
}
