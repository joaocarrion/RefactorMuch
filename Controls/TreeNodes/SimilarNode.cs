using RefactorMuch.Parse;
using System;

namespace RefactorMuch.Controls
{
  public partial class RefactoredNode : CrossCompareNode
  {
    public RefactoredNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      Text = $"{Math.Round(compare.similarity * 100, 0):00}%: {compare.left.name} => {compare.right.name} ({compare.left.localPath} => {compare.right.localPath})";
    }

    private void Diff() => Tools.GetInstance().ToolDictionary[Tool.ToolType.Diff].Run(compare.left.absolutePath, compare.right.absolutePath);
  }
}
