using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public partial class DuplicateNode
  {
    private static object menuLock = new object();
    private static MenuStrip menuStrip = null;

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

      private void View(object sender, System.EventArgs e) => ((DuplicateNode)((TreeView)SourceControl).SelectedNode).View();
      private void RemoveRight(object sender, System.EventArgs e) => ((DuplicateNode)((TreeView)SourceControl).SelectedNode).RemoveLeft();
      private void RemoveLeft(object sender, System.EventArgs e) => ((DuplicateNode)((TreeView)SourceControl).SelectedNode).RemoveRight();
    }

    protected override ContextMenuStrip GetMenu()
    {
      lock (menuLock) { if (menuStrip == null) menuStrip = new MenuStrip(); }
      return menuStrip;
    }
  }

  public partial class ChangedNode
  {
    private static object menuLock = new object();
    private static MenuStrip menuStrip = null;

    private class MenuStrip : ContextMenuStrip
    {
      public MenuStrip() : base()
      {
        var menuItems = new ToolStripItem[]{
          Items.Add("Remove left"),
          Items.Add("Remove right"),
          Items.Add("Diff...")
        };

        menuItems[0].Click += RemoveLeft;
        menuItems[1].Click += RemoveRight;
        menuItems[2].Click += Diff;
      }

      private void Diff(object sender, System.EventArgs e) => ((ChangedNode)((TreeView)SourceControl).SelectedNode).Diff();
      private void RemoveRight(object sender, System.EventArgs e) => ((ChangedNode)((TreeView)SourceControl).SelectedNode).RemoveLeft();
      private void RemoveLeft(object sender, System.EventArgs e) => ((ChangedNode)((TreeView)SourceControl).SelectedNode).RemoveRight();
    }
    protected override ContextMenuStrip GetMenu()
    {
      lock (menuLock) { if (menuStrip == null) menuStrip = new MenuStrip(); }
      return menuStrip;
    }

  }

  public partial class MovedNode
  {
    private static object menuLock = new object();
    private static MenuStrip menuStrip = null;

    private class MenuStrip : ContextMenuStrip
    {
      public MenuStrip() : base()
      {
        var menuItems = new ToolStripItem[]{
          Items.Add("Move left"),
          Items.Add("Move right"),
          Items.Add("View")
        };

        menuItems[0].Click += MoveLeft;
        menuItems[1].Click += MoveRight;
        menuItems[2].Click += View;
      }

      private void View(object sender, System.EventArgs e) => ((MovedNode)((TreeView)SourceControl).SelectedNode).View();
      private void MoveLeft(object sender, System.EventArgs e) => ((MovedNode)((TreeView)SourceControl).SelectedNode).MoveLeft();
      private void MoveRight(object sender, System.EventArgs e) => ((MovedNode)((TreeView)SourceControl).SelectedNode).MoveRight();
    }

    protected override ContextMenuStrip GetMenu()
    {
      lock (menuLock) { if (menuStrip == null) menuStrip = new MenuStrip(); }
      return menuStrip;
    }
  }

  public partial class SimilarNode
  {
    private static object menuLock = new object();
    private static MenuStrip menuStrip = null;

    private class MenuStrip : ContextMenuStrip
    {
      public MenuStrip() : base()
      {
        var menuItems = new ToolStripItem[]{
          Items.Add("Diff...")
      };

        menuItems[0].Click += Diff;
      }

      private void Diff(object sender, System.EventArgs e) => ((SimilarNode)((TreeView)SourceControl).SelectedNode).Diff();
    }
    protected override ContextMenuStrip GetMenu()
    {
      lock (menuLock) { if (menuStrip == null) menuStrip = new MenuStrip(); }
      return menuStrip;
    }
  }
}
