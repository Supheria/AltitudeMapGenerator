using AtlasGenerator.Layout;

namespace AtlasGenerator;

public class AtlasData(string name, Size size, Size segmentNumber, Size riverSegmentNumber, RiverLayout.Type riverLayoutType, int pixelNumber, float pixelDensity, IPointsGeneration pointsGeneration)
{
    public string Name { get; set; } = name;

    public Size Size { get; set; } = size;

    public long Area => Size.Width * Size.Height;

    public Size SegmentNumber { get; set; } = segmentNumber;

    public Size RiverSegmentNumber { get; set; } = riverSegmentNumber;

    public RiverLayout.Type RiverLayoutType { get; set; } = riverLayoutType;

    public int PixelNumber { get; set; } = pixelNumber;

    public float PixelDensity { get; set; } = pixelDensity;

    public IPointsGeneration PointsGeneration { get; set; } = pointsGeneration;
}
