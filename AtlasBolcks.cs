using AtlasGenerator.DLA;
using LocalUtilities.TypeGeneral;
using LocalUtilities.TypeToolKit.Convert;

namespace AtlasGenerator;

public class AtlasBolcks : SerializableTagValues<Coordinate, List<DlaPixel>>
{
    public override string LocalName { get; set; } = "BlockSite";

    protected override Func<Coordinate, string> WriteTag => c => c.ToIntString();

    protected override Func<List<DlaPixel>, List<string>> WriteValue => p => p.Select(p => StringArray.ToArrayString(p.X, p.Y, p.Height)).ToList();

    protected override Func<string, Coordinate> ReadTag => Coordinate.Parse;

    protected override Func<List<string>, List<DlaPixel>> ReadValue => list =>
    {
        var pixels = new List<DlaPixel>();
        foreach (var item in list)
        {
            var arr = item.ToArray();
            if (arr.Length is not 3)
                continue;
            pixels.Add(new(int.Parse(arr[0]), int.Parse(arr[1])) { Height = int.Parse(arr[2]) });
        }
        return pixels;
    };
}
