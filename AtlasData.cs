using AtlasGenerator.Layout;
using LocalUtilities.GraphUtilities;

namespace AtlasGenerator;

public class AtlasData(Size size, Size segmentNumber, Size riverSegmentNumber, RiverLayout.Type riverLayoutType, int pixelNumber, float pixelDensity, IPointsGeneration pointsGeneration)
{
    public Size Size { get; set; } = size;

    public long Area => Size.Width * Size.Height;

    public Size SegmentNumber { get; set; } = segmentNumber;

    public Size RiverSegmentNumber { get; set; } = riverSegmentNumber;

    public RiverLayout.Type RiverLayoutType { get; set; } = riverLayoutType;

    public int PixelNumber { get; set; } = pixelNumber;

    public float PixelDensity { get; set; } = pixelDensity;

    public IPointsGeneration PointsGeneration { get; set; } = pointsGeneration;
}
