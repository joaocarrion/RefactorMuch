using RefactorMuch.Parse;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public class MovedNode : TreeNode
  {
    public MovedNode(FileCompareData left, FileCompareData right, int imageIndex)
    {
      ImageIndex = imageIndex;
      SelectedImageIndex = imageIndex;

      Text = $"{left.name} moved ({left.localPath} => {right.localPath})";
    }
  }
}
