using RefactorMuch.Parse;
using System;
using System.IO;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public partial class MovedNode : CrossCompareNode
  {
    public MovedNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      if (compare.left.localPath.Equals(compare.right.localPath))
        Text = $"{compare.left.name} renamed {compare.right.name} => {compare.left.localPath}";
      else
        Text = $"{compare.left.name} moved ({compare.left.localPath} => {compare.right.localPath})";
    }

    private static MenuStrip menuStrip = null;
    private static object menuLock = new object();

    protected override ContextMenuStrip GetMenu()
    {
      if (menuStrip == null)
        lock (menuLock)
          if (menuStrip == null)
          {
            var items = new string[]
            {
              "Move Left",
              "Move Right",
            };

            var actions = new Action<TreeView>[]
            {
              (view) => { ((MovedNode)view.SelectedNode).MoveLeft(); },
              (view) => { ((MovedNode)view.SelectedNode).MoveRight(); },
            };
            menuStrip = new MenuStrip(items, actions);
          }

      return menuStrip;
    }

    private void Move(bool isLeft)
    {
      FileCompareData to = isLeft ? compare.left : compare.right;
      FileCompareData from = isLeft ? compare.right : compare.left;

      try
      {
        if (DialogHelper.QuestionDialog($"Are you sure you want to move {from.name} to {to.path}") == DialogResult.Yes)
        {
          File.Move(from.absolutePath, to.absolutePath);
          if (Directory.GetFiles(from.path).Length == 0 && Directory.GetDirectories(from.path).Length == 0)
          {
            if (DialogHelper.QuestionDialog($"Directory {from.path} is Empty. Remove directory?") == DialogResult.Yes)
              Directory.Delete(from.path);
          }

          // remove node from view
          Remove();
        }
      }
      catch (Exception exc)
      {
        DialogHelper.ErrorDialog($"Error deleting file {from.absolutePath}: {exc.Message}");
      }
    }

    private void MoveLeft() => Move(true);
    private void MoveRight() => Move(false);
  }
}
