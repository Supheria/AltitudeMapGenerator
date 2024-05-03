using AtlasGenerator.VoronoiDiagram.Data;

namespace AtlasGenerator.VoronoiDiagram.BorderDisposal;

internal abstract class EdgeBorderNode(VoronoiEdge edge, int fallbackComparisonIndex) : BorderNode
{
    public VoronoiEdge Edge { get; } = edge;

    public override int FallbackComparisonIndex { get; } = fallbackComparisonIndex;
}
