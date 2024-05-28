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
                (new(Directions.Left, OperatorType.LessThan, size), new(Directions.Right, OperatorType.LessThan, size)),
                (new(Directions.Left, OperatorType.GreaterThan, size), new(Directions.Right, OperatorType.GreaterThan, size))
                ),
            RiverLayout.Types.Vertical => (size) => new(
                (new(Directions.Top, OperatorType.LessThan, size), new(Directions.Bottom, OperatorType.LessThan, size)),
                (new(Directions.Top, OperatorType.GreaterThan, size), new(Directions.Bottom, OperatorType.GreaterThan, size))
                ),
            RiverLayout.Types.ForwardSlash => (size) => new(
                (new(Directions.Top, OperatorType.GreaterThanOrEqualTo, size), new(Directions.Left, OperatorType.GreaterThanOrEqualTo, size)),
                (new(Directions.Right, OperatorType.LessThanOrEqualTo, size), new(Directions.Bottom, OperatorType.LessThanOrEqualTo, size))
                ),
            RiverLayout.Types.BackwardSlash => (size) => new(
                (new(Directions.Left, OperatorType.LessThanOrEqualTo, size), new(Directions.Bottom, OperatorType.GreaterThanOrEqualTo, size)),
                (new(Directions.Top, OperatorType.LessThanOrEqualTo, size), new(Directions.Right, OperatorType.GreaterThanOrEqualTo, size))
                ),
            RiverLayout.Types.OneForTest => (size) => new(
                (new(Directions.Top, OperatorType.GreaterThanOrEqualTo, size), new(Directions.Left, OperatorType.GreaterThanOrEqualTo, size))
                ),
            _ => throw new InvalidOperationException()
        };
    }
}