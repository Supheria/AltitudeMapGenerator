using AtlasGenerator.Common;

namespace AtlasGenerator.DijkstraShortestPath;

internal class DijkstraNode(CoordinateD node, int index)
{
    internal bool Used { get; set; } = false;
    internal List<CoordinateD> Nodes { get; } = [];

    internal CoordinateD Coordinate { get; set; } = node;

    internal int Index { get; set; } = index;

    internal double Weight { get; set; } = double.MaxValue;
}
