using AtlasGenerator.Common;
using LocalUtilities.TypeGeneral;

namespace AtlasGenerator;

internal class AtlasBolcks : SerializableTagValues<Coordinate, List<AtlasPoint>>
{
    public override string LocalName { get; set; } = "BlockSite";

    protected override Func<Coordinate, string> WriteTag => c => c.ToString();

    protected override Func<List<AtlasPoint>, List<string>> WriteValue => p => p.Select(c => c.ToString()).ToList();

    protected override Func<string, Coordinate> ReadTag => Coordinate.Parse;

    protected override Func<List<string>, List<AtlasPoint>> ReadValue => array =>
    {
        var points = new List<AtlasPoint>();
        foreach (var str in array)
            points.Add(AtlasPoint.Parse(str));
        return points;
    };
}
