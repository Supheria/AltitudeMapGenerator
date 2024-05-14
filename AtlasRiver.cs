using AtlasGenerator.DijkstraShortestPath;
using AtlasGenerator.Layout;
using AtlasGenerator.VoronoiDiagram;
using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.GraphUtilities;
using LocalUtilities.MathBundle;

namespace AtlasGenerator;

internal class AtlasRiver
{
    RiverLayout RiverLayout { get; }

    Random Random { get; } = new();

    private enum NodeTowards
    {
        Start = 0,
        Finish,
    }

    Dictionary<(int, NodeTowards Towards), HashSet<Coordinate>> BorderNodeMap { get; } = [];

    HashSet<Coordinate> InnerVertexes { get; set; } = [];

    HashSet<Edge> Edges { get; set; } = [];

    internal HashSet<Edge> River { get; private set; } = [];

    internal bool Successful { get; private set; } = true;

    internal AtlasRiver(Size size, Size segmentNumber, RiverLayout.Type riverLayoutType, IPointsGeneration pointsGeneration, List<Coordinate> existedSites)
    {
        RiverLayout = riverLayoutType.Parse()(size);
        List<VoronoiCell> cells;
        var plane = new VoronoiPlane(size);
        var sites = plane.GenerateSites(segmentNumber, pointsGeneration, existedSites);
        cells = plane.Generate(sites);
        foreach (var cell in cells)
        {
            foreach (var vertex in cell.Vertexes)
            {
                if (vertex.DirectionOnBorder is Direction.None)
                    InnerVertexes.Add(vertex);
                else
                    BorderNodeFilter(vertex);
                var nextVertex = cell.VertexCounterClockwiseNext(vertex);
                Edges.Add(new(vertex, nextVertex));
            }
        }
        for (int i = 0; i < RiverLayout.Layout.Count; i++)
            GenerateRiver(i);
    }

    private void BorderNodeFilter(VoronoiVertex vertex)
    {
        for (int i = 0; i < RiverLayout.Layout.Count; i++)
        {
            if (RiverLayout.Layout[i].Start.VoronoiVertexFilter(vertex))
            {
                var key = (i, NodeTowards.Start);
                if (BorderNodeMap.TryGetValue(key, out var nodes))
                    nodes.Add(vertex);
                else
                    BorderNodeMap[key] = [vertex];
            }
            else if (RiverLayout.Layout[i].Finish.VoronoiVertexFilter(vertex))
            {
                var key = (i, NodeTowards.Finish);
                if (BorderNodeMap.TryGetValue(key, out var nodes))
                    nodes.Add(vertex);
                else
                    BorderNodeMap[key] = [vertex];
            }
        }
    }

    private void GenerateRiver(int riverIndex)
    {
        var startNodes = BorderNodeMap[(riverIndex, NodeTowards.Start)].ToList();
        var finishNodes = BorderNodeMap[(riverIndex, NodeTowards.Finish)].ToList();
        List<Edge>? river = null;
        var existed = River.ToHashSet();
        var startVisited = new HashSet<Coordinate>();
        var finishVisited = new HashSet<Coordinate>();
        do
        {
            if (startVisited.Count == startNodes.Count && finishNodes.Count == finishNodes.Count)
                break;
            var start = startNodes[Random.Next(0, startNodes.Count)];
            var finish = finishNodes[Random.Next(0, finishNodes.Count)];
            startVisited.Add(start);
            finishVisited.Add(finish);
            var nodes = InnerVertexes.ToList();
            nodes.AddRange([start, finish]);
            river = new Dijkstra(Edges.ToList(), nodes, start, finish).Path;
        } while (river is null || river.FirstOrDefault(existed.Contains) is not null);
        if (river is not null && river.FirstOrDefault(existed.Contains) is null)
            river.ForEach(e => River.Add(e));
        else
            Successful = false;
    }
}
