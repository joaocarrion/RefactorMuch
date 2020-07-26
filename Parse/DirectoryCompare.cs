using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RefactorMuch.Parse
{
  public partial class DirectoryCompare
  {
    private int totalFiles = 0;
    private int fileScanIndex = 0;
    private int totalDirectories = 0;
    private int directoryScanIndex = 0;
    private long crossCompareIndex = 0;
    private long totalCrossCompare = 0;

    public string LeftPath { get; }
    public string RightPath { get; }
    public SortedSet<string> Filenames { get; private set; } = new SortedSet<string>();
    public Dictionary<string, FileCompareData> LeftFiles { get; private set; } = new Dictionary<string, FileCompareData>();
    public Dictionary<string, FileCompareData> RightFiles { get; private set; } = new Dictionary<string, FileCompareData>();
    public CrossCompareSet DuplicateLeft { get; private set; } = new CrossCompareSet(1f);
    public CrossCompareSet DuplicateRight { get; private set; } = new CrossCompareSet(1f);

    // TODO: In the future, semantic comparison would be nice instead
    public CrossCompareSet CrossSet { get; private set; } = new CrossCompareSet(0.45f);

    public int Progress => ProgressCount();

    public string TaskName { get; private set; } = "Idle";

    public string FileFilter { get; }

    public DirectoryCompare(string left, string right, string fileFilter)
    {
      LeftPath = left;
      RightPath = right;
      FileFilter = fileFilter;
    }

    public async Task Parse()
    {
      totalFiles = 0;
      fileScanIndex = 0;
      totalDirectories = 0;
      directoryScanIndex = 0;
      crossCompareIndex = 0;
      totalCrossCompare = 0;

      var taskLeft = Task.Run(() =>
      {
        var files = ListFiles(LeftPath);
        totalFiles += files.Count;

        CompareSets left = new CompareSets
        {
          FileList = files,
          CrossCompareList = CrossSet,
          Duplicates = DuplicateLeft,
          Filenames = Filenames,
          Files = LeftFiles,
          Path = LeftPath
        };

        return ParseFiles(left);
      });

      var taskRight = Task.Run(() =>
      {
        var files = ListFiles(RightPath);
        totalFiles += files.Count;

        CompareSets right = new CompareSets
        {
          FileList = files,
          CrossCompareList = CrossSet,
          Duplicates = DuplicateRight,
          Filenames = Filenames,
          Files = RightFiles,
          Path = RightPath
        };

        return ParseFiles(right);
      });

      var leftSet = await taskLeft;
      var rightSet = await taskRight;

      totalCrossCompare = leftSet.Files.Count * leftSet.Files.Count + rightSet.Files.Count * rightSet.Files.Count + leftSet.Files.Count * rightSet.Files.Count;
      var taskCompareLeft = Task.Run(() => { CrossCompare(leftSet, leftSet); });
      var taskCompareRight = Task.Run(() => { CrossCompare(rightSet, rightSet); });
      var taskCompareLeftRight = Task.Run(() => { CrossCompare(leftSet, rightSet); });

      await taskCompareLeft;
      await taskCompareRight;
      await taskCompareLeftRight;

      totalFiles = 0;
      fileScanIndex = 0;
      totalDirectories = 0;
      directoryScanIndex = 0;
      crossCompareIndex = 0;
      totalCrossCompare = 0;
    }

    private object debugLock = new object();

    private void FindDuplicates(CompareSets set)
    {
      // DEBUG
      lock (debugLock)
      {
        var comparable = (from file in set.AllFiles
                          where file.name == "GameSessionActivator.cs"
                          select file).ToArray();

        if (comparable.Length == 2)
        {
          CrossCompare a = new CrossCompare(comparable[0], comparable[1], 1f);
          CrossCompare b = new CrossCompare(comparable[1], comparable[0], 1f);

          var equals = a.Equals(b);
          var compareTo = a.CompareTo(b);
          var codeA = a.GetHashCode();
          var codeB = b.GetHashCode();
          var fCompare = new bool[] { a.left == b.left, a.right == b.left, a.left == b.right, a.right == b.right };

          SortedSet<CrossCompare> sortedA = new SortedSet<CrossCompare>();
          sortedA.Add(a);
          sortedA.Add(b);

          foreach (var sorted in sortedA)
          {
            Console.WriteLine(sorted.ToString());
          }
        }
      }

      foreach (var leftFile in set.AllFiles)
      {
        foreach (var rightFile in set.AllFiles)
        {
          if (leftFile.Equals(rightFile)) continue;
          
          if (leftFile.hash.Equals(rightFile.hash))
            lock (set.Duplicates) { set.Duplicates.Add(new CrossCompare(leftFile, rightFile, 1f)); }
        }
      }

      lock (debugLock)
      {

      }
    }

    private void CrossCompare(CompareSets leftSet, CompareSets rightSet)
    {
      // find all duplicates on set
      if (leftSet == rightSet)
        FindDuplicates(leftSet);

      foreach (var leftFile in leftSet.AllFiles)
      {
        foreach (var rightFile in rightSet.AllFiles)
        {
          // ignore same file
          if (leftFile.Equals(rightFile))
            continue;

          // ignore equals (moved detection can show them elsewhere
          if (leftFile.hash.Equals(rightFile.hash))
            continue;

          // find similar
          var cc = new CrossCompare(leftFile, rightFile);

          // add similar files
          lock (leftSet) { leftSet.CrossCompareList.Add(cc); }
        }
      }
    }

    private CompareSets ParseFiles(CompareSets set)
    {
      foreach (var file in set.FileList)
      {
        // Could create a task
        var data = FileCompareData.FromFile(file, set.Path);
        lock (set)
        {
          set.AllFiles.Add(data);
          set.Files[data.name] = data;
          set.Filenames.Add(data.name);
        }

        ++fileScanIndex;
      }

      return set;
    }

    private List<string> ListFiles(string path)
    {
      var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
      totalDirectories += directories.Length;

      List<string> files = new List<string>();
      foreach (var dir in directories)
      {
        GetFiles(dir, files);
        ++directoryScanIndex;
      }

      return files;
    }

    private void GetFiles(string path, List<string> files)
    {
      string[] filters = GetFilters();
      foreach (var filter in filters)
      {
        var filePaths = Directory.GetFiles(path, filter);
        files.AddRange(filePaths);
      }
    }

    private string[] GetFilters()
    {
      return FileFilter.Split(',');
    }

    private int ProgressCount()
    {
      if (totalFiles != 0)
      {
        TaskName = "Parsing Files";
        return (int)((float)fileScanIndex / totalFiles * 100);
      }
      else if (totalDirectories != 0)
      {
        TaskName = "Scanning Directories";
        return (int)((float)directoryScanIndex / totalDirectories * 100);
      }
      else if (totalCrossCompare != 0)
      {
        TaskName = "Cross Compare Files";
        return (int)((float)crossCompareIndex / totalCrossCompare);
      }
      else
      {
        TaskName = "Idle";
        return 100;
      }
    }
  }
}
