using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.MathBundle;

namespace AtlasGenerator.VoronoiDiagram.BorderDisposal;

internal class EdgeStartBorderNode(VoronoiEdge edge, int fallbackComparisonIndex) :
        EdgeBorderNode(edge, fallbackComparisonIndex)
{
    public override Direction BorderLocation => Edge.Starter.DirectionOnBorder;

    public override VoronoiVertex Vertex => Edge.Starter;

    public override double Angle => Vertex.AngleTo(Edge.Ender); // away from border

#if DEBUG
    public override string ToString()
    {
        return "Edge Start " + base.ToString();
    }
#endif
}