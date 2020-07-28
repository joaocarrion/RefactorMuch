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
    public CrossCompareSet RefactoredLeft { get; private set; } = new CrossCompareSet(0.7f);
    public CrossCompareSet RefactoredRight { get; private set; } = new CrossCompareSet(0.7f);
    public CrossCompareSet MovedSet { get; private set; } = new CrossCompareSet(1f);
    public CrossCompareSet ChangedSet { get; private set; } = new CrossCompareSet(0f, 1f);
    public CrossCompareSet RenamedSet { get; private set; } = new CrossCompareSet(1f);
    public CrossCompareSet CrossSet { get; private set; } = new CrossCompareSet(0.51f);
    public CrossCompareSet UnchangedFileSet { get; private set; } = new CrossCompareSet(1f);

    // TODO: In the future, semantic comparison would be nice

    public int Progress => ProgressCount();

    public string TaskName { get; private set; } = "Idle";

    public string FileFilter { get; }

    public DirectoryCompare(string left, string right, string fileFilter)
    {
      LeftPath = left;
      RightPath = right;
      FileFilter = fileFilter;
      ThreadPool.SetMaxThreads(7, 3);
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
        Refactored = RefactoredLeft
      };

      CompareSets right = new CompareSets
      {
        FileList = null,
        Path = RightPath,
        Files = RightFiles,
        Duplicates = DuplicateRight,
        Refactored = RefactoredRight
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

    private Task AddToSet(CrossCompareSet set, FileCompareData left, FileCompareData right, float similarity = -1f)
    {
      return Task.Run(() =>
      {
        var cc = similarity == -1f ? new CrossCompare(left, right) : new CrossCompare(left, right, similarity);
        lock (set) set.Add(cc);
      });
    }

    private void CrossCompareSelf(CompareSets set)
    {
      var tasks = new List<Task>();
      foreach (var left in set.AllFiles)
        foreach (var right in set.AllFiles)
          if (left != right && !left.Equals(right))
            if (left.name == right.name)
              // same name
              if (left.hash == right.hash)
                // local folder must be different
                tasks.Add(AddToSet(set.Duplicates, left, right, 1f));
              else
                // local folder must be different, however, the file has the same name... duplicate?
                tasks.Add(AddToSet(CrossSet, left, right));
            else
              // cross compare all files for similatiries
              tasks.Add(AddToSet(set.Refactored, left, right));

      Task.WaitAll(tasks.ToArray());
    }

    private void CrossCompare(CompareSets leftSet, CompareSets rightSet)
    {
      var tasks = new List<Task>();
      foreach (var left in leftSet.AllFiles)
        foreach (var right in rightSet.AllFiles)
          if (left.name == right.name)
            // same name
            if (left.localPath == right.localPath)
              // same path
              if (left.hash == right.hash)
                // exact match
                tasks.Add(AddToSet(UnchangedFileSet, left, right, 1f));
              else
                // matching names and folders
                tasks.Add(AddToSet(ChangedSet, left, right));
            // same name, different path
            else if (left.hash == right.hash)
              // moved
              tasks.Add(AddToSet(MovedSet, left, right, 1f));
            else
              // moved and changed (probably)
              tasks.Add(AddToSet(ChangedSet, left, right));
          else if (left.hash == right.hash)
            // same file, different names
            if (left.localPath == right.localPath)
              // renamed
              tasks.Add(AddToSet(RenamedSet, left, right, 1f));
            else
              // moved
              tasks.Add(AddToSet(MovedSet, left, right, 1f));
          else
            // different names, different files, check for refactor
            tasks.Add(AddToSet(CrossSet, left, right));

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
