using RefactorMuch.Parse;
using System;
using System.IO;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public class RenamedNode : CrossCompareNode
  {
    public RenamedNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      Text = $"{compare.left.name} renamed {compare.right.name} => {compare.left.localPath}";
      ContextMenuStrip = new ContextMenuStrip();
      ContextMenuStrip.Items.Add($"Match left name: {compare.left.name}").Click += MatchLeft;
      ContextMenuStrip.Items.Add($"Match right name: {compare.right.name}").Click += MatchRight;
    }

    private void Match(bool isRight)
    {
      var left = isRight ? compare.left : compare.right;
      var right = isRight ? compare.right : compare.left;

      if (DialogHelper.QuestionDialog("Are you sure?") == DialogResult.Yes)
      {
        var newName = left.path + right.name;
        File.Move(left.absolutePath, newName);
      }
    }

    private void MatchRight(object sender, EventArgs e) => Match(true);

    private void MatchLeft(object sender, EventArgs e) => Match(false);
  }
}
