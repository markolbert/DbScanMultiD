namespace J4JSoftware.Data;

public interface ICluster
{
    int Id { get; }
    string? Label { get; }
    HashSet<IPoint> Points { get; }
}
