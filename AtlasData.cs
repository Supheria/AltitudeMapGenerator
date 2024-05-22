﻿using AtlasGenerator.Layout;

namespace AtlasGenerator;

public class AtlasData(Size size, Size segmentNumber, Size riverSegmentNumber, RiverLayout.Type riverLayoutType, double riverWidth, int pixelNumber, float pixelDensity)
{
    public Size Size { get; set; } = size;

    public Size SegmentNumber { get; set; } = segmentNumber;

    public Size RiverSegmentNumber { get; set; } = riverSegmentNumber;

    public double RiverWidth { get; set; } = riverWidth;

    public RiverLayout.Type RiverLayoutType { get; set; } = riverLayoutType;

    public int PixelNumber { get; set; } = pixelNumber;

    public float PixelDensity { get; set; } = pixelDensity;
}
