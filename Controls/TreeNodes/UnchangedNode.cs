using RefactorMuch.Parse;

namespace RefactorMuch.Controls.TreeNodes
{
  public class UnchangedNode : CrossCompareNode
  {
    public UnchangedNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      Text = $"{compare.left.name} => {compare.left.localPath}";
    }
  }
}
