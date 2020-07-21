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
  public partial class MovedPath : UserControl
  {
    public static string vscode = "C:\\Program Files\\Microsoft VS Code\\Code.exe";
    public static string winmerge = "C:\\Program Files (x86)\\WinMerge\\WinMergeU.exe";

    public MovedPath()
    {
      InitializeComponent();
    }
    
    public string LeftPath { get => lbLeftPath.Text; set { lbLeftPath.Text = value; toolTip1.SetToolTip(lbLeftPath, value); } }
    public string RightPath { get => lbRightPath.Text; set { lbRightPath.Text = value; toolTip1.SetToolTip(lbRightPath, value); } }
    public string Filename { get => lbFilename.Text; set { lbFilename.Text = value; toolTip1.SetToolTip(lbFilename, value); } }

    private void btDiff_Click(object sender, EventArgs e)
    {
      Process.Start(winmerge, $"/s \"{LeftPath}\" \"{RightPath}\"");
    }
  }
}
