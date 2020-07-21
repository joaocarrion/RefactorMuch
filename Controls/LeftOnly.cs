using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace InteractiveMerge.Controls
{
  public partial class LeftOnly : UserControl
  {
    public LeftOnly()
    {
      InitializeComponent();
    }
    public string LeftPath { get => lbLeftPath.Text; set { lbLeftPath.Text = value; toolTip1.SetToolTip(lbLeftPath, value); } }
    public string Filename { get => lbFilename.Text; set { lbFilename.Text = value; toolTip1.SetToolTip(lbFilename, value); } }

    private void btDiff_Click(object sender, EventArgs e)
    {
      Process.Start(MovedPath.vscode, $"\"{LeftPath}\"");
    }
  }
}
