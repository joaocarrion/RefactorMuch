using System;

namespace RefactorMuch.Parse
{
  public partial class FileCompareData
  {
    private class LineCompare : IComparable<LineCompare>
    {
      public long hash = 0;
      public string line;
      public string compareLine;
      public bool EmptyLine => compareLine.Length == 0;

      public LineCompare(string line)
      {
        compareLine = lineSet.Execute(line);
        if (compareLine.Length == 0)
          hash = -1;
        else
          foreach (var c in compareLine)
            hash += c;

        this.line = line;
      }

      public int CompareTo(LineCompare right)
      {
        if (hash == right.hash)
          return string.Compare(compareLine, right.compareLine);
        else
          return hash < right.hash ? -1 : 1;
      }

      public override string ToString()
      {
        return $"{hash}: {line}";
      }
    }
  }
}

