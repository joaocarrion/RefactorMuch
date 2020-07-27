using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactorMuch.Util
{
  public class MultiTask
  {
    public static Task[] Run(Action []actions)
    {
      var tasks = new List<Task>();
      foreach (var ac in actions)
        tasks.Add(Task.Run(ac));

      return tasks.ToArray();
    }

    public static Task<T>[] Run<T>(Func<T>[] actions)
    {
      var tasks = new List<Task<T>>();
      foreach (var ac in actions)
        tasks.Add(Task.Run(ac));

      return tasks.ToArray();
    }
  }
}
