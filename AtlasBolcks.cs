using AtlasGenerator.DLA;
using LocalUtilities.MathBundle;
using LocalUtilities.TypeBundle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasGenerator;

public class AtlasBolcks : SerializableTagValues<Coordinate, DlaPixel>
{
    public override string LocalName { get; set; } = nameof(AtlasBolcks);

    public override string KeyName { get; set; } = "Site";

    protected override Func<Coordinate, string> WriteKey => c => c.ToIntString();

    protected override Func<DlaPixel, string> WriteValue => p => StringTypeConverter.ToArrayString(p.X, p.Y, p.Height);

    protected override Func<string, Coordinate> ReadKey => s => s.ToCoordinate(new());

    protected override Func<string, DlaPixel> ReadValue => s =>
    {
        var list = s.ToArray();
        if (list.Length is not 3)
            return new();
        return new(list[0].ToInt(0), list[1].ToInt(0)) { Height = list[2].ToInt(0) };
    };
}
