using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using RefactorMuch.Parse;

namespace RefactorMuch.Controls
{
  public partial class LeftOnly : UserControl
  {
    private FileCompareData fileData;

    public LeftOnly()
    {
      InitializeComponent();
    }

    private bool isLeft = true;
    public bool IsLeft {
      get => IsLeft;
      set {
        isLeft = value;
        lblIsLeft.Text = isLeft ? "<" : ">";
        toolTip1.SetToolTip(lblIsLeft, isLeft ? "Left only" : "Right only");
      }
    }

    public FileCompareData LeftFile {
      get { return fileData; }
      set {
        fileData = value;
        LeftPath = fileData.localPath;
        Filename = fileData.name;
      }
    }

    public string RightRootPath { get; set; }

    public string LeftPath { get => lbLeftPath.Text; set { lbLeftPath.Text = value; toolTip1.SetToolTip(lbLeftPath, value); } }

    public string Filename { get => lbFilename.Text; set { lbFilename.Text = value; toolTip1.SetToolTip(lbFilename, value); } }

    private void btDiff_Click(object sender, EventArgs e)
    {
      Process.Start(MovedPath.vscode, $"\"{fileData.absolutePath}\"");
    }

    private void btCopy_Click(object sender, EventArgs e)
    {
      string source = fileData.path;
      string destination = RightRootPath + fileData.localPath;

      try {
        Directory.CreateDirectory(destination);
        File.Copy(source, destination + fileData.name);
      }
      catch (Exception) { MessageBox.Show("Failed to copy", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
  }
}
