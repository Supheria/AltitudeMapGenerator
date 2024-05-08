using AtlasGenerator.DLA;
using AtlasGenerator.VoronoiDiagram;
using LocalUtilities.MathBundle;

namespace AtlasGenerator;

public class Atlas
{
    public Dictionary<Coordinate, DlaPixel[]> PixelsMap { get; } = [];

    public List<Edge> Rivers { get; }

    public Rectangle Bounds { get; }

    public int Width => Bounds.Width;

    public int Height => Bounds.Height;

    public Atlas(AtlasData data)
    {
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
        Rivers = river.Rivers.ToList();
#if DEBUG
        DlaMap.TestForm.Total = data.PixelNumber;
        DlaMap.TestForm.Show();
#endif
        Parallel.ForEach(plane.Generate(sites),
            (cell) => PixelsMap[cell.Site] = new DlaMap(cell).Generate((int)(cell.GetArea() / data.Area * data.PixelNumber), data.PixelDensity));
    }
}
