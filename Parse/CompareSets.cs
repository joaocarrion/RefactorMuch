using System.Collections.Generic;

namespace RefactorMuch.Parse
{
  public partial class DirectoryCompare
  {
    public class CompareSets
    {
      public string Path;
      public CrossCompareList CrossCompareList;
      public List<string> FileList;
      public HashSet<string> Filenames;
      public HashSet<CrossCompare> Duplicates;
      public Dictionary<string, FileCompareData> Files;
      public List<FileCompareData> AllFiles = new List<FileCompareData>();

      public override string ToString() => $"Cross: {CrossCompareList.Count}, Duplicates: {Duplicates.Count}, Path: {Path}";
    }
  }
}
