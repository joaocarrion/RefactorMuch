using System;
using System.Collections.Generic;
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

    public override string ToString()
    {
      return $"File: {name}, Path: ${absolutePath}, Lines: {lineHash.Count}";
    }

    public override int GetHashCode()
    {
      return absolutePath.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      var cast = (FileCompareData)obj;
      if (cast != null)
        return absolutePath == cast.absolutePath;
      else
        return false;
    }
  }
}
