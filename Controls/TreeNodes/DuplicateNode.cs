using RefactorMuch.Parse;

namespace RefactorMuch.Controls.TreeNodes
{
  public class DuplicateNode : CrossCompareNode
  {
    public DuplicateNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      Text = $"{compare.left.name,-32}: {compare.right.localPath} ==> {compare.left.localPath}";
    }
  }
}
