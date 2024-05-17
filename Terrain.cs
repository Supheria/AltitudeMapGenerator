using LocalUtilities.TypeToolKit.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasGenerator;

public class Terrain
{
    public enum Type
    {
        Plain,
        Hill,
        Stream,
        Woodland
    }

    public static Type GetTerrain(double altitudeRatio)
    {
        if (altitudeRatio < 0 || altitudeRatio > 1)
            throw AtlasException.AltitudeRatioOutRange();
        if (altitudeRatio.ApproxEqualTo(0))
            return Type.Plain;
        if (altitudeRatio.ApproxLessThan(1d / 12d))
            return Type.Woodland;
        if (altitudeRatio.ApproxLessThan(3d / 12d))
            return Type.Stream;
        return Type.Hill;
    }
}
