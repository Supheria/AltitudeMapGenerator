using LocalUtilities.TypeGeneral;
using LocalUtilities.TypeGeneral.Convert;

namespace AtlasGenerator;

public class AtlasPoint(int x, int y, double altitude)
{
    public int X { get; private set; } = x;

    public int Y { get; private set; } = y;

    public double Altitude { get; private set; } = altitude;

    public override string ToString()
    {
        return ArrayString.ToArrayString(X, Y, Altitude);
    }

    public static AtlasPoint Parse(string str)
    {
        var list = str.ToArray();
        if (list.Length is 3)
            return new(int.Parse(list[0]), int.Parse(list[1]), double.Parse(list[2]));
        throw TypeConvertException.CannotConvertStringTo<AtlasPoint>();
    }

    public static implicit operator Coordinate(AtlasPoint point)
    {
        return new(point.X, point.Y);
    }
}
