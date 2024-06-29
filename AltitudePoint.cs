using LocalUtilities.TypeGeneral;
using LocalUtilities.TypeGeneral.Convert;

namespace AltitudeMapGenerator;

public class AltitudePoint(int x, int y, double altitude)
{
    public int X { get; private set; } = x;

    public int Y { get; private set; } = y;

    public double Altitude { get; private set; } = altitude;

    public override string ToString()
    {
        return ArrayString.ToArrayString(X, Y, Altitude);
    }

    public static AltitudePoint Parse(string str)
    {
        var array = str.ToArray();
        if (array.Length is not 3 ||
            !int.TryParse(array[0], out var x) ||
            !int.TryParse(array[1], out var y) ||
            !int.TryParse(array[2], out var altitude))
            return new(0, 0, 0);
        return new(x, y, altitude);
    }

    public static implicit operator Coordinate(AltitudePoint point)
    {
        return new(point.X, point.Y);
    }
}
