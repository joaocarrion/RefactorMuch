using Newtonsoft.Json.Linq;
using RefactorMuch.Configuration;
using RefactorMuch.Controls;
using RefactorMuch.Parse;
using RefactorMuch.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
      config = ConfigData.GetInstance();
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
      btStartRefresh.Enabled = false;
      SuspendLayout();

      // TODO: Settings
      var exts = "*.cs";
      string leftPath = cbLeftDirectory.Text;
      string rightPath = cbRightDirectory.Text;

      // Directory compare
      compare = new DirectoryCompare(leftPath, rightPath, exts);
      await compare.Parse();

      // create root noew
      treeView1.Nodes.Add(new TreeNode($"Comparison: {leftPath} x {rightPath}", 0, 0));

      // run tasks
      var tasks = MultiTask.Run(new Func<TreeNode>[]
      {
        () => { return AddDuplicates(new TreeNode("Left Duplicates", 1, 1), compare.DuplicateLeft); },
        () => { return AddDuplicates(new TreeNode("Right Duplicates", 1, 1), compare.DuplicateRight); },
        () => { return AddMoved(); },
        () => { return AddChanged(); },
        () => { return AddSimilar(); }
      });

      Task.WaitAll(tasks);

      foreach (var t in tasks)
        treeView1.Nodes[0].Nodes.Add(t.Result);

      System.GC.Collect();
      treeView1.Nodes[0].Expand();
      btStartRefresh.Enabled = true;

      ResumeLayout();
    }

    private TreeNode AddMoved()
    {
      CrossCompareSet movedSet = new CrossCompareSet(1f);
      foreach (var filename in compare.Filenames)
      {
        FileCompareData left = Find(filename, compare.LeftFiles);
        FileCompareData right = Find(filename, compare.RightFiles);

        if (left != null && right != null && !left.localPath.Equals(right.localPath) && left.hash.Equals(right.hash))
          movedSet.Add(new CrossCompare(left, right, 1f));
      }

      return AddCompareNodes(new TreeNode("Moved Files", 2, 2), movedSet, (CrossCompare compare) => { return new MovedNode(compare, 3); });
    }

    private TreeNode AddChanged()
    {
      CrossCompareSet changedSet = new CrossCompareSet(1f);
      foreach (var filename in compare.Filenames)
      {
        FileCompareData left = Find(filename, compare.LeftFiles);
        FileCompareData right = Find(filename, compare.RightFiles);

        if (left != null && right != null && left.localPath.Equals(right.localPath) && !left.hash.Equals(right.hash))
          changedSet.Add(new CrossCompare(left, right, 1f));
      }

      return AddCompareNodes(new TreeNode("Changed Files", 3, 3), changedSet, (CrossCompare compare) => { return new ChangedNode(compare, 3); });
    }

    private TreeNode AddDuplicates(TreeNode root, CrossCompareSet set) => AddCompareNodes(root, set, (CrossCompare compare) => { return new DuplicateNode(compare, 1); });
    private TreeNode AddSimilar() => AddCompareNodes(new TreeNode("Similar Files", 4, 4), compare.CrossSet, (CrossCompare compare) => { return new SimilarNode(compare, 4); });

    private TreeNode AddCompareNodes(TreeNode root, CrossCompareSet set, Func<CrossCompare, TreeNode> constructor)
    {
      // sort directories
      SortedList<string, TreeNode> duplicatePath = new SortedList<string, TreeNode>(new StringCompareSizeFirst());

      // sort filenames
      var nameFirst = set.ToArray();
      Array.Sort(nameFirst, new CrossCompareNameFirst());

      // create directory nodes
      foreach (var file in nameFirst)
      {
        var smallerPath = file.left.SmallerLocalPath(file.right);
        if (!duplicatePath.ContainsKey(smallerPath.localPath))
          duplicatePath.Add(smallerPath.localPath, new TreeNode(smallerPath.localPath, 1, 1));
      }

      // add file nodes
      foreach (var file in nameFirst)
      {
        var smallerPath = file.left.SmallerLocalPath(file.right);
        duplicatePath[smallerPath.localPath].Nodes.Add(constructor(file));
      }

      // add directory nodes to the tree
      foreach (var node in duplicatePath)
        root.Nodes.Add(node.Value);

      return root;
    }

    private void MouseClickTreeView(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
    }

    private class StringCompareSizeFirst : IComparer<string>
    {
      public int Compare(string x, string y) => x.Length < y.Length ? -1 : (x.Length > y.Length) ? 1 : string.Compare(x, y, true);
    }

    private class CrossCompareNameFirst : IComparer<CrossCompare>
    {
      public int Compare(CrossCompare x, CrossCompare y)
      {
        var smallerX = x.left.SmallerLocalPath(x.right);
        var smallerY = y.left.SmallerLocalPath(y.right);
        if (smallerX.name.Equals(smallerY.name))
          return x.CompareTo(y);
        else
          return string.Compare(smallerX.name, smallerY.name, true);
      }
    }

    private FileCompareData Find(string filename, Dictionary<string, FileCompareData> dictionary)
    {
      FileCompareData file;
      dictionary.TryGetValue(filename, out file);
      return file;
    }
  }
}
