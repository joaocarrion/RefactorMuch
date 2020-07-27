using RefactorMuch.Parse;
using System;
using System.IO;
using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public partial class DuplicateNode : CrossCompareNode
  {
    public DuplicateNode(CrossCompare compare, int imageIndex) : base(compare, imageIndex)
    {
      Text = $"{compare.left.name,-32} ==> {compare.right.localPath}";
    }

    public void View() => Tools.GetInstance().ToolDictionary[Tool.ToolType.View].Run(compare.left.absolutePath);

    public void Remove(FileCompareData file)
    {
      try
      {
        if (DialogHelper.QuestionDialog($"Are you sure you want to remove {file.name} from {file.path}") == DialogResult.Yes)
        {
          File.Delete(file.absolutePath);
          if (Directory.GetFiles(file.path).Length == 0 && Directory.GetDirectories(file.path).Length == 0)
          {
            if (DialogHelper.QuestionDialog($"Directory {file.path} is Empty. Remove directory?") == DialogResult.Yes)
              Directory.Delete(file.path);
          }

          // remove node from view
          Remove();
        }
      }
      catch (Exception exc)
      {
        DialogHelper.ErrorDialog($"Error deleting file {file.name}: {exc.Message}");
      }
    }

    public void RemoveLeft() =>Remove(compare.left);
    public void RemoveRight() => Remove(compare.right);
  }
}
