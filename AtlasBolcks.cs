using AtlasGenerator.DLA;
using LocalUtilities.MathBundle;
using LocalUtilities.TypeBundle;

namespace AtlasGenerator;

public class AtlasBolcks : SerializableTagValues<Coordinate, List<DlaPixel>>
{
    public override string LocalName { get; set; } = "BlockSite";

    protected override Func<Coordinate, string> WriteTag => c => c.ToIntString();

    protected override Func<List<DlaPixel>, List<string>> WriteValue => p => p.Select(p => StringTypeConverter.ToArrayString(p.X, p.Y, p.Height)).ToList();

    protected override Func<string, Coordinate> ReadTag => s => s.ToCoordinate(new());

    protected override Func<List<string>, List<DlaPixel>> ReadValue => list =>
    {
        var pixels = new List<DlaPixel>();
        foreach (var item in list)
        {
            var arr = item.ToArray();
            if (arr.Length is not 3)
                continue;
            pixels.Add(new(arr[0].ToInt(0), arr[1].ToInt(0)) { Height = arr[2].ToInt(0) });
        }
        return pixels;
    };
}
