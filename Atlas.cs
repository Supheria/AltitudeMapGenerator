using AtlasGenerator.DLA;
using AtlasGenerator.VoronoiDiagram;
using LocalUtilities.SimpleScript.Serialization;
using LocalUtilities.TypeGeneral;
using LocalUtilities.TypeToolKit.Mathematic;

namespace AtlasGenerator;

public class Atlas : ISsSerializable
{
    public List<Coordinate> OriginPoints { get; private set; } = [];

    public List<Coordinate> RiverPoints { get; private set; } = [];

    public List<AtlasPoint> AltitudePoints { get; private set; } = [];

    public double AltitudeMax { get; private set; } = 0;

    public Rectangle Bounds
    {
        get => _bounds;
        private set
        {
            _bounds = value;
            Area = _bounds.Width * _bounds.Height;
        }
    }

    Rectangle _bounds = new();

    public int Width => Bounds.Width;

    public int Height => Bounds.Height;

    public int Area { get; private set; }

    public double[] RandomTable { get; private set; } = [];

    public string LocalName { get; set; }

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
            sites = plane.GenerateSites(data.SegmentNumber);
            river = new AtlasRiver(data.RiverWidth, data.Size, data.RiverSegmentNumber, data.RiverLayoutType, sites);
        } while (river.Successful is false);
        RiverPoints = river.River.ToList();

        DlaMap.TestForm.Total = data.PixelNumber;
        DlaMap.TestForm.Show();

        var altitudes = new List<double>();
        Parallel.ForEach(plane.Generate(sites), (cell) =>
        {
            var dlaMap = new DlaMap(cell);
            var pixels = dlaMap.Generate((int)(cell.GetArea() / Area * data.PixelNumber), data.PixelDensity);
            altitudes.Add(dlaMap.AltitudeMax);
            OriginPoints.Add(cell.Site);
            AltitudePoints.AddRange(pixels.Select(p => new AtlasPoint(p.X, p.Y, p.Altitude)));
        });
        AltitudeMax = altitudes.Max();
        var random = new Random();
        RandomTable = new double[1000];
        for (int i = 0; i < RandomTable.Length; i++)
            RandomTable[i] = Math.Round(random.NextDouble(), 3);
    }

    public void Serialize(SsSerializer serializer)
    {
        serializer.WriteTag(nameof(Width), Width.ToString());
        serializer.WriteTag(nameof(Height), Height.ToString());
        serializer.WriteTag(nameof(AltitudeMax), AltitudeMax.ToString());
        serializer.WriteValues(nameof(OriginPoints), OriginPoints, c => c.ToString());
        serializer.WriteValues(nameof(RiverPoints), RiverPoints, c => c.ToString());
        serializer.WriteValues(nameof(AltitudePoints), AltitudePoints, p => p.ToString());
        serializer.WriteValues(nameof(RandomTable), RandomTable.ToList(), d=>d.ToString());
    }

    public void Deserialize(SsDeserializer deserializer)
    {
        var width = deserializer.ReadTag(nameof(Width), int.Parse);
        var height = deserializer.ReadTag(nameof(Height), int.Parse);
        Bounds = new(0, 0, width, height);
        AltitudeMax = deserializer.ReadTag(nameof(AltitudeMax), double.Parse);
        OriginPoints = deserializer.ReadValues(nameof(OriginPoints), Coordinate.Parse);
        RiverPoints = deserializer.ReadValues(nameof(RiverPoints), Coordinate.Parse);
        AltitudePoints = deserializer.ReadValues(nameof(AltitudePoints), AtlasPoint.Parse);
        RandomTable = deserializer.ReadValues(nameof(RandomTable), double.Parse).ToArray();
    }
}
