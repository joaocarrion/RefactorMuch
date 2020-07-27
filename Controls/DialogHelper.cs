using System;
using System.Collections.Generic;
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

  }
}
