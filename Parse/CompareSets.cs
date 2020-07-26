using System.Collections.Generic;

namespace RefactorMuch.Parse
{
  public partial class DirectoryCompare
  {
    public class CompareSets
    {
      public string Path;
      public CrossCompareSet CrossCompareList;
      public List<string> FileList;
      public SortedSet<string> Filenames;
      public CrossCompareSet Duplicates;
      public Dictionary<string, FileCompareData> Files;
      public SortedSet<FileCompareData> AllFiles = new SortedSet<FileCompareData>();

      public override string ToString() => $"Cross: {CrossCompareList.Count}, Duplicates: {Duplicates.Count}, Path: {Path}";
    }
  }
}
