using RefactorMuch.Parse;
using System.Drawing;
using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public class DuplicateNode : TreeNode
  {
    private class MenuStrip : ContextMenuStrip
    {
      public MenuStrip() : base()
      {
        var menuItems = new ToolStripItem[]{
          Items.Add("Remove left"),
          Items.Add("Remove right"),
          Items.Add("View...")
        };

        menuItems[0].Click += RemoveLeft;
        menuItems[1].Click += RemoveRight;
        menuItems[2].Click += View;
      }

      private void View(object sender, System.EventArgs e)
      {
        TreeView tv = (TreeView)SourceControl;
        if (tv.SelectedNode is DuplicateNode)
          (tv.SelectedNode as DuplicateNode).View();
      }

      private void RemoveRight(object sender, System.EventArgs e) => (sender as DuplicateNode).RemoveRight();
      private void RemoveLeft(object sender, System.EventArgs e) => (sender as DuplicateNode).RemoveLeft();
    }

    private static MenuStrip menuStrip = null;

    public DuplicateNode(FileCompareData left, FileCompareData right, int imageIndex)
    {
      Text = $"{left.name,-32} ==> {right.localPath}";
      if (menuStrip == null)
        menuStrip = new MenuStrip();
      ContextMenuStrip = menuStrip;
      ImageIndex = imageIndex;
      SelectedImageIndex = imageIndex;
    }

    public void View()
    {

    }

    public void RemoveLeft()
    {

    }

    public void RemoveRight() { }
  }
}
