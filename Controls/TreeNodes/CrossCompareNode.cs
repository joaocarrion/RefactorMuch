using RefactorMuch.Parse;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public class CrossCompareNode: TreeNode
  {
    protected CrossCompare compare;

    public CrossCompareNode(CrossCompare compare, int imageIndex)
    {
      this.compare = compare;
      ImageIndex = imageIndex;
      SelectedImageIndex = imageIndex;

      ContextMenuStrip = GetMenu();
      Text = this.compare.ToString();

      Nodes.Add(new FileDataNode(compare.left, imageIndex));
      Nodes.Add(new FileDataNode(compare.right, imageIndex));
    }

    protected virtual ContextMenuStrip GetMenu() { return null; }
  }
}
