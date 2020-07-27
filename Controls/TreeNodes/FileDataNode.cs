using RefactorMuch.Parse;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public class FileDataNode : TreeNode
  {
    private FileCompareData file;
    private class MenuStrip : ContextMenuStrip
    {
      public MenuStrip()
      {
        var menuItems = new ToolStripItem[]{
          Items.Add("Remove"),
          Items.Add("View...")
        };

        menuItems[0].Click += Remove;
        menuItems[1].Click += View;
      }

      private void Remove(object sender, System.EventArgs e) => ((FileDataNode)((TreeView)SourceControl).SelectedNode).RemoveFile();
      private void View(object sender, System.EventArgs e) => ((FileDataNode)((TreeView)SourceControl).SelectedNode).View();
    }

    private static object menuLock = new object();
    private static MenuStrip menuStrip = null;

    private static MenuStrip GetMenuStrip()
    {
      lock (menuLock)
        if (menuStrip == null)
          menuStrip = new MenuStrip();
      return menuStrip;
    }

    public FileDataNode(FileCompareData file, int imageIndex)
    {
      this.file = file;
      ImageIndex = imageIndex;
      SelectedImageIndex = imageIndex;

      Text = file.ToString();
      ContextMenuStrip = GetMenuStrip();
    }

    private void RemoveFile()
    {
      if (DialogHelper.RemoveFile(file))
        Parent.Remove();
    }

    private void View() => Tools.GetInstance().ToolDictionary[Tool.ToolType.View].Run(file.absolutePath);
  }
}
