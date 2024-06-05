using LocalUtilities.TypeToolKit.Mathematic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AltitudeMapGenerator.Test;

public partial class ProcessForm : Form
{
    public float Total { get; set; } = 0;

    public int Now { get; set; } = 0;

    Label Label { get; } = new()
    {
        Dock = DockStyle.Fill,
        Text = "aa"
    };

    BackgroundWorker BackgroundWorker { get; } = new();

    public ProcessForm()
    {
        //CheckForIllegalCrossThreadCalls = false;
        Controls.Add(Label);
    }

    private static readonly object locker = new();

    public void Progress(int count)
    {
        //lock (locker)
        {
            if (Label.InvokeRequired)
            {
                //lock (locker)
                {
                    Label.BeginInvoke(new ThreadStart(new Action(() =>
                    {
                        lock (locker)
                        {
                            //Now++;
                            Label.Text = Math.Round(++Now / Total * 100, 2).ToString();
                        }
                    })));
                }
            }
            else
            {
                //lock(locker)
                {
                    //Now++;
                    new ThreadStart(new Action(() =>
                    {
                        //Now++;
                        lock (locker)
                        {
                            Label.Text = Math.Round(++Now / Total * 100, 2).ToString();
                        }
                    })).Invoke();
                }
            }
        }
    }

    private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        Now++;
        Label.Text = Math.Round(Now / Total * 100, 2).ToString();
    }

    private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        //Label.Text = Math.Round(Now / Total * 100, 2).ToString();
    }
}
