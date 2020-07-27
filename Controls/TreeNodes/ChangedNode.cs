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

    private static ContextMenuStrip menuStrip = null;
    private static object menuLock = new object();

    protected override ContextMenuStrip GetMenu()
    {
      if (menuStrip == null)
        lock (menuLock)
          if (menuStrip == null)
          {
            menuStrip = new ContextMenuStrip();
            menuStrip.Items.Add("Diff...");
            menuStrip.Items[0].Click += (sender, e) => { ((ChangedNode)TreeView.SelectedNode).Diff(); };
          }

      return menuStrip;
    }

    private void Diff() => Tools.GetInstance().ToolDictionary[Tool.ToolType.Diff].Run(compare.left.absolutePath, compare.right.absolutePath);
  }
}
