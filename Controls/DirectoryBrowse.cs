using Newtonsoft.Json.Linq;
using RefactorMuch.Configuration;
using RefactorMuch.Controls.TreeNodes;
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
    public delegate void FinishedParsing();

    private JObject doc;
    private ConfigData config;
    private DirectoryCompare compare;

    public static string LeftPath { get; private set; }
    public static string RightPath { get; private set; }

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
      LeftPath = cbLeftDirectory.Text;
      RightPath = cbRightDirectory.Text;
      treeView1.Nodes.Clear();

      // Directory compare
      compare = new DirectoryCompare(LeftPath, RightPath, exts);
      await compare.Parse();

      // create root noew
      treeView1.Nodes.Add(new TreeNode($"Comparison: {LeftPath} x {RightPath}", 0, 0));

      // run tasks
      var tasks = MultiTask.Run(new Func<TreeNode>[]
      {
        () => { return AddDuplicates(new TreeNode("Left Duplicates", 1, 1), compare.DuplicateLeft); },
        () => { return AddDuplicates(new TreeNode("Right Duplicates", 1, 1), compare.DuplicateRight); },
        () => { return AddMoved(); },
        () => { return AddRenamed(); },
        () => { return AddChanged(); },
        () => { return AddRefactored(); }
      });

      Task.WaitAll(tasks);

      foreach (var t in tasks)
        treeView1.Nodes[0].Nodes.Add(t.Result);

      System.GC.Collect();
      treeView1.Nodes[0].Expand();
      btStartRefresh.Enabled = true;

      ResumeLayout();
    }

    private TreeNode AddMoved() => AddCompareNodes(new TreeNode("Moved Files", 2, 2), compare.MovedSet, (CrossCompare compare) => { return new MovedNode(compare, 3); });
    private TreeNode AddRenamed() => AddCompareNodes(new TreeNode("Renamed Files", 2, 2), compare.RenamedSet, (CrossCompare compare) => { return new RenamedNode(compare, 3); });
    private TreeNode AddChanged() => AddCompareNodes(new TreeNode("Changed Files", 3, 3), compare.ChangedSet, (CrossCompare compare) => { return new ChangedNode(compare, 4); });
    private TreeNode AddDuplicates(TreeNode root, CrossCompareSet set) => AddCompareNodes(root, set, (CrossCompare compare) => { return new DuplicateNode(compare, 1); });
    private TreeNode AddRefactored() => AddCompareNodes(new TreeNode("Refactored? Files", 4, 4), compare.CrossSet, (CrossCompare compare) => { return new RefactoredNode(compare, 4); });

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
        duplicatePath[file.left.SmallerLocalPath(file.right).localPath].Nodes.Add(constructor(file));

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
