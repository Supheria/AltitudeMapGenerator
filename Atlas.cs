using AtlasGenerator.Common;
using AtlasGenerator.DLA;
using AtlasGenerator.VoronoiDiagram;
using LocalUtilities.SimpleScript.Serialization;
using LocalUtilities.TypeGeneral;

namespace AtlasGenerator;

public class Atlas : ISsSerializable
{
    public Dictionary<Coordinate, List<AtlasPoint>> PixelsMap { get; private set; } = [];

    public List<Edge> River { get; private set; } = [];

    public double AltitudeMax { get; private set; } = 0;

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
        List<CoordinateD> sites;
        AtlasRiver river;
        do
        {
            Bounds = new(new(0, 0), data.Size);
            plane = new VoronoiPlane(data.Size);
            sites = plane.GenerateSites(data.SegmentNumber, data.PointsGeneration);
            river = new AtlasRiver(data.Size, data.RiverSegmentNumber, data.RiverLayoutType, data.PointsGeneration, sites);
        } while (river.Successful is false);
        River = river.River.Select(e => (Edge)e).ToList();

        DlaMap.TestForm.Total = data.PixelNumber;
        DlaMap.TestForm.Show();
        
        var altitudes = new List<double>();
        Parallel.ForEach(plane.Generate(sites),(cell) =>
        {
            var dlaMap = new DlaMap(cell);
            var pixels = dlaMap.Generate((int)(cell.GetArea() / data.Area * data.PixelNumber), data.PixelDensity);
            altitudes.Add(dlaMap.AltitudeMax);
            PixelsMap[cell.Site] = pixels.Select(p => new AtlasPoint(p.X, p.Y, p.Altitude)).ToList();
        });
        AltitudeMax = altitudes.Max();
    }

    public void Serialize(SsSerializer serializer)
    {
        serializer.WriteTag(nameof(Width), Width.ToString());
        serializer.WriteTag(nameof(Height), Height.ToString());
        serializer.WriteTag(nameof(AltitudeMax), AltitudeMax.ToString());
        serializer.WriteValuesArray(nameof(River), River, e => e.ToIntStringArray());
        serializer.WriteObject(new AtlasBolcks() { Map = PixelsMap });
    }

    public void Deserialize(SsDeserializer deserializer)
    {
        var width = deserializer.ReadTag(nameof(Width), int.Parse);
        var height = deserializer.ReadTag(nameof(Height), int.Parse);
        Bounds = new(0, 0, width, height);
        AltitudeMax = deserializer.ReadTag(nameof(AltitudeMax), double.Parse);
        River = deserializer.ReadValuesArray(nameof(River), Edge.Parse);
        PixelsMap = deserializer.ReadObject(new AtlasBolcks()).Map;
    }
}
