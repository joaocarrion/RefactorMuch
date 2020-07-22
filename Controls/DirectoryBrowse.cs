using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RefactorMuch.Configuration;
using System.IO;
using RefactorMuch.Parse;
using RefactorMuch.Controls;
using Newtonsoft.Json.Linq;

namespace RefactorMuch
{
  public partial class DirectoryBrowse : UserControl
  {
    private DirectoryCompare compare;
    private ConfigData config;
    private JObject doc;

    public delegate void FinishedParsing();
    public FinishedParsing OnFinishedParsing { get; set; }

    public Dictionary<string, object> properties = new Dictionary<string, object>();

    public DirectoryBrowse()
    {
      InitializeComponent();
    }

    private void DirectoryBrowse_Load(object sender, EventArgs e)
    {
      config = new ConfigData(Application.LocalUserAppDataPath + "\\app.config");
      doc = config.doc;
      LoadProps();
    }

    private void LoadProps()
    {
      var ll = doc.Value<string>("lastLeft");
      var lr = doc.Value<string>("lastRight");
      var listItems = doc.Value<string []>("lastItems");

      if (ll != null)
        cbLeftDirectory.Text = ll;
      if (lr != null)
        cbRightDirectory.Text = lr;
      
      if (listItems != null)
      {
        cbLeftDirectory.Items.AddRange(listItems);
        cbRightDirectory.Items.AddRange(listItems);
      }
    }

    private void ConfigSave()
    {
      doc.Add("lastLeft", cbLeftDirectory.Text);
      doc.Add("lastRight", cbRightDirectory.Text);
      
      var set = new HashSet<string>();
      foreach (var item in cbLeftDirectory.Items)
        set.Add(item.ToString());
      foreach (var item in cbRightDirectory.Items)
        set.Add(item.ToString());
      doc.Add("lastItems", new JArray(set.ToArray()));

      config.Save();
    }

    private void btBrowseLeft_Click(object sender, EventArgs e)
    {
      bool left = sender == btBrowseLeft;

      FolderBrowserDialog dialog = new FolderBrowserDialog();
      dialog.Description = "Select folder to compare";
      if (dialog.ShowDialog() == DialogResult.OK)
        if (left)
          cbLeftDirectory.Text = dialog.SelectedPath;
        else
          cbRightDirectory.Text = dialog.SelectedPath;
    }

    private void btStartRefresh_Click(object sender, EventArgs e)
    {
      if (!Directory.Exists(cbLeftDirectory.Text))
        MessageBox.Show("Left path is not a directory");
      else if (!Directory.Exists(cbRightDirectory.Text))
        MessageBox.Show("Left path is not a directory");
      else
      {
        cbLeftDirectory.Items.Add(cbLeftDirectory.Text);
        cbRightDirectory.Items.Add(cbRightDirectory.Text);
        ConfigSave();

        flowPanel.Controls.Clear();
        Compare();
      }
    }


    private void updateProgess_Tick(object sender, EventArgs e)
    {
      if (compare != null)
      {
        taskProgress1.PercentDone = compare.Progress;
        taskProgress1.CurrentTask = compare.TaskName;
      }
    }

    private async void Compare()
    {
      var exts = "*.cs"; //Aggregate(filterExtentions);
      string leftPath = cbLeftDirectory.Text;
      string rightPath = cbRightDirectory.Text;

      compare = new DirectoryCompare(leftPath, rightPath, exts);
      await compare.Parse();

      var left = compare.LeftFiles;
      var right = compare.RightFiles;

      // TODO: Move into file comparison
      foreach (string file in compare.Filenames)
      {
        FileCompareData lFile = null;
        FileCompareData rFile = null;

        if (left.ContainsKey(file))
          lFile = left[file];

        if (right.ContainsKey(file))
          rFile = right[file];

        if (lFile != null && rFile != null && lFile.parsed && rFile.parsed)
        {
          // Add only different
          if (!lFile.hash.SequenceEqual(rFile.hash))
            AddMoved(lFile, rFile);
        }
        else if (lFile != null && lFile.parsed)
          AddSideOnly(lFile, true);
        else if (rFile != null && rFile.parsed)
          AddSideOnly(rFile, false);
      }

      // TODO: Enable/disable cross compare
      foreach (var crossCompare in compare.CrossList)
      {
        AddMoved(crossCompare.left, crossCompare.right, crossCompare.similarity);
      }
    }

    private void AddSideOnly(FileCompareData fileCompare, bool isLeft)
    {
      LeftOnly leftOnly = new LeftOnly();
      leftOnly.LeftFile = fileCompare;
      leftOnly.IsLeft = isLeft;
      leftOnly.RightRootPath = isLeft ? compare.RightPath : compare.LeftPath;
      leftOnly.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

      flowPanel.Controls.Add(leftOnly);
      leftOnly.Width = flowPanel.Width - 48;
    }

    private void AddMoved(FileCompareData lFile, FileCompareData rFile, float similarity = 1f)
    {
      MovedPath movedPath = new MovedPath();
      movedPath.LeftFile = lFile;
      movedPath.RightFile = rFile;
      movedPath.Similarity = similarity;
      movedPath.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

      flowPanel.Controls.Add(movedPath);
      movedPath.Width = flowPanel.Width - 48;
    }

    private void AddUnchangedPath(FileCompareData lFile, FileCompareData rFile)
    {
    }
  }
}
