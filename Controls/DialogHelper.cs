using RefactorMuch.Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RefactorMuch.Controls
{
  public class DialogHelper
  {
    public static DialogResult QuestionDialog(string text)
    {
      return MessageBox.Show(text, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    public static DialogResult ErrorDialog(string text)
    {
      return MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void InfoDialog(string text)
    {
      MessageBox.Show(text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static bool RemoveFile(FileCompareData file)
    {
      try
      {
        if (QuestionDialog($"Are you sure you want to remove {file.name} from {file.path}") == DialogResult.Yes)
        {
          File.Delete(file.absolutePath);
          if (Directory.GetFiles(file.path).Length == 0 && Directory.GetDirectories(file.path).Length == 0)
          {
            if (QuestionDialog($"Directory {file.path} is Empty. Remove directory?") == DialogResult.Yes)
              Directory.Delete(file.path);
          }
        }

        return true;
      }
      catch (Exception exc)
      {
        ErrorDialog($"Error deleting file {file.name}: {exc.Message}");
      }

      return false;
    }
  }
}
