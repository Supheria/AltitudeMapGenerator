using AtlasGenerator.DLA;
using AtlasGenerator.VoronoiDiagram;
using LocalUtilities.MathBundle;
using LocalUtilities.SimpleScript.Serialization;
using LocalUtilities.TypeBundle;

namespace AtlasGenerator;

public class Atlas : ISsSerializable
{
    public Dictionary<Coordinate, List<DlaPixel>> PixelsMap { get; private set; } = [];

    public List<Edge> River { get; private set; } = [];

    public Rectangle Bounds { get; private set; }

    public int Width => Bounds.Width;

    public int Height => Bounds.Height;

    public string LocalName { get; set; }

    /// <summary>
    /// Do not generate new map, just use for read saved file
    /// </summary>
    /// <param name="localName"></param>
    public Atlas(string localName)
    {
        LocalName = localName;
    }

    public Atlas(AtlasData data)
    {
        LocalName = data.Name;
        VoronoiPlane plane;
        List<Coordinate> sites;
        AtlasRiver river;
        do
        {
            Bounds = new(new(0, 0), data.Size);
            plane = new VoronoiPlane(data.Size);
            sites = plane.GenerateSites(data.SegmentNumber, data.PointsGeneration);
            river = new AtlasRiver(data.Size, data.RiverSegmentNumber, data.RiverLayoutType, data.PointsGeneration, sites);
        } while (river.Successful is false);
        River = river.River.ToList();
#if DEBUG
        DlaMap.TestForm.Total = data.PixelNumber;
        DlaMap.TestForm.Show();
#endif
        Parallel.ForEach(plane.Generate(sites),
            (cell) => PixelsMap[cell.Site] = new DlaMap(cell).Generate((int)(cell.GetArea() / data.Area * data.PixelNumber), data.PixelDensity));
    }

    public void Serialize(SsSerializer serializer)
    {
        serializer.WriteTag(nameof(Width), Width.ToString());
        serializer.WriteTag(nameof(Height), Height.ToString());
        serializer.WriteValueArrays(nameof(River), River, e => e.ToStringArray(true));
        serializer.Serialize(new AtlasBolcks() { Map = PixelsMap });
    }

    public void Deserialize(SsDeserializer deserializer)
    {
        var width = deserializer.ReadTag(nameof(Width), s => s.ToInt(Width));
        var height = deserializer.ReadTag(nameof(Height), s => s.ToInt(Height));
        Bounds = new(0, 0, width, height);
        River = deserializer.ReadValueArrays(nameof(River), Edge.ParseStringArray);
        PixelsMap = deserializer.Deserialize(new AtlasBolcks()).Map;
    }
}
