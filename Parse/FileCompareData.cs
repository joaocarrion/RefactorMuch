using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RefactorMuch.Parse
{
  public class FileCompareData : IComparable<FileCompareData>
  {
    public bool parsed = false;
    public string name;
    public string extension;
    public string path;
    public string hash;
    public string localPath;
    public DateTime lastChange;
    public string absolutePath;
    public List<string> lineHash = new List<string>();
    public string parseError;
    public string basePath;

    private static object setLock = new object();
    private static Dictionary<string, RuleSet> ruleSets = null;
    private static RuleSet fullSet = null;
    private static RuleSet lineSet = null;
    private static RuleSet normalizeSet = null;

    protected FileCompareData()
    {
    }

    public static void GetRules()
    {
      // initialize rules
      lock (setLock)
      {
        if (ruleSets == null)
          ruleSets = RuleSet.FromConfiguration();

        fullSet = ruleSets["FileCompare"];
        lineSet = ruleSets["LineCompare"];
        if (ruleSets.ContainsKey("Normalize"))
          normalizeSet = ruleSets["Normalize"];
      }
    }

    public static FileCompareData FromFile(string file, string basePath, Regex[] filterExpressions = null)
    {
      GetRules();

      FileCompareData data = new FileCompareData();
      data.absolutePath = file;
      data.name = Path.GetFileName(file);
      data.extension = Path.GetExtension(file);
      data.path = Path.GetDirectoryName(file);
      data.localPath = data.path.Replace(basePath, "");
      data.lastChange = File.GetLastWriteTime(file);
      data.basePath = basePath;

      try
      {
        using (var sr = new StreamReader(File.OpenRead(file), true))
        {
          var fileString = sr.ReadToEnd();
          if (normalizeSet != null)
            fileString = normalizeSet.Execute(fileString);

          var fullCompare = Encoding.UTF8.GetBytes(fullSet.Execute(fileString));
          var lineCompare = (from line in lineSet.Execute(fileString).Split('\n')
                             where line.Length > 0
                             select Encoding.UTF8.GetBytes(line)).ToArray();

          using (var md5Hash = MD5.Create())
          {
            data.hash = BitConverter.ToString(md5Hash.ComputeHash(fullCompare));
            foreach (var line in lineCompare)
              data.lineHash.Add(BitConverter.ToString(md5Hash.ComputeHash(line)));
          }
          data.parsed = true;
        }
      }
      catch (IOException exc) { data.parseError = exc.Message; }

      return data;
    }

    public override string ToString() => $"File: {name}, LocalPath: {localPath}, Path: {absolutePath}, Lines: {lineHash.Count}";
    public override int GetHashCode() => absolutePath.GetHashCode();
    public override bool Equals(object obj) => string.Compare(absolutePath, ((FileCompareData)obj).absolutePath, true) == 0;

    // comparison to enter sorted lists/sets
    public int CompareTo(FileCompareData other) => absolutePath.CompareTo(other.absolutePath);
    public FileCompareData SmallerLocalPath(FileCompareData other) => localPath.Length < other.localPath.Length ? this : other;
    public bool DifferentLocalFile(FileCompareData right) => !name.Equals(right.name) || !hash.Equals(right.hash) || !localPath.Equals(right.localPath);
  }
}

