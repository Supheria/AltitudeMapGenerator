using LocalUtilities.TypeGeneral;
using LocalUtilities.TypeGeneral.Convert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return new(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]));
        throw TypeConvertException.CannotConvertStringTo<AtlasPoint>();
    }
}
