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
    public CrossCompareSet DuplicateLeft { get; private set; } = new CrossCompareSet(100);
    public CrossCompareSet DuplicateRight { get; private set; } = new CrossCompareSet(100);
    public CrossCompareSet RefactoredLeft { get; private set; } = new CrossCompareSet(70);
    public CrossCompareSet RefactoredRight { get; private set; } = new CrossCompareSet(70);
    public CrossCompareSet MovedSet { get; private set; } = new CrossCompareSet(70);
    public CrossCompareSet ChangedSet { get; private set; } = new CrossCompareSet(0, 100);
    public CrossCompareSet RenamedSet { get; private set; } = new CrossCompareSet(100);
    public CrossCompareSet CrossSet { get; private set; } = new CrossCompareSet(51);
    public CrossCompareSet UnchangedFileSet { get; private set; } = new CrossCompareSet(100);
    public SortedSet<FileCompareData> EmptyFilesLeft { get; private set; } = new SortedSet<FileCompareData>();
    public SortedSet<FileCompareData> EmptyFilesRight { get; private set; } = new SortedSet<FileCompareData>();

    // TODO: In the future, semantic comparison would be nice

    public int Progress => ProgressCount();

    public string TaskName { get; private set; } = "Idle";

    public string []FilterExtensions { get; }

    public DirectoryCompare(string left, string right, string filterExtensions)
    {
      LeftPath = left;
      RightPath = right;
      FilterExtensions = filterExtensions.Split('|');
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
        Refactored = RefactoredLeft,
        EmptyFiles = EmptyFilesLeft
      };

      CompareSets right = new CompareSets
      {
        FileList = null,
        Path = RightPath,
        Files = RightFiles,
        Duplicates = DuplicateRight,
        Refactored = RefactoredRight,
        EmptyFiles = EmptyFilesRight
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

    private Task AddToSet(string name, CrossCompareSet set, FileCompareData left, FileCompareData right, int similarity = -1)
    {
      return Task.Run(() =>
      {
        var debugName = name;
        var cc = similarity == -1 ? new CrossCompare(left, right) : new CrossCompare(left, right, similarity);
        lock (set) set.Add(cc);
      });
    }

    private void CrossCompareSelf(CompareSets set)
    {
      var tasks = new List<Task>();
      foreach (var left in set.AllFiles)
      {
        if (left.lineHash.Count == 0)
          EmptyFilesLeft.Add(left);
        else
          foreach (var right in set.AllFiles)
          {
            if (right.lineHash.Count == 0) continue;
            if (left != right && !left.Equals(right))
              if (left.name == right.name)
                // same name
                if (left.hash == right.hash)
                  // local folder must be different
                  tasks.Add(AddToSet("self.duplicates", set.Duplicates, left, right, 100));
                else
                  // local folder must be different, however, the file has the same name... duplicate?
                  tasks.Add(AddToSet("self.cross", CrossSet, left, right));
              else if (left.hash == right.hash)
                // renamed?
                tasks.Add(AddToSet("self.renamed", RefactoredLeft, left, right, 100));
              else
                // cross compare all files for similatiries
                tasks.Add(AddToSet("self.refactor", set.Refactored, left, right));
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
          if (left.name == right.name)
            // same name
            if (left.localPath == right.localPath)
              // same path
              if (left.hash == right.hash)
                // exact match
                tasks.Add(AddToSet("other.unchanged", UnchangedFileSet, left, right, 100));
              else
                // matching names and folders
                tasks.Add(AddToSet("other.changed", ChangedSet, left, right));
            // same name, different path
            else if (left.hash == right.hash)
              // moved
              tasks.Add(AddToSet("other.moved", MovedSet, left, right, 100));
            else
              // moved and changed (probably)
              tasks.Add(AddToSet("other.changed2", ChangedSet, left, right));
          else if (left.hash == right.hash)
            // same file, different names
            if (left.localPath == right.localPath)
              // renamed
              tasks.Add(AddToSet("other.renamed", RenamedSet, left, right, 100));
            else
              // moved
              tasks.Add(AddToSet("other.moved2", MovedSet, left, right, 100));
          else
            // different names, different files, check for refactor
            tasks.Add(AddToSet("other.cross2", CrossSet, left, right));
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
      foreach (var filter in FilterExtensions)
      {
        var filePaths = Directory.GetFiles(path, filter);
        files.AddRange(filePaths);
      }
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
