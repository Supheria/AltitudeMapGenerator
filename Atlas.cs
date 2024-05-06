using AtlasGenerator.DijkstraShortestPath;
using AtlasGenerator.DLA;
using AtlasGenerator.VoronoiDiagram;
using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.GraphUtilities;
using LocalUtilities.MathBundle;

namespace AtlasGenerator;


// [Horizontal]   [Vertical)
//    _______      _______
//   | _____ |    | |   | |
//   |       |    | |   | |
//   |       |    | |   | |
//   | ----- |    | |   | |
//    -------      -------
public enum RiverLayoutType
{
    None,
    Horizontal,
    Vertical,
}

public class Atlas(Size size, Size segmentNumber, RiverLayoutType riverLayoutType, int riverSegmentNumber, int totalPixelNumber, IPointsGeneration pointsGeneration)
{
    VoronoiPlane? VoronoiPlane { get; set; }

    public Dictionary<Coordinate, DlaPixel[]> PixelsMap { get; } = [];

    List<Dictionary<Direction, List<VoronoiCell>>> CellMapForRiver { get; } = [[], []];

    public List<Edge> Rivers { get; } = [];

    RiverLayoutType RiverLayout { get; } = riverLayoutType;

    public int Width { get; } = size.Width;

    public int Height { get; } = size.Height;

    double WidthHalf { get; } = size.Width / 2d;

    double HeightHalf { get; } = size.Height / 2d;

    public Rectangle Bounds => new(0, 0, (int)Width, (int)Height);

    int WidthSegmentNumber { get; } = segmentNumber.Width;

    int HeightSegmentNumber { get; } = segmentNumber.Height;

    int RiverSegmentNumber { get; } = riverSegmentNumber;

    public int TotalPixelNumber { get; } = totalPixelNumber;

    IPointsGeneration PointsGeneration { get; } = pointsGeneration;

    Random Random { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="density">[0,1], bigger means that grid-shape is closer to voronoi-cells' shape</param>
    public void Generate(float density)
    {
        List<VoronoiCell> cells;
        do
        {
            VoronoiPlane = new(Width, Height);
            cells = VoronoiPlane.Generate(WidthSegmentNumber, HeightSegmentNumber, PointsGeneration);
        } while (!GenerateRiver(GenerateRiverVoronoiCells()));
        long area = Width * Height;
        DlaMap.TestForm.Total = TotalPixelNumber;
        DlaMap.TestForm.Show();
        Parallel.ForEach(cells,
            (cell) => PixelsMap[cell.Centroid] = new DlaMap(cell).Generate((int)(cell.GetArea() / area * TotalPixelNumber), density));
    }

    private List<VoronoiCell> GenerateRiverVoronoiCells()
    {
        ArgumentNullException.ThrowIfNull(VoronoiPlane);
        if (RiverLayout is RiverLayoutType.Horizontal)
        {
            CellMapForRiver.ForEach(m => { m.Clear(); m.Add(Direction.Left, []); m.Add(Direction.Right, []); });
            var widthSegmentNumber = RiverSegmentNumber < WidthSegmentNumber ? WidthSegmentNumber : RiverSegmentNumber;
            //cells = VoronoiPlane.Generate(widthSegmentNumber, HeightSegmentNumber, PointsGeneration);
            return VoronoiPlane.Generate(WidthSegmentNumber, widthSegmentNumber, PointsGeneration);
        }
        else if (RiverLayout is RiverLayoutType.Vertical)
        {
            CellMapForRiver.ForEach(m => { m.Clear(); m.Add(Direction.Top, []); m.Add(Direction.Bottom, []); });
            var heightSegmentNumber = RiverSegmentNumber < WidthSegmentNumber ? WidthSegmentNumber : RiverSegmentNumber;
            //cells = VoronoiPlane.Generate(WidthSegmentNumber, heightSegmentNumber, PointsGeneration);
            return VoronoiPlane.Generate(heightSegmentNumber, HeightSegmentNumber, PointsGeneration);
        }
        else
            throw new ArgumentException();
    }

    private bool GenerateRiver(List<VoronoiCell> cells)
    {
        HashSet<Coordinate> nodes = [];
        HashSet<Edge> edges = [];
        foreach (var cell in cells)
        {
            if (RiverLayout is 0)
            {
                var index = cell.Centroid.Y.ApproxLessThanOrEqualTo(HeightHalf) ? 0 : 1;
                if (cell.DirectionOnBorder.HasFlag(Direction.Left))
                    CellMapForRiver[index][Direction.Left].Add(cell);
                else if (cell.DirectionOnBorder.HasFlag(Direction.Right))
                    CellMapForRiver[index][Direction.Right].Add(cell);
            }
            else
            {
                var index = cell.Centroid.X.ApproxLessThanOrEqualTo(WidthHalf) ? 0 : 1;
                if (cell.DirectionOnBorder.HasFlag(Direction.Top))
                    CellMapForRiver[index][Direction.Top].Add(cell);
                else if (cell.DirectionOnBorder.HasFlag(Direction.Bottom))
                    CellMapForRiver[index][Direction.Bottom].Add(cell);
            }
            foreach (var vertex in cell.Vertexes)
            {
                if (vertex.DirectionOnBorder is Direction.None)
                    nodes.Add(vertex);
                var nextVertex = cell.VerticeClockwiseNext(vertex);
                edges.Add(new(vertex, nextVertex));
            }
        }
        var start1st = GetRiverEndpoint(true, true);
        var end1st = GetRiverEndpoint(true, false);
        var start2nd = GetRiverEndpoint(false, true);
        var end2nd = GetRiverEndpoint(false, false);
        nodes.Add(start1st);
        nodes.Add(end1st);
        nodes.Add(start2nd);
        nodes.Add(end2nd);
        Dijkstra.Initialize(edges.ToList(), nodes.ToList());
        var river = Dijkstra.GetPath(start1st, end1st);
        if (river.Count is 0)
            return false;
        Rivers.AddRange(river);
        river = Dijkstra.GetPath(start2nd, end2nd);
        if (river.Count is 0)
            return false;
        Rivers.AddRange(river);
        return true;
    }

    private Coordinate GetRiverEndpoint(bool firstRiver, bool startEndpoint)
    {
        var direction = RiverLayout is 0 ?
            startEndpoint ? Direction.Left : Direction.Right :
            startEndpoint ? Direction.Top : Direction.Bottom;
        var cells = CellMapForRiver[firstRiver ? 0 : 1][direction];
        var vertexes = new List<VoronoiVertex>();
        do
        {
            var cell = cells[Random.Next(0, cells.Count)];
            foreach (var vertex in cell.Vertexes)
            {
                if (vertex.DirectionOnBorder != direction)
                    continue;
                if (RiverLayout is 0)
                {
                    if (firstRiver && vertex.Y.ApproxLessThanOrEqualTo(HeightHalf))
                        vertexes.Add(vertex);
                    else if ((!firstRiver) && vertex.Y.ApproxGreaterThanOrEqualTo(HeightHalf))
                        vertexes.Add(vertex);
                }
                else
                {
                    if (firstRiver && vertex.X.ApproxLessThanOrEqualTo(WidthHalf))
                        vertexes.Add(vertex);
                    else if ((!firstRiver) && vertex.X.ApproxGreaterThanOrEqualTo(WidthHalf))
                        vertexes.Add(vertex);
                }
            }
        } while (vertexes.Count is 0);
        return vertexes[Random.Next(0, vertexes.Count)];
    }

#if DEBUG
    [Obsolete("just for test")]
    public List<VoronoiCell> GenerateVoronoi()
    {
        VoronoiPlane = new(Width, Height);
        return VoronoiPlane.Generate(WidthSegmentNumber, HeightSegmentNumber, PointsGeneration);
    }

    [Obsolete("just for test")]
    public List<VoronoiCell> GenerateRiverVoronoi()
    {
        var cells = GenerateRiverVoronoiCells();
        GenerateRiver(cells);
        return cells;
    }
#endif
}
