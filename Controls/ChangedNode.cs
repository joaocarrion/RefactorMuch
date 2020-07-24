using RefactorMuch.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public class ChangedNode :TreeNode
  {
    public ChangedNode(FileCompareData left, FileCompareData right, int imageIndex)
    {
      ImageIndex = imageIndex;
      SelectedImageIndex = imageIndex;
      Text = $"{left.name} doesn't match ({left.localPath} => {right.localPath})";
    }
  }
}
