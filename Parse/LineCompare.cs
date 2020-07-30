using System;

namespace RefactorMuch.Parse
{
  public partial class FileCompareData
  {
    private class LineCompare : IComparable<LineCompare>
    {
      public long hash = 0;
      public string line;
      public bool EmptyLine => line.Length == 0;

      public LineCompare(string line)
      {
        this.line = line;
        if (this.line.Length == 0)
          hash = -1;
        else
          foreach (var c in this.line)
            hash += c;
      }

      public int CompareTo(LineCompare right)
      {
        if (hash == right.hash)
          return string.Compare(line, right.line);
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

