﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RefactorMuch.Parse
{
  public partial class FileCompareData : IComparable<FileCompareData>
  {
    public bool parsed = false;
    public string name;
    public string extension;
    public string path;
    public string hash;
    public string localPath;
    public DateTime lastChange;
    public string absolutePath;
    public string parseError;
    public string basePath;
    public bool empty = false;

    public int LineCount => lineHash.Length;

    public IEnumerable<string> CodeLines => codeLines;

    private string[] codeLines;
    private LineCompare[] lineHash = new LineCompare[0];

    private static object setLock = new object();
    private static Dictionary<string, RuleSet> ruleSets = null;
    private static RuleSet fullSet = null;
    private static RuleSet lineSet = null;

    protected FileCompareData() { }

    public static void GetRules()
    {
      // initialize rules
      lock (setLock)
      {
        if (ruleSets == null)
          ruleSets = RuleSet.FromConfiguration();

        fullSet = ruleSets["preFileCompare"];
        lineSet = ruleSets["preLineCompare"];
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
          var fullCompare = Encoding.UTF8.GetBytes(fullSet.Execute(fileString));
          data.codeLines = fileString.Split('\n');
          if (fullCompare.Length > 0)
          {
            var lineCompare = lineSet.Execute(fileString).Split('\n');
            using (var md5Hash = MD5.Create())
            {
              data.hash = BitConverter.ToString(md5Hash.ComputeHash(fullCompare));
              data.lineHash = (from line in lineCompare
                               where line.Length > 0
                               select new LineCompare(line)).ToArray();
            }
          }
          else data.empty = true;
          data.parsed = true;
        }
      }
      catch (IOException exc) { data.parseError = exc.Message; }

      return data;
    }

    public override string ToString() => $"File: {name}, LocalPath: {localPath}, Path: {absolutePath}, Lines: {LineCount}";
    public override int GetHashCode() => absolutePath.GetHashCode();
    public override bool Equals(object obj) => string.Compare(absolutePath, ((FileCompareData)obj).absolutePath, true) == 0;

    // comparison to enter sorted lists/sets
    public int CompareTo(FileCompareData other) => absolutePath.CompareTo(other.absolutePath);
    public FileCompareData SmallerLocalPath(FileCompareData other) => localPath.Length < other.localPath.Length ? this : other;
    public bool DifferentLocalFile(FileCompareData right) => !name.Equals(right.name) || !hash.Equals(right.hash) || !localPath.Equals(right.localPath);

    private class LineComparerWithHash : IComparer<string>
    {
      public int Compare(string x, string y)
      {
        if (x.GetHashCode() == y.GetHashCode())
          return string.Compare(x, y);
        else return string.Compare(x, y);
      }
    }

    public int CrossCompareFiles(FileCompareData right)
    {
      if (empty || right.empty)
        return 0;

      if (string.Compare(extension, right.extension, true) != 0)
        return 0;

      SortedSet<LineCompare> leftHashes = new SortedSet<LineCompare>();
      foreach (var line in lineHash)
        if (!line.EmptyLine)
          leftHashes.Add(line);

      int equals = 0;
      foreach (var line in right.lineHash)
        if (!line.EmptyLine)
          if (leftHashes.Contains(line))
            ++equals;

      return equals * 100 / Math.Max(lineHash.Length, right.lineHash.Length);
    }

  }
}
