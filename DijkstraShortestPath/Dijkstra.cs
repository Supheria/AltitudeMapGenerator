using AtlasGenerator.Common;
using AtlasGenerator.VoronoiDiagram;

namespace AtlasGenerator.DijkstraShortestPath;

internal class Dijkstra
{
    double[,] Graph { get; }

    List<DijkstraEdge> Edges { get; }

    List<DijkstraNode> Nodes { get; }

    internal List<EdgeD>? Path { get; private set; }

    internal Dijkstra(List<EdgeD> edges, List<CoordinateD> vertexes, CoordinateD startVertex, CoordinateD finishVertex)
    {
        if (vertexes.FirstOrDefault(c => c == startVertex) is null)
            throw VoronoiException.NoMatchVertexInDijkstra(nameof(startVertex));
        if (vertexes.FirstOrDefault(c => c == finishVertex) is null)
            throw VoronoiException.NoMatchVertexInDijkstra(nameof(finishVertex));
        Edges = edges.Select(e => new DijkstraEdge(e)).ToList();
        Nodes = [];
        Graph = new double[vertexes.Count, vertexes.Count];
        foreach (var row in Enumerable.Range(0, vertexes.Count))
        {
            var rowNode = vertexes[row];
            foreach (var colnum in Enumerable.Range(0, vertexes.Count))
            {
                if (row == colnum)
                {
                    Graph[row, colnum] = 0;
                    continue;
                }
                var edge = Edges.FirstOrDefault(x => x.Edge.Starter == rowNode && x.Edge.Ender == vertexes[colnum]);
                Graph[row, colnum] = edge == null ? double.MaxValue : edge.Weight;
            }
            Nodes.Add(new(vertexes[row], row));
        }
        Path = GetPath(startVertex, finishVertex);
    }

    private List<EdgeD>? GetPath(CoordinateD startVertex, CoordinateD endVertex)
    {
        VoronoiException.ThrowIfCountZero(Nodes, "dijkstra nodes");
        Nodes.First(n => n.Coordinate == startVertex).Used = true;
        Nodes.ForEach(x =>
        {
            var index = 0;
            while (index < Nodes.Count)
            {
                if (startVertex == Nodes[index].Coordinate)
                    break;
                index++;
            }
            x.Weight = GetRowArray(index)[x.Index];
            x.Nodes.Add(startVertex);
        });
        while (Nodes.Any(x => !x.Used))
        {
            var item = GetUnUsedAndMinNodeItem();
            if (item == null)
                break;
            item.Used = true;
            var tempRow = GetRowArray(item.Index);
            foreach (var nodeItem in Nodes)
            {
                if (nodeItem.Weight > tempRow[nodeItem.Index] + item.Weight)
                {
                    nodeItem.Weight = tempRow[nodeItem.Index] + item.Weight;
                    nodeItem.Nodes.Clear();
                    nodeItem.Nodes.AddRange(item.Nodes);
                    nodeItem.Nodes.Add(item.Coordinate);
                }
            }
        }
        var desNodeitem = Nodes.First(x => x.Coordinate == endVertex);
        if (!(desNodeitem.Used && desNodeitem.Weight < double.MaxValue))
            return null;
        var path = new List<EdgeD>();
        foreach (var index in Enumerable.Range(0, desNodeitem.Nodes.Count - 1))
        {
            var e = Edges.FirstOrDefault(e => e.Edge.Starter == desNodeitem.Nodes[index] && e.Edge.Ender == desNodeitem.Nodes[index + 1]);
            if (e is not null)
                path.Add(e.Edge);
        }
        var edge = Edges.FirstOrDefault(x => x.Edge.Starter == desNodeitem.Nodes.Last() && x.Edge.Ender == endVertex);
        if (edge is not null)
            path.Add(edge.Edge);
        return path;
    }

    private DijkstraNode? GetUnUsedAndMinNodeItem()
    {
        return Nodes.Where(x => !x.Used && x.Weight != double.MaxValue).OrderBy(x => x.Weight).FirstOrDefault();
    }

    private double[] GetRowArray(int row)
    {
        double[] result = new double[Graph.GetLength(1)];
        foreach (var index in Enumerable.Range(0, result.Length))
        {
            result[index] = Graph[row, index];
        }
        return result;
    }
}
