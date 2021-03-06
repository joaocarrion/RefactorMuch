﻿using System.Collections.Generic;

namespace RefactorMuch.Parse
{
  public partial class DirectoryCompare
  {
    public class CompareSets
    {
      public string Path;
      public List<string> FileList;
      public CrossCompareSet Duplicates;
      public CrossCompareSet Refactored;
      public Dictionary<string, FileCompareData> Files;
      public SortedSet<FileCompareData> AllFiles = new SortedSet<FileCompareData>();
      public SortedSet<FileCompareData> EmptyFiles;
    }
  }
}
