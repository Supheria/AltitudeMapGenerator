using LocalUtilities.TypeGeneral;

namespace AtlasGenerator.Layout;

public static class RiverLayoutType
{
    public static Func<Size, RiverLayout> Parse(this RiverLayout.Type type)
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
            RiverLayout.Type.Horizontal => (size) => new(
                (new(Direction.Left, OperatorType.LessThan, size), new(Direction.Right, OperatorType.LessThan, size)),
                (new(Direction.Left, OperatorType.GreaterThan, size), new(Direction.Right, OperatorType.GreaterThan, size))
                ),
            RiverLayout.Type.Vertical => (size) => new(
                (new(Direction.Top, OperatorType.LessThan, size), new(Direction.Bottom, OperatorType.LessThan, size)),
                (new(Direction.Top, OperatorType.GreaterThan, size), new(Direction.Bottom, OperatorType.GreaterThan, size))
                ),
            RiverLayout.Type.ForwardSlash => (size) => new(
                (new(Direction.Top, OperatorType.GreaterThanOrEqualTo, size), new(Direction.Left, OperatorType.GreaterThanOrEqualTo, size)),
                (new(Direction.Right, OperatorType.LessThanOrEqualTo, size), new(Direction.Bottom, OperatorType.LessThanOrEqualTo, size))
                ),
            RiverLayout.Type.BackwardSlash => (size) => new(
                (new(Direction.Left, OperatorType.LessThanOrEqualTo, size), new(Direction.Bottom, OperatorType.GreaterThanOrEqualTo, size)),
                (new(Direction.Top, OperatorType.LessThanOrEqualTo, size), new(Direction.Right, OperatorType.GreaterThanOrEqualTo, size))
                ),
            RiverLayout.Type.OneForTest => (size) => new(
                (new(Direction.Top, OperatorType.GreaterThanOrEqualTo, size), new(Direction.Left, OperatorType.GreaterThanOrEqualTo, size))
                ),
            _ => throw new InvalidOperationException()
        };
    }
}