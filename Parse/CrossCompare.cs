using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace RefactorMuch.Parse
{
  public class CrossCompare : IComparable<CrossCompare>
  {
    public float similarity;
    public FileCompareData left;
    public FileCompareData right;

    private string compareString;

    public CrossCompare(FileCompareData left, FileCompareData right)
    {
      var lData = left.absolutePath.Contains(DirectoryBrowse.LeftPath) ? left : right;
      var rData = left.absolutePath.Contains(DirectoryBrowse.LeftPath) ? right : left;

      this.left = lData;
      this.right = rData;

      CreateCompareString();
      CrossCompareFiles();
    }

    public CrossCompare(FileCompareData left, FileCompareData right, float similarity)
    {
      var lData = left.absolutePath.Contains(DirectoryBrowse.LeftPath) ? left : right;
      var rData = left.absolutePath.Contains(DirectoryBrowse.LeftPath) ? right : left;

      this.left = lData;
      this.right = rData;
      this.similarity = similarity;

      CreateCompareString();
    }

    private void CrossCompareFiles()
    {
      SortedSet<string> leftHashes = new SortedSet<string>();
      foreach (var line in left.lineHash)
        leftHashes.Add(line);

      int equals = 0;
      foreach (var line in right.lineHash)
        if (leftHashes.Contains(line))
          ++equals;

      similarity = (float)equals / (float)Math.Max(left.lineHash.Count, right.lineHash.Count);
    }

    private void CreateCompareString()
    {
      var lp = left.absolutePath;
      var rp = right.absolutePath;

      // always the smallest path first in order to compare with other cross compare items
      bool leftFirst = lp.Length == rp.Length ? (string.Compare(lp, rp, true) == -1) : (lp.Length < rp.Length);
      compareString = leftFirst ? string.Concat(lp, rp) : string.Concat(rp, lp);
    }


    public int CompareTo(CrossCompare other)
    {
      var f = "SimpleController.cs";
      if ((left.name == f || right.name == f) && (other.left.name == f || other.right.name == f))
      {

      }

      return string.Compare(compareString, other.compareString, true);
    }
    public override bool Equals(object obj) => ((CrossCompare)obj).compareString.Equals(compareString);
    public override int GetHashCode() => compareString.GetHashCode();
    public override string ToString() => $"Similarity {Math.Round(similarity * 100, 0):0}%, Left: {left.name,-25}, Right: {right.name,-25}, Local Path: {left.localPath} ";
  }
}
