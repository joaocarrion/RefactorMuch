using RefactorMuch.Parse;
using System;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public class ChangedNode : CrossCompareNode
  {
    public ChangedNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      Text = $"{Math.Round(compare.similarity * 100f)}%: {compare.left.name}";
    }

    private static MenuStrip menuStrip = null;
    private static object menuLock = new object();

    protected override ContextMenuStrip GetMenu()
    {
      if (menuStrip == null)
        lock (menuLock)
          if (menuStrip == null)
          {
            var items = new string[] { "Diff..." };
            var actions = new Action<TreeView>[] { (view) => { ((ChangedNode)view.SelectedNode).Diff(); } };
            menuStrip = new MenuStrip(items, actions);
          }

      return menuStrip;
    }

    private void Diff() => Tools.GetInstance().ToolDictionary[Tool.ToolType.Diff].Run(compare.left.absolutePath, compare.right.absolutePath);
  }
}
