namespace AltitudeMapGenerator.Test;

public partial class ProcessForm : Form
{
    public float Total { get; set; } = 0;

    public float Now { get; set; } = 0;

    public ProcessForm()
    {
        InitializeComponent();
    }

    public void Progress()
    {
        var DoAction = delegate ()
        {
            this.Text = $"{Math.Round(Now / Total * 100, 2)}";
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
