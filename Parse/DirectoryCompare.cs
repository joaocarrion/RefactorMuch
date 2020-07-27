using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
    public CrossCompareSet MovedSet { get; private set; } = new CrossCompareSet(1f);
    public CrossCompareSet ChangedSet { get; private set; } = new CrossCompareSet(1f);

    // TODO: In the future, semantic comparison would be nice
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

      CompareSets left = new CompareSets
      {
        Path = LeftPath,
        FileList = null,
        Files = LeftFiles,
        Duplicates = DuplicateLeft,
      };

      CompareSets right = new CompareSets
      {
        FileList = null,
        Path = RightPath,
        Files = RightFiles,
        Duplicates = DuplicateRight,
      };

      // Parse files left and right
      var tasks = new Task[] { Task.Run(() => ParseFiles(left)), Task.Run(() => ParseFiles(right)) };
      await Task.WhenAll(tasks);

      // compare left with left, right with right and then left with right
      totalCrossCompare = left.Files.Count * left.Files.Count + right.Files.Count * right.Files.Count + left.Files.Count * right.Files.Count;

      var crossCompareTasks = new Task[]
      {
        Task.Run(() => { CrossCompareSelf(left); }),
        Task.Run(() => { CrossCompareSelf(right); }),
        Task.Run(() => { CrossCompare(left, right); })
      };

      // wait until done
      await Task.WhenAll(crossCompareTasks);

      totalFiles = 0;
      fileScanIndex = 0;
      totalDirectories = 0;
      directoryScanIndex = 0;
      crossCompareIndex = 0;
      totalCrossCompare = 0;
    }

    private void CrossCompareSelf(CompareSets set)
    {
      var tasks = new List<Task>();
      foreach (var left in set.AllFiles)
      {
        foreach (var right in set.AllFiles)
        {
          // same file on both sets
          if (left.Equals(right)) continue;
          // same file contents
          else if (left.hash.Equals(right.hash))
            // renamed or duplicate
            // if (!left.localPath.Equals(right.localPath) || !left.name.Equals(right.name)) // one will always be different
            lock (set.Duplicates) { set.Duplicates.Add(new CrossCompare(left, right, 1f)); }
          else tasks.Add(Task.Run(() =>
          {
            // may be an inner refactory
            var cc = new CrossCompare(left, right);
            lock (CrossSet) CrossSet.Add(cc);
          }));
        }
      }

      Task.WaitAll(tasks.ToArray());
    }

    private void CrossCompare(CompareSets leftSet, CompareSets rightSet)
    {
      var tasks = new List<Task>();
      foreach (var left in leftSet.AllFiles)
      {
        foreach (var right in rightSet.AllFiles)
        {
          if (left.name.Equals(right.name)) // same name
          {
            if (left.hash.Equals(right.hash)) // same contents
            {
              if (!left.localPath.Equals(right.localPath)) // different path => moved
                // same file different local paths
                lock (MovedSet) MovedSet.Add(new CrossCompare(left, right, 1f));
            }
            else // different contents... compare
            {
              tasks.Add(Task.Run(() =>
              {
                var changedCompare = new CrossCompare(left, right);
                lock (ChangedSet) ChangedSet.Add(changedCompare);
              }));
            }
          }
          else if (left.hash.Equals(right.hash)) // different name, same content, renamed
            lock (MovedSet) MovedSet.Add(new CrossCompare(left, right, 1f));
          // Different name/contents, check if it may be a refactor
          else tasks.Add(Task.Run(() =>
          {
            var cc = new CrossCompare(left, right);
            lock (CrossSet) CrossSet.Add(cc);
          }));
        }
      }

      Task.WaitAll(tasks.ToArray());
    }

    private void ParseFiles(CompareSets set)
    {
      // scan directories
      var files = ListFiles(set.Path);
      
      // set files
      set.FileList = files;
      totalFiles += files.Count;

      int wt, cpt;
      ThreadPool.GetMaxThreads(out wt, out cpt);

      // files file comparision
      var tasks = new List<Task>();
      foreach (var file in set.FileList)
        tasks.Add(Task.Run(() =>
        {
          var data = FileCompareData.FromFile(file, set.Path);
          lock (set)
          {
            set.AllFiles.Add(data);
            set.Files[data.absolutePath] = data;
            Filenames.Add(data.name);
          }

          ++fileScanIndex;
        }));

      Task.WaitAll(tasks.ToArray());
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
