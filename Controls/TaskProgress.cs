using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public partial class TaskProgress : UserControl
  {
    public TaskProgress()
    {
      InitializeComponent();
    }

    public int PercentDone { get => progressBar1.Value; set => progressBar1.Value = value; }
    public string CurrentTask { get => lbTaskInfo.Text; set => lbTaskInfo.Text = value; }
    public ProgressBarStyle Style { get => progressBar1.Style; set => progressBar1.Style = value; }
  }
}
