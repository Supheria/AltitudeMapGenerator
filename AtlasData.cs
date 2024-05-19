using AtlasGenerator.Layout;

namespace AtlasGenerator;

public class AtlasData(string name, Size size, Size segmentNumber, Size riverSegmentNumber, RiverLayout.Type riverLayoutType, double riverWidth, int pixelNumber, float pixelDensity)
{
    public string Name { get; set; } = name;

    public Size Size { get; set; } = size;

    public Size SegmentNumber { get; set; } = segmentNumber;

    public Size RiverSegmentNumber { get; set; } = riverSegmentNumber;

    public double RiverWidth { get; set; } = riverWidth;

    public RiverLayout.Type RiverLayoutType { get; set; } = riverLayoutType;

    public int PixelNumber { get; set; } = pixelNumber;

    public float PixelDensity { get; set; } = pixelDensity;
}
