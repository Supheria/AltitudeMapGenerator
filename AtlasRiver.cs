using AtlasGenerator.Common;
using AtlasGenerator.DijkstraShortestPath;
using AtlasGenerator.Layout;
using AtlasGenerator.VoronoiDiagram;
using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.TypeGeneral;

namespace AtlasGenerator;

internal class AtlasRiver
{
    RiverLayout RiverLayout { get; }

    Random Random { get; } = new();

    /// <summary>
    /// position of border node, left and top are start, right and bottom are finish
    /// </summary>
    private enum NodeTowards
    {
        Start = 0,
        Finish,
    }

    /// <summary>
    /// nodes on the border
    /// </summary>
    Dictionary<(int, NodeTowards Towards), HashSet<CoordinateD>> BorderNodeMap { get; } = [];

    HashSet<CoordinateD> InnerNodes { get; set; } = [];

    HashSet<EdgeD> Edges { get; set; } = [];

    internal HashSet<EdgeD> River { get; private set; } = [];

    /// <summary>
    /// when rivers generated not match to layout, will be set to false in <see cref="GenerateRiver"/>
    /// </summary>
    internal bool Successful { get; private set; } = true;

    internal AtlasRiver(Size size, Size segmentNumber, RiverLayout.Type riverLayoutType, IPointsGeneration pointsGeneration, List<CoordinateD> existedSites)
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
                    InnerNodes.Add(vertex);
                else
                    BorderNodeFilter(vertex);
                var nextVertex = cell.VertexCounterClockwiseNext(vertex);
                Edges.Add(new(vertex, nextVertex));
            }
        }
        for (int i = 0; i < RiverLayout.Layout.Count; i++)
            GenerateRiver(i);
    }

    /// <summary>
    /// select border nodes fit to type of river layout
    /// </summary>
    /// <param name="vertex"></param>
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
        List<EdgeD>? river = null;
        var startVisited = new HashSet<CoordinateD>();
        var finishVisited = new HashSet<CoordinateD>();
        var existed = River.ToHashSet();
        do
        {
            if (startVisited.Count == startNodes.Count && finishNodes.Count == finishNodes.Count)
                break;
            var start = startNodes[Random.Next(0, startNodes.Count)];
            var finish = finishNodes[Random.Next(0, finishNodes.Count)];
            startVisited.Add(start);
            finishVisited.Add(finish);
            var nodes = InnerNodes.ToList();
            nodes.AddRange([start, finish]);
            river = new Dijkstra(Edges.ToList(), nodes, start, finish).Path;
        } while (river is null || river.FirstOrDefault(existed.Contains) is not null);
        if (river is not null && river.FirstOrDefault(existed.Contains) is null)
            river.ForEach(e => River.Add(e));
        else
            Successful = false;
    }
}
