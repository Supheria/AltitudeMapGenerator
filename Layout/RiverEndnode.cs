using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.MathBundle;

namespace AtlasGenerator.Layout;

public class RiverEndnode(Direction direction, OperatorType operatorType, Size size)
{
    public Direction Direction { get; } = direction;

    public OperatorType OperatorType { get; } = operatorType;

    public double CompareValue { get; } = direction switch
    {

        Direction.Left or Direction.Right => size.Height / 2d,
        Direction.Top or Direction.Bottom => size.Width / 2d,
        _ => throw new InvalidOperationException()
    };

    public bool VoronoiVertexFilter(VoronoiVertex vertex)
    {
        if (vertex.DirectionOnBorder != Direction)
            return false;
        var value = Direction switch
        {
            Direction.Left or Direction.Right => vertex.Y,
            Direction.Top or Direction.Bottom => vertex.X,
            _ => throw new InvalidOperationException()
        };
        return value.ApproxOperatorTo(OperatorType, CompareValue);
    }
}
