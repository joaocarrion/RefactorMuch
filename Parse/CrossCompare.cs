using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RefactorMuch.Parse
{
  public class CrossCompare
  {
    public float similarity;
    public FileCompareData left;
    public FileCompareData right;

    public override bool Equals(object obj)
    {
      var cast = (CrossCompare)obj;
      if (cast == null) return false;

      return (cast.left == left && cast.right == right) || (cast.right == left && cast.left == right);
    }

    public override int GetHashCode()
    {
      return left.GetHashCode() | right.GetHashCode();
    }

    public override string ToString()
    {
      return $"Left: {left.name}, Right: {right.name}, Similarity {Math.Round(similarity * 100, 0):0}";
    }
  }
}
