using RefactorMuch.Parse;
using System;
using System.IO;
using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public partial class MovedNode : CrossCompareNode
  {
    public MovedNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      if (compare.left.localPath.Equals(compare.right.localPath))
        Text = $"{compare.left.name} renamed {compare.right.name} => {compare.left.localPath}";
      else
        Text = $"{compare.left.name} moved ({compare.left.absolutePath} => {compare.right.absolutePath})";
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
    private void View() => Tools.GetInstance().ToolDictionary[Tool.ToolType.View].Run(compare.left.absolutePath);
  }
}
