using LocalUtilities.TypeGeneral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasGenerator;

public class AtlasException(string message) : Exception(message)
{
    public static AtlasException NotProperRiverLayoutType()
    {
        return new($"not a proper type of river layout");
    }

    public static AtlasException NotProperRiverEndnodeDirection(Direction direction)
    {
        return new($"{direction} is not proper to river end node");
    }

    public static AtlasException AltitudeRatioOutRange()
    {
        return new($"altitude ratio is out of range, it should between 0 and 1");
    }
}
