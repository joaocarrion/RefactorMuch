using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Policy;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace InteractiveMerge.Parse
{
  public class FileCompareData
  {
    public bool parsed = false;
    public string name;
    public string path;
    public byte[] hash;
    public string localPath;
    public DateTime lastChange;
  }

  public class DirectoryCompare
  {
    private string left;
    private string right;
    private string fileFilter;

    // filename => path
    private HashSet<string> filenames = new HashSet<string>();
    private Dictionary<string, FileCompareData> leftFiles = new Dictionary<string, FileCompareData>();
    private Dictionary<string, FileCompareData> rightFiles = new Dictionary<string, FileCompareData>();

    private int totalFiles = 0;
    private int fileScanIndex = 0;
    private int totalDirectories = 0;
    private int directoryScanIndex = 0;

    public int Progress {
      get {
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
        else
        {
          TaskName = "Idle";
          return 100;
        }
      }
    }

    public string TaskName { get; private set; } = "Idle";

    public HashSet<string> Filenames { get => filenames; }

    public Dictionary<string, FileCompareData> Left => leftFiles;
    public Dictionary<string, FileCompareData> Right => rightFiles;

    public DirectoryCompare(string left, string right, string fileFilter)
    {
      this.left = left;
      this.right = right;
      this.fileFilter = fileFilter;
    }

    public async Task Parse()
    {
      var taskLeft = Task.Run(() => ListFiles(left));
      var taskRight = Task.Run(() => ListFiles(right));

      var leftFiles = await taskLeft;
      var rightFiles = await taskRight;

      totalFiles = leftFiles.Count + rightFiles.Count;
      var fileTasks = new Task[] {
        Task.Run(() => ParseFiles(leftFiles, this.leftFiles, this.left)),
        Task.Run(() => ParseFiles(rightFiles, this.rightFiles, this.right))
      };
      foreach (var t in fileTasks) await t;

      totalFiles = 0;
      fileScanIndex = 0;
      totalDirectories = 0;
      directoryScanIndex = 0;
    }

    private void ParseFiles(List<string> files, Dictionary<string, FileCompareData> target, string sourcePath)
    {
      // detect file moves
      foreach (var file in files)
      {
        // TODO: Hash, Date
        var data = GetCompareData(file);
        data.localPath = data.path.Replace(sourcePath, "");

        // TODO: Ignoring right if there are two files with the same name
        target[data.name] = data;
        ++fileScanIndex;

        lock (filenames)
        {
          filenames.Add(data.name);
        }
      }
    }

    private Regex regex = new Regex("[ \\n\\r;\\{\\}]");

    private FileCompareData GetCompareData(string file)
    {
      var fd = new FileCompareData()
      {
        hash = null,
        lastChange = File.GetLastWriteTime(file),
        name = Path.GetFileName(file),
        path = file
      };

      try
      {
        using (var bs = new BufferedStream(File.OpenRead(file)))
        {
          byte[] data = new byte[512 * 1024];
          int read = bs.Read(data, 0, 512 * 1024);
          string str = System.Text.Encoding.UTF8.GetString(data, 0, read);
          // ignore white space
          var res = regex.Replace(str, "");

          var resultData = System.Text.Encoding.UTF8.GetBytes(res);
          using (var md5Hash = MD5.Create())
          {
            fd.hash = md5Hash.ComputeHash(resultData);
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
      return fileFilter.Split(',');
    }
  }
}
