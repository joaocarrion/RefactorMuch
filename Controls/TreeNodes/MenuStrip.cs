using System;
using System.Windows.Forms;

namespace RefactorMuch.Controls.TreeNodes
{
  public class MenuStrip : ContextMenuStrip
  {
    public MenuStrip(string[] text, Action<TreeView>[] actions)
    {
      foreach (var t in text)
        Items.Add(t);

      for (int i = 0; i < Items.Count; ++i)
        Items[i].Click += (sender, e) => { actions[i]((TreeView)sender); };
    }
  }
}
