using System.Linq;
using System.Text;

namespace RefactorMuch.Parse
{
  public partial class DirectoryCompare
  {
    public class LineMatch
    {
      public int count;
      public byte[] hash;

      public override string ToString() => System.BitConverter.ToString(hash) + $": {count}";
      public override bool Equals(object obj) => hash.SequenceEqual((obj as LineMatch).hash);
      public override int GetHashCode() => hash.GetHashCode();
    }
  }
}
