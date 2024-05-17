using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.TypeGeneral;

namespace AtlasGenerator.VoronoiDiagram.BorderDisposal;

internal class CornerBorderNode(VoronoiVertex point) : BorderNode
{
    public override Direction BorderLocation { get; } = point.DirectionOnBorder;

    public override VoronoiVertex Vertex { get; } = point;

    public override double Angle => throw new InvalidOperationException();

    public override int FallbackComparisonIndex => throw new InvalidOperationException();

#if DEBUG
    public override string ToString()
    {
        return "Corner " + base.ToString();
    }
#endif
}
