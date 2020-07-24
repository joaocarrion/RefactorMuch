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
      var lastItemsArray = doc.Value<JArray>("lastItems");
      string[] lastItems = lastItemsArray != null ? lastItemsArray.Select(c => (string)c).ToArray() : null;

      if (ll != null)
        cbLeftDirectory.Text = ll;
      if (lr != null)
        cbRightDirectory.Text = lr;

      if (lastItems != null)
      {
        cbLeftDirectory.Items.AddRange(lastItems);
        cbRightDirectory.Items.AddRange(lastItems);
      }
    }

    private void ConfigSave()
    {
      doc["lastLeft"] = cbLeftDirectory.Text;
      doc["lastRight"] = cbRightDirectory.Text;

      var set = new HashSet<string>();
      foreach (var item in cbLeftDirectory.Items)
        set.Add(item.ToString());
      foreach (var item in cbRightDirectory.Items)
        set.Add(item.ToString());

      doc["lastItems"] = new JArray(set.ToArray());

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

    private void ButtonStart(object sender, EventArgs e)
    {
      if (!Directory.Exists(cbLeftDirectory.Text))
        MessageBox.Show("Left path is not a directory");
      else if (!Directory.Exists(cbRightDirectory.Text))
        MessageBox.Show("Left path is not a directory");
      else
      {
        btStartRefresh.Enabled = false;

        cbLeftDirectory.Items.Add(cbLeftDirectory.Text);
        cbRightDirectory.Items.Add(cbRightDirectory.Text);
        ConfigSave();
        
        treeView1.Nodes.Clear();
        SuspendLayout();
        Compare();
      }
    }


    private void UpdateProgress(object sender, EventArgs e)
    {
      if (compare != null)
      {
        taskProgress1.PercentDone = compare.Progress;
        taskProgress1.CurrentTask = compare.TaskName;
      }
    }

    private async void Compare()
    {
      // TODO: Settings
      var exts = "*.cs";
      string leftPath = cbLeftDirectory.Text;
      string rightPath = cbRightDirectory.Text;

      compare = new DirectoryCompare(leftPath, rightPath, exts);
      await compare.Parse();

      var rootNode = new TreeNode($"Comparison: {leftPath} x {rightPath}", 0, 0);
      var leftDuplicates = new TreeNode("Left Duplicates", 1, 1);
      var rightDuplicates = new TreeNode("Right Duplicates", 1, 1);

      treeView1.Nodes.Add(rootNode);
      rootNode.Nodes.Add(leftDuplicates);
      rootNode.Nodes.Add(rightDuplicates);

      AddDuplicates(leftDuplicates, compare.DuplicateLeft);
      AddDuplicates(rightDuplicates, compare.DuplicateRight);

      AddMovedOrChanged();

      rootNode.Expand();

      //// TODO: Move into file comparison
      //foreach (string file in compare.Filenames)
      //{
      //  FileCompareData lFile = null;
      //  FileCompareData rFile = null;

      //  if (left.ContainsKey(file))
      //    lFile = left[file];

      //  if (right.ContainsKey(file))
      //    rFile = right[file];

      //  if (lFile != null && rFile != null)
      //    AddMoveOrChanged(lFile, rFile);
      //  else if (lFile != null)
      //    AddSideOnly(lFile, true);
      //  else if (rFile != null)
      //    AddSideOnly(rFile, false);
      //}

      //foreach (var duplicate in compare.DuplicateLeft)
      //  AddMoveOrChanged(duplicate.left, duplicate.right);

      //// TODO: Enable/disable cross compare
      //foreach (var crossCompare in compare.CrossList)
      //  AddMoveOrChanged(crossCompare.left, crossCompare.right, crossCompare.similarity);

      btStartRefresh.Enabled = true;
      ResumeLayout();
    }

    private void AddMovedOrChanged()
    {
      var rootNode = treeView1.Nodes[0];
      var moved = rootNode.Nodes.Add("Moved Files");
      var changed = rootNode.Nodes.Add("Changed Files");

      Dictionary<string, TreeNode> movedGroup = new Dictionary<string, TreeNode>();
      Dictionary<string, TreeNode> changedGroup = new Dictionary<string, TreeNode>();
      foreach (var filename in compare.Filenames)
      {
        FileCompareData left;
        compare.LeftFiles.TryGetValue(filename, out left);

        FileCompareData right;
        compare.RightFiles.TryGetValue(filename, out right); // no pun intented

        if (left != null && right != null)
        {
          if (!left.localPath.Equals(right.localPath))
          {
            if (!movedGroup.ContainsKey(left.localPath))
            {
              movedGroup.Add(left.localPath, new TreeNode(left.localPath, 1, 1));
              moved.Nodes.Add(movedGroup[left.localPath]);
            }

            movedGroup[left.localPath].Nodes.Add(new MovedNode(left, right, 1));
          }

          if (!left.hash.SequenceEqual(right.hash))
          {
            if (!changedGroup.ContainsKey(left.localPath))
            {
              changedGroup.Add(left.localPath, new TreeNode(left.localPath, 1, 1));
              changed.Nodes.Add(changedGroup[left.localPath]);
            }

            changedGroup[left.localPath].Nodes.Add(new ChangedNode(left, right, 1));
          }
        }
      }
    }

    private void AddDuplicates(TreeNode root, HashSet<CrossCompare> cross)
    {
      Dictionary<string, TreeNode> duplicateNodes = new Dictionary<string, TreeNode>();
      foreach (var file in cross)
      {
        if (!duplicateNodes.ContainsKey(file.left.localPath))
        {
          var node = new TreeNode(file.left.localPath, 1, 1);
          root.Nodes.Add(node);
          duplicateNodes.Add(file.left.localPath, node);
        }

        duplicateNodes[file.left.localPath].Nodes.Add(new DuplicateNode(file.left, file.right, 1));
      }
    }

    //private void AddSideOnly(FileCompareData fileCompare, bool isLeft)
    //{
    //  LeftOnly leftOnly = new LeftOnly();
    //  leftOnly.LeftFile = fileCompare;
    //  leftOnly.IsLeft = isLeft;
    //  leftOnly.RightRootPath = isLeft ? compare.RightPath : compare.LeftPath;
    //  leftOnly.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

    //  flowPanel.Controls.Add(leftOnly);
    //  leftOnly.Width = flowPanel.Width - 48;
    //}

    //private void AddMoveOrChanged(FileCompareData lFile, FileCompareData rFile, float similarity = 1f)
    //{
    //  if (!lFile.localPath.Equals(rFile.localPath) || !lFile.hash.SequenceEqual(rFile.hash))
    //  {
    //    MovedPath movedPath = new MovedPath();
    //    movedPath.LeftFile = lFile;
    //    movedPath.RightFile = rFile;
    //    movedPath.Similarity = similarity;
    //    movedPath.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

    //    flowPanel.Controls.Add(movedPath);
    //    movedPath.Width = flowPanel.Width - 48;
    //  }
    //}

  }
}
