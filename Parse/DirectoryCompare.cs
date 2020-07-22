using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

namespace RefactorMuch.Parse
{
  public class FileCompareData
  {
    public bool parsed = false;
    public string name;
    public string path;
    public byte[] hash;
    public string localPath;
    public DateTime lastChange;
    internal string absolutePath;
    public List<byte[]> lineHash = new List<byte[]>();
  }

  public class DirectoryCompare
  {
    private int totalFiles = 0;
    private int fileScanIndex = 0;
    private int totalDirectories = 0;
    private int directoryScanIndex = 0;
    private long crossCompareIndex = 0;
    private long totalCrossCompare = 0;

    private class CompareSets
    {
      public string Path;
      public CrossList CrossList;
      public List<string> FileList;
      public HashSet<string> Filenames;
      public HashSet<CrossCompare> Duplicates;
      public Dictionary<string, FileCompareData> Files;
      public List<FileCompareData> AllFiles = new List<FileCompareData>();
    }

    public string LeftPath { get; }
    public string RightPath { get; }
    public HashSet<string> Filenames { get; private set; } = new HashSet<string>();
    public Dictionary<string, FileCompareData> LeftFiles { get; private set; } = new Dictionary<string, FileCompareData>();
    public Dictionary<string, FileCompareData> RightFiles { get; private set; } = new Dictionary<string, FileCompareData>();
    public HashSet<CrossCompare> DuplicateLeft { get; private set; } = new HashSet<CrossCompare>();
    public HashSet<CrossCompare> DuplicateRight { get; private set; } = new HashSet<CrossCompare>();
    public CrossList CrossList { get; private set; } = new CrossList(0.65f); // minimum 65%

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
          CrossList = CrossList,
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

        CompareSets left = new CompareSets
        {
          FileList = files,
          CrossList = CrossList,
          Duplicates = DuplicateRight,
          Filenames = Filenames,
          Files = LeftFiles,
          Path = LeftPath
        };

        return ParseFiles(left);
      });

      var leftSet = await taskLeft;
      var rightSet = await taskRight;

      totalCrossCompare = leftSet.Files.Count * leftSet.Files.Count + rightSet.Files.Count * rightSet.Files.Count + leftSet.Files.Count * rightSet.Files.Count;
      var taskCompareLeft = Task.Run(() => { CrossCompare(leftSet, leftSet); });
      var taskCompareRight = Task.Run(() => { CrossCompare(rightSet, rightSet); });
      var taskCompareLeftRight = Task.Run(() => { CrossCompare(leftSet, rightSet); });

      // TODO: create setting to enable/disable cross compare and line compare
      // cross compare duplicates on left
      await taskCompareLeft;
      // cross compare duplicates on right
      await taskCompareRight;
      // cross compare left/right
      await taskCompareLeftRight;

      totalFiles = 0;
      fileScanIndex = 0;
      totalDirectories = 0;
      directoryScanIndex = 0;
      crossCompareIndex = 0;
      totalCrossCompare = 0;
    }

    private void CrossCompare(CompareSets leftSet, CompareSets rightSet)
    {
      foreach (var leftFile in leftSet.AllFiles)
      {
        foreach (var rightFile in rightSet.AllFiles)
        {
          if (leftFile.hash.SequenceEqual(rightFile.hash))
            continue;

          float percentEqual = CrossCompareFiles(leftFile, rightFile);
          lock(leftSet) {
            leftSet.CrossList.Add(new CrossCompare
            {
               left = leftFile,
               right = rightFile,
               similarity = percentEqual
            });
          }
        }
      }
    }

    private float CrossCompareFiles(FileCompareData leftFile, FileCompareData rightFile)
    {
      int matchingLines = 0;
      int lines = Math.Max(leftFile.lineHash.Count, rightFile.lineHash.Count);

      foreach (var lLine in leftFile.lineHash)
        foreach (var rLine in rightFile.lineHash)
          if (lLine.SequenceEqual(rLine))
            ++matchingLines;

      return (float)matchingLines / (float)lines;
    }

    private CompareSets ParseFiles(CompareSets set)
    {
      foreach (var file in set.FileList)
      {
        var data = GetCompareData(file, set.Path);
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

    private Regex regFullcompare = new Regex("[ \\n\\r;\\{\\}]");
    private Regex regLineCompare = new Regex("[ \\r;\\{\\}]");

    private FileCompareData GetCompareData(string file, string sourcePath)
    {
      var name = Path.GetFileName(file);
      var path = file.Replace(name, "");
      var local = path.Replace(sourcePath, "");

      var fd = new FileCompareData()
      {
        hash = null,
        lastChange = File.GetLastWriteTime(file),
        name = name,
        absolutePath = file,
        path = path,
        localPath = local
      };

      try
      {
        using (var bs = new BufferedStream(File.OpenRead(file)))
        {
          long size = bs.Seek(0, SeekOrigin.End);
          bs.Seek(0, SeekOrigin.Begin);

          if (size > int.MaxValue)
            return fd; // no comparison available, file too big

          byte[] data = new byte[size + 1];
          int read = bs.Read(data, 0, (int)size);

          string str = Encoding.UTF8.GetString(data, 0, read);
          data = null; // take a hint :D

          var fullCompare = regFullcompare.Replace(str, "");
          var fullData = Encoding.UTF8.GetBytes(fullCompare);

          var lineCompare = regLineCompare.Replace(str, "").Split('\n');
          var lineData = new byte[lineCompare.Length][];
          for (int i = 0; i < lineCompare.Length; i++)
            lineData[i] = Encoding.UTF8.GetBytes(lineCompare[i]);

          using (var md5Hash = MD5.Create())
          {
            fd.hash = md5Hash.ComputeHash(fullData);
            foreach (var line in lineData)
              fd.lineHash.Add(md5Hash.ComputeHash(line));
          }
          fd.parsed = true;
        }
      }
      catch (IOException) { }

      return fd;
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
