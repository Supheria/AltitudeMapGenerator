using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.TypeGeneral;
using LocalUtilities.TypeGeneral.Convert;
using LocalUtilities.TypeToolKit.Math;

namespace AtlasGenerator.Common;

internal class CoordinateD(double x, double y)
{
    internal double X { get; } = x;

    internal double Y { get; } = y;

    internal CoordinateD() : this(0, 0)
    {

    }

    public static bool operator ==(CoordinateD? coordinate, object? obj)
    {
        if (coordinate is null)
        {
            if (obj is null)
                return true;
            else
                return false;
        }
        if (obj is not CoordinateD other)
            return false;
        return coordinate.X == other.X && coordinate.Y == other.Y;
    }

    public static bool operator !=(CoordinateD? coordinate, object? obj)
    {
        return !(coordinate == obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override bool Equals(object? obj)
    {
        return this == obj;
    }

    public static implicit operator Coordinate(CoordinateD coordinateD)
    {
        return new((int)coordinateD.X, (int)coordinateD.Y);
    }

    public static implicit operator PointF(CoordinateD coordinate)
    {
        return new((float)coordinate.X, (float)coordinate.Y);
    }
}
