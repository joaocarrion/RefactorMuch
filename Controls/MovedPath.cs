using System;
using System.Windows.Forms;
using System.Diagnostics;
using RefactorMuch.Parse;

namespace RefactorMuch.Controls
{
  public partial class MovedPath : UserControl
  {
    public static string vscode = "C:\\Program Files\\Microsoft VS Code\\Code.exe";
    public static string winmerge = "C:\\Program Files (x86)\\WinMerge\\WinMergeU.exe";

    public MovedPath()
    {
      InitializeComponent();
      lblPercent.Text = "100%";
    }

    private FileCompareData left = null;
    private FileCompareData right = null;

    public FileCompareData LeftFile {
      get { return left; }
      set {
        left = value;
        LeftPath = left.localPath;
        Filename = left.name;
      }
    }

    public FileCompareData RightFile {
      get { return right; }
      set {
        right = value;
        RightPath = right.localPath;
        Filename = right.name;
      }
    }

    public float Similarity {
      get => float.Parse(lblPercent.Text.Replace("%", ""));
      set {
        lblPercent.Text = $"{value:00}%";
      }
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
