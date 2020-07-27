using RefactorMuch.Parse;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public class CrossCompareNode: TreeNode
  {
    protected CrossCompare compare;

    public CrossCompareNode(CrossCompare compare, int imageIndex)
    {
      ImageIndex = imageIndex;
      SelectedImageIndex = imageIndex;

      ContextMenuStrip = GetMenu();

      var left = compare.left.absolutePath.Contains(DirectoryBrowse.LeftPath) ? compare.left : compare.right;
      var right = compare.left.absolutePath.Contains(DirectoryBrowse.LeftPath) ? compare.right : compare.left;
      this.compare = new CrossCompare(left, right, compare.similarity);

      Text = this.compare.ToString();

      Nodes.Add(new FileDataNode(compare.left, imageIndex));
      Nodes.Add(new FileDataNode(compare.right, imageIndex));
    }

    protected virtual ContextMenuStrip GetMenu() { return null; }
  }
}
