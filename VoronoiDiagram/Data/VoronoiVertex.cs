using LocalUtilities.MathBundle;

namespace AtlasGenerator.VoronoiDiagram.Data;

/// <summary>
/// The vertices/nodes of the Voronoi cells, i.e. the points equidistant to three or more Voronoi sites.
/// These are the end points of a <see cref="VoronoiEdge"/>.
/// These are the <see cref="VoronoiCell.Vertexes"/>.
/// Also used for some other derived locations.
/// </summary>
public class VoronoiVertex(double x, double y, Direction borderLocation = Direction.None)
{
    public double X => x;

    public double Y => y;

    /// <summary>
    /// Specifies if this point is on the border of the bounds and where.
    /// </summary>
    /// <remarks>
    /// Using this would be preferrable to comparing against the X/Y values due to possible precision issues.
    /// </remarks>
    public Direction DirectionOnBorder { get; internal set; } = borderLocation;

    public double AngleTo(VoronoiVertex other)
    {
        return Math.Atan2(other.Y - Y, other.X - X);
    }

    public static implicit operator Coordinate(VoronoiVertex vertex)
    {
        return new(vertex.X, vertex.Y);
    }

    public static implicit operator PointF(VoronoiVertex vertex)
    {
        return new((float)vertex.X, (float)vertex.Y);
    }

    public static bool operator ==(VoronoiVertex? v1, object? v2)
    {
        if (v1 is null)
        {
            if (v2 is null)
                return true;
            else
                return false;
        }
        if (v2 is not VoronoiVertex other)
            return false;
        return v1.X.ApproxEqualTo(other.X) && v1.Y.ApproxEqualTo(other.Y);
    }

    public static bool operator !=(VoronoiVertex? v1, object? v2)
    {
        return !(v1 == v2);
    }

    public static bool operator ==(VoronoiVertex v, (double X, double Y) p)
    {
        return v.X.ApproxEqualTo(p.X) && v.Y.ApproxEqualTo(p.Y);
    }

    public static bool operator !=(VoronoiVertex v, (double X, double Y) p)
    {
        return !(v == p);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override bool Equals(object? obj)
    {
        return this == obj;
    }

#if DEBUG
    public override string ToString()
    {
        return
            "("
            + (X == double.MinValue ? "-∞" : X == double.MaxValue ? "+∞" : X.ToString("F3"))
            + ","
            + (Y == double.MinValue ? "-∞" : Y == double.MaxValue ? "+∞" : Y.ToString("F3"))
            + ")"
            + BorderLocationToString(DirectionOnBorder);
    }

    public string ToString(string format)
    {
        return
            "("
            + (X == double.MinValue ? "-∞" : X == double.MaxValue ? "+∞" : X.ToString(format))
            + ","
            + (Y == double.MinValue ? "-∞" : Y == double.MaxValue ? "+∞" : Y.ToString(format))
            + ")"
            + BorderLocationToString(DirectionOnBorder);
    }

    private static string BorderLocationToString(Direction location)
    {
        return location switch
        {
            Direction.LeftTop => "LT",
            Direction.Left => "L",
            Direction.LeftBottom => "LB",
            Direction.Bottom => "B",
            Direction.BottomRight => "BR",
            Direction.Right => "R",
            Direction.TopRight => "TR",
            Direction.Top => "T",
            _ => "",
        };
    }
#endif
}
