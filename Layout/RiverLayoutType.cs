using LocalUtilities.TypeGeneral;

namespace AltitudeMapGenerator.Layout;

public static class RiverLayoutType
{
    public static Func<Size, RiverLayout> Parse(this RiverLayout.Types type)
    {
        // [Horizontal]   [Vertical)  [ForwardSlash)  [BackwardSlash)
        //    _______      _______       _______          _______
        //   | _____ |    | |   | |     |    /  |        |  \    |
        //   |       |    | |   | |     |      /|        |\      |
        //   |       |    | |   | |     |/      |        |      \|
        //   | ----- |    | |   | |     |  /    |        |    \  |
        //    -------      -------       -------          -------

        return type switch
        {
            RiverLayout.Types.Horizontal => (size) => new(
                (new(Direction.Left, OperatorType.LessThan, size), new(Direction.Right, OperatorType.LessThan, size)),
                (new(Direction.Left, OperatorType.GreaterThan, size), new(Direction.Right, OperatorType.GreaterThan, size))
                ),
            RiverLayout.Types.Vertical => (size) => new(
                (new(Direction.Top, OperatorType.LessThan, size), new(Direction.Bottom, OperatorType.LessThan, size)),
                (new(Direction.Top, OperatorType.GreaterThan, size), new(Direction.Bottom, OperatorType.GreaterThan, size))
                ),
            RiverLayout.Types.ForwardSlash => (size) => new(
                (new(Direction.Top, OperatorType.GreaterThanOrEqualTo, size), new(Direction.Left, OperatorType.GreaterThanOrEqualTo, size)),
                (new(Direction.Right, OperatorType.LessThanOrEqualTo, size), new(Direction.Bottom, OperatorType.LessThanOrEqualTo, size))
                ),
            RiverLayout.Types.BackwardSlash => (size) => new(
                (new(Direction.Left, OperatorType.LessThanOrEqualTo, size), new(Direction.Bottom, OperatorType.GreaterThanOrEqualTo, size)),
                (new(Direction.Top, OperatorType.LessThanOrEqualTo, size), new(Direction.Right, OperatorType.GreaterThanOrEqualTo, size))
                ),
            RiverLayout.Types.OneForTest => (size) => new(
                (new(Direction.Top, OperatorType.GreaterThanOrEqualTo, size), new(Direction.Left, OperatorType.GreaterThanOrEqualTo, size))
                ),
            _ => throw new InvalidOperationException()
        };
    }
}