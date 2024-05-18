using AtlasGenerator.VoronoiDiagram.Data;
using LocalUtilities.TypeGeneral;
using LocalUtilities.TypeToolKit.Mathematic;

namespace AtlasGenerator.Layout;

public class RiverEndnode(Direction direction, OperatorType operatorType, Size size)
{
    internal Direction Direction { get; } = direction;

    internal OperatorType OperatorType { get; } = operatorType;

    internal double CompareValue { get; } = direction switch
    {
        Direction.Left or Direction.Right => size.Height / 2d,
        Direction.Top or Direction.Bottom => size.Width / 2d,
        _ => throw AtlasException.NotProperRiverEndnodeDirection(direction)
    };

    internal bool VoronoiVertexFilter(VoronoiVertex vertex)
    {
        if (vertex.DirectionOnBorder != Direction)
            return false;
        var value = Direction switch
        {
            Direction.Left or Direction.Right => vertex.Y,
            Direction.Top or Direction.Bottom => vertex.X,
            _ => throw AtlasException.NotProperRiverEndnodeDirection(Direction)
        };
        return value.ApproxOperatorTo(OperatorType, CompareValue);
    }
}
