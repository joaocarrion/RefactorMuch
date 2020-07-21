using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InteractiveMerge.Configuration;
using System.IO;
using InteractiveMerge.Parse;
using System.Reflection;
using System.Drawing.Text;
using InteractiveMerge.Controls;

namespace InteractiveMerge
{
  public partial class DirectoryBrowse : UserControl
  {
    public delegate void FinishedParsing();

    //private ConfigData dbConfiguration;
    private DirectoryCompare compare;
    //private DictionaryProperty configuration;
    private KeyValuePair<string, Property>[] propertiesInitializer = new KeyValuePair<string, Property>[]
    {
      new KeyValuePair<string, Property>("lastLeftDir", new StringProperty("")),
      new KeyValuePair<string, Property>("lastRightDir", new StringProperty("")),
      new KeyValuePair<string, Property>("recentList", new ArrayProperty()),
      new KeyValuePair<string, Property>("filterExtensions", new ArrayProperty(new object[] { "*.cs" })),
      new KeyValuePair<string, Property>("fileFilter", new ArrayProperty())
    };

    private ArrayProperty recentList, filterExtentions, recentFilter;

    public FinishedParsing OnFinishedParsing { get; set; }

    public DirectoryBrowse()
    {
      InitializeComponent();

      //dbConfiguration = new ConfigData(Application.LocalUserAppDataPath + "/app.config");
      //var properties = dbConfiguration.config;
      //if (!properties.ContainsKey(Name))
      //  properties.Add(Name, configuration = new DictionaryProperty());
      //else
      //  configuration = (DictionaryProperty)properties[Name];
    }

    private void DirectoryBrowse_Load(object sender, EventArgs e)
    {
      //LoadProps();
    }

    //private void LoadProps()
    //{
    //  foreach (var kv in propertiesInitializer)
    //    if (!configuration.ContainsKey(kv.Key))
    //      configuration[kv.Key] = kv.Value;

    //  recentList = (ArrayProperty)configuration["recentList"];
    //  filterExtentions = (ArrayProperty)configuration["filterExtensions"];
    //  recentFilter = (ArrayProperty)configuration["fileFilter"];

    //  cbLeftDirectory.Items.AddRange(recentList.ToArray());
    //  cbRightDirectory.Items.AddRange(recentList.ToArray());

    //  // TODO: fill last entry and when start/refresh are pressed, update
    //  cbLeftDirectory.Text = configuration["lastLeftDir"];
    //  cbRightDirectory.Text = configuration["lastRightDir"];
    //}

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
        //configuration["lastLeftDir"].value = cbLeftDirectory.Text;
        //configuration["lastRightDir"].value = cbRightDirectory.Text;
        //cbLeftDirectory.Items.Add(cbLeftDirectory.Text);
        //cbRightDirectory.Items.Add(cbRightDirectory.Text);

        //ConfigSave();
        flowPanel.Controls.Clear();
        Compare();
      }
    }

    private void ConfigSave()
    {
      foreach (var item in cbLeftDirectory.Items)
        if (!recentList.Contains(item))
          recentList.Add(item);

      foreach (var item in cbRightDirectory.Items)
        if (!recentList.Contains(item))
          recentList.Add(item);

      //dbConfiguration.Save();
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

      var left = compare.Left;
      var right = compare.Right;

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
          AddSideOnly(lFile);
        else if (rFile != null && rFile.parsed)
          AddSideOnly(rFile);
      }
    }

    private void AddSideOnly(FileCompareData fileCompare)
    {
      LeftOnly leftOnly = new LeftOnly();
      leftOnly.LeftPath = fileCompare.path;
      leftOnly.Filename = fileCompare.name;
      leftOnly.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

      flowPanel.Controls.Add(leftOnly);
      leftOnly.Width = flowPanel.Width - 48;
    }

    private void AddMoved(FileCompareData lFile, FileCompareData rFile)
    {
      MovedPath movedPath = new MovedPath();
      movedPath.LeftPath = lFile.path;
      movedPath.RightPath = rFile.path;
      movedPath.Filename = rFile.name;
      movedPath.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

      flowPanel.Controls.Add(movedPath);
      movedPath.Width = flowPanel.Width - 48;
    }

    private void AddUnchangedPath(FileCompareData lFile, FileCompareData rFile)
    {
    }
  }
}
