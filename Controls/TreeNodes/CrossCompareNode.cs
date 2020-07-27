using RefactorMuch.Parse;
using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public abstract class CrossCompareNode: TreeNode
  {
    protected CrossCompare compare;

    public CrossCompareNode(CrossCompare compare, int imageIndex)
    {
      this.compare = compare;
      ImageIndex = imageIndex;
      SelectedImageIndex = imageIndex;
      ContextMenuStrip = GetMenu();
    }

    protected abstract ContextMenuStrip GetMenu();
  }
}
