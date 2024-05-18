using LocalUtilities.TypeGeneral;

namespace AtlasGenerator.Common;

internal class EdgeD(CoordinateD starter, CoordinateD ender)
{
    internal CoordinateD Starter { get; } = starter;

    internal CoordinateD Ender { get; } = ender;

    public static implicit operator Edge(EdgeD edgeD)
    {
        return new(edgeD.Starter, edgeD.Ender);
    }
}
