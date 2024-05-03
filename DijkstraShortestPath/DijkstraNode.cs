﻿using LocalUtilities.MathBundle;

namespace AtlasGenerator.DijkstraShortestPath;

internal class DijkstraNode(Coordinate node, int index)
{
    public bool Used { get; set; } = false;
    public List<Coordinate> Nodes { get; } = [];

    public Coordinate Node { get; set; } = node;

    public int Index { get; set; } = index;

    public double Weight { get; set; } = double.MaxValue;
}