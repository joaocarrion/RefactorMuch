using Newtonsoft.Json.Linq;
using RefactorMuch.Configuration;
using RefactorMuch.Controls;
using RefactorMuch.Controls.TreeNodes;
using RefactorMuch.Parse;
using RefactorMuch.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RefactorMuch
{
  public partial class DirectoryBrowse : UserControl
  {
    public delegate void FinishedParsing();

    private JObject doc;
    private ConfigData config;
    private Stopwatch processTime;
    private DirectoryCompare compare;
    private ContextMenuStrip rootNodeMenu;
    private SortedSet<FileCompareData> unlisted = new SortedSet<FileCompareData>();

    public static string LeftPath { get; private set; }
    public static string RightPath { get; private set; }
    public Dictionary<string, object> properties = new Dictionary<string, object>();
    private string filterExtensions;

    public DirectoryBrowse()
    {
      InitializeComponent();
    }

    private void DirectoryBrowse_Load(object sender, EventArgs e)
    {
      config = ConfigData.GetInstance();
      doc = config.doc;
      LoadProps();

      rootNodeMenu = new ContextMenuStrip();
      rootNodeMenu.Items.Add(new ToolStripMenuItem("Compare left and right folders...", null, DiffRoot));
    }

    private void DiffRoot(object sender, EventArgs e)
    {
      if (compare != null)
      {
        if (treeView1.SelectedNode == treeView1.Nodes[0])
          Tools.GetInstance().ToolDictionary[Tool.ToolType.Diff].Run(LeftPath, RightPath);
        else
        {
          var leftPath = LeftPath + treeView1.SelectedNode.Text;
          var rightPath = RightPath + treeView1.SelectedNode.Text;

          if (Directory.Exists(leftPath) && Directory.Exists(rightPath))
            Tools.GetInstance().ToolDictionary[Tool.ToolType.Diff].Run(leftPath, rightPath);
          else
            DialogHelper.InfoDialog("Directory does not exist in the other context");
        }
      }
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

      filterExtensions = "*.*";
      if (doc.ContainsKey("fileFilters"))
      {
        var filters = doc.Value<JObject>("fileFilters");
        var exts = filters.Value<JArray>("extensions");

        StringBuilder extensions = new StringBuilder();
        foreach (var ext in exts)
        {
          extensions.Append(ext.Value<string>());
          extensions.Append('|');
        }
        extensions.Remove(extensions.Length - 1, 1);
        filterExtensions = extensions.ToString();
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

        processTime = new Stopwatch();
        processTime.Start();
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
      SuspendLayout();
      LeftPath = cbLeftDirectory.Text;
      RightPath = cbRightDirectory.Text;
      treeView1.Nodes.Clear();

      // Directory compare
      compare = new DirectoryCompare(LeftPath, RightPath, filterExtensions);
      await compare.Parse();

      // create root noew
      treeView1.Nodes.Add(new TreeNode($"Comparison: {LeftPath} x {RightPath}", 0, 0));
      treeView1.Nodes[0].ContextMenuStrip = rootNodeMenu;

      unlisted.Clear();
      unlisted.UnionWith(compare.LeftFiles.Values);
      unlisted.UnionWith(compare.RightFiles.Values);

      // run tasks
      var tasks = MultiTask.Run(new Func<TreeNode>[]
      {
        () => { return AddInconsistencies(new TreeNode("Left Analysis", 1, 1), compare.DuplicateLeft, compare.RefactoredLeft, compare.EmptyFilesLeft); },
        () => { return AddInconsistencies(new TreeNode("Right Analysis", 1, 1), compare.DuplicateRight, compare.RefactoredRight, compare.EmptyFilesRight); },
        () => { return AddMoved(); },
        () => { return AddRenamed(); },
        () => { return AddChanged(); },
        () => { return AddRefactored(); },
        () => { return AddUnchangedFiles(); }
      });

      Task.WaitAll(tasks);
      treeView1.BeginUpdate();
      foreach (var t in tasks)
      {
        if (t.Result.Nodes.Count > 0)
          treeView1.Nodes[0].Nodes.Add(t.Result);
      }

      treeView1.Nodes[0].Nodes.Add(AddUnlisted());
      treeView1.EndUpdate();

      processTime.Stop();
      taskProgress1.Information = $"Processed in {processTime.ElapsedMilliseconds} ms";

      treeView1.Nodes[0].Expand();
      btStartRefresh.Enabled = true;
      ResumeLayout();

      var forget = Task.Run(() => { System.GC.Collect(); });
    }

    private TreeNode AddMoved() => AddCompareNodes(new TreeNode("Moved Files", 2, 2), compare.MovedSet, (CrossCompare compare) => { return new MovedNode(compare, 3); });
    private TreeNode AddRenamed() => AddCompareNodes(new TreeNode("Renamed Files", 2, 2), compare.RenamedSet, (CrossCompare compare) => { return new RenamedNode(compare, 3); });
    private TreeNode AddChanged() => AddCompareNodes(new TreeNode("Changed Files", 3, 3), compare.ChangedSet, (CrossCompare compare) => { return new ChangedNode(compare, 4); });
    private TreeNode AddDuplicates(TreeNode root, CrossCompareSet set) => AddCompareNodes(root, set, (CrossCompare compare) => { return new DuplicateNode(compare, 1); });
    private TreeNode AddRefactored(TreeNode root, CrossCompareSet set) => AddCompareNodes(root, set, (CrossCompare compare) => { return new RefactoredNode(compare, 4); });
    private TreeNode AddRefactored() => AddCompareNodes(new TreeNode("Refactored? Files", 4, 4), compare.CrossSet, (CrossCompare compare) => { return new RefactoredNode(compare, 4); });
    private TreeNode AddUnchangedFiles() => AddCompareNodes(new TreeNode("Unchanged Files", 1, 1), compare.UnchangedFileSet, (CrossCompare compare) => { return new UnchangedNode(compare, 4); });

    private TreeNode AddUnlisted()
    {
      var root = new TreeNode("Unlisted", 1, 1);
      SortedList<string, TreeNode> duplicatePath = new SortedList<string, TreeNode>(); //new StringCompareSizeFirst()

      // create directory nodes
      foreach (var file in unlisted)
      {
        if (!duplicatePath.ContainsKey(file.localPath))
        {
          var node = new TreeNode(file.localPath, 1, 1);
          node.ContextMenuStrip = rootNodeMenu;
          duplicatePath.Add(file.localPath, node);
        }
      }

      // add file nodes
      foreach (var file in unlisted)
        duplicatePath[file.localPath].Nodes.Add(new FileDataNode(file, 1));

      // add directory nodes to the tree
      foreach (var node in duplicatePath)
        root.Nodes.Add(node.Value);

      return root;
    }

    private TreeNode AddEmpty(TreeNode root, SortedSet<FileCompareData> data)
    {
      SortedList<string, TreeNode> duplicatePath = new SortedList<string, TreeNode>(); //new StringCompareSizeFirst()

      // create directory nodes
      foreach (var file in data)
      {
        if (!duplicatePath.ContainsKey(file.localPath))
        {
          var node = new TreeNode(file.localPath, 1, 1);
          node.ContextMenuStrip = rootNodeMenu;
          duplicatePath.Add(file.localPath, node);
        }
      }

      // add file nodes
      foreach (var file in data)
      {
        lock (unlisted) unlisted.Remove(file);
        duplicatePath[file.localPath].Nodes.Add(new FileDataNode(file, 1));
      }

      // add directory nodes to the tree
      foreach (var node in duplicatePath)
        root.Nodes.Add(node.Value);

      return root;
    }

    private TreeNode AddInconsistencies(TreeNode root, CrossCompareSet duplicates, CrossCompareSet refactored, SortedSet<FileCompareData> empty)
    {
      var tasks = MultiTask.Run(new Func<TreeNode>[]
      {
        () => { return AddDuplicates(new TreeNode("Left Duplicates", 1, 1), duplicates); },
        () => { return AddRefactored(new TreeNode("Left Refactored", 1, 1), refactored); },
        () => { return AddEmpty(new TreeNode("Left Empty", 1, 1), empty); },
      });

      Task.WhenAll(tasks);
      foreach (var t in tasks)
      {
        var node = t.Result;
        if (node.Nodes.Count > 0)
          root.Nodes.Add(t.Result);
      }

      return root;
    }

    private TreeNode AddCompareNodes(TreeNode root, CrossCompareSet set, Func<CrossCompare, TreeNode> constructor)
    {
      // sort directories
      SortedList<string, TreeNode> duplicatePath = new SortedList<string, TreeNode>(); //new StringCompareSizeFirst()

      // sort filenames
      var nameFirst = set.ToArray();
      Array.Sort(nameFirst, new CrossCompareNameFirst());

      // create directory nodes
      foreach (var file in nameFirst)
      {
        var smallerPath = file.left.SmallerLocalPath(file.right);
        if (!duplicatePath.ContainsKey(smallerPath.localPath))
        {
          var node = new TreeNode(smallerPath.localPath, 1, 1);
          node.ContextMenuStrip = rootNodeMenu;
          duplicatePath.Add(smallerPath.localPath, node);
        }
      }

      // add file nodes
      foreach (var file in nameFirst)
      {
        lock (unlisted)
        {
          unlisted.Remove(file.left);
          unlisted.Remove(file.right);
        }
        duplicatePath[file.left.SmallerLocalPath(file.right).localPath].Nodes.Add(constructor(file));
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

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (treeView1.SelectedNode is FileDataNode)
      {
        filePreview.Clear();
        foreach (string line in (treeView1.SelectedNode as FileDataNode).CodeLines)
          filePreview.AppendText(line + "\n");
      }
    }
  }
}
