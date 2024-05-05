using System.Net.Sockets;
using System.Net;
using System.Text;

namespace AtlasGenerator.Test;

public partial class TestForm : Form
{
    public float Total { get; set; } = 0;

    public float Now {  get; set; } = 0;

    public TestForm()
    {
        InitializeComponent();
    }

    public void Progress()
    {
        Action DoAction = delegate ()
        {
            this.Text = $"{Math.Round(Now / Total * 100, 2)}";
            this.Invalidate();
        };

        if (this.InvokeRequired)
        {
            ControlExtensions.UIThreadBeginInvoke(this, delegate
            {
                DoAction();
            });
        }
        else
        {
            DoAction();
        }
    }
}
