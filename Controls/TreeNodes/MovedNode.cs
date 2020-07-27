using RefactorMuch.Parse;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public partial class MovedNode : CrossCompareNode
  {
    public MovedNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      Text = $"{compare.left.name} moved ({compare.left.localPath} => {compare.right.localPath})";
      ContextMenuStrip = new ContextMenuStrip();
      ContextMenuStrip.Items.Add($"Move left: {compare.left.localPath}", null, MoveLeft);
      ContextMenuStrip.Items.Add($"Move right: {compare.right.localPath}", null, MoveRight);
    }

    private void Move(bool isLeft)
    {
      FileCompareData to = isLeft ? compare.left : compare.right;
      FileCompareData from = isLeft ? compare.right : compare.left;

      var sourceDir = from.path;
      var destination = $"{from.basePath}{to.localPath}\\{from.name}";

      try
      {
        if (DialogHelper.QuestionDialog($"Are you sure you want to move {from.name} to {destination}") == DialogResult.Yes)
        {
          if (!Directory.Exists(Path.GetDirectoryName(destination)))
            Directory.CreateDirectory(Path.GetDirectoryName(destination));

          File.Move(from.absolutePath, destination);
          if (Directory.GetFiles(sourceDir).Length == 0 && Directory.GetDirectories(sourceDir).Length == 0)
          {
            if (DialogHelper.QuestionDialog($"Directory {sourceDir} is Empty. Remove directory?") == DialogResult.Yes)
              Directory.Delete(sourceDir);
          }

          // remove node from view
          if (Parent.Nodes.Count == 1)
            Parent.Remove();
          else
            Remove();
        }
      }
      catch (Exception exc)
      {
        DialogHelper.ErrorDialog($"Error moving file {from.absolutePath}: {exc.Message}");
      }
    }

    private void MoveLeft(object sender, EventArgs e) => Move(true);
    private void MoveRight(object sender, EventArgs e) => Move(false);
  }
}
