using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RefactorMuch.Controls
{
  public class Tool
  {
    public enum ToolType
    {
      View,
      Diff,
    }

    public string path;
    public string parameters;
    public ToolType type;

    public Exception lastException = null;

    public void Run(params string[] arguments)
    {
      try
      {
        var args = string.Join(" ", (from arg in arguments.ToList<string>() select $"\"{arg}\""));
        Process.Start(path, $"{parameters} {args}");
      }
      catch (Exception exc)
      {
        DialogHelper.ErrorDialog($"Couldn't run tool: {exc.Message}");
      }
    }
  }

  public class Tools
  {
    public Dictionary<Tool.ToolType, Tool> ToolDictionary { get; } = new Dictionary<Tool.ToolType, Tool>();

    private Tools()
    {
      var config = Configuration.ConfigData.GetInstance();
      var toolArray = config.doc.Value<JArray>("tools");
      foreach (var t in toolArray)
      {
        var tool = new Tool
        {
          path = t.Value<string>("path"),
          parameters = t.Value<string>("parameters"),
          type = t.Value<string>("type").Equals("View") ? Tool.ToolType.View : Tool.ToolType.Diff
        };

        ToolDictionary.Add(tool.type, tool);
      }
    }

    private static Tools instance;
    public static Tools GetInstance()
    {
      if (instance == null) instance = new Tools();
      return instance;
    }
  }
}
