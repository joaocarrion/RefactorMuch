using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RefactorMuch.Configuration
{
  public class ConfigData
  {
    private readonly string path;

    public JObject doc { get; private set; }

    private ConfigData(string path)
    {
      this.path = path;

      try
      {
        using (var reader = new JsonTextReader(new StreamReader(File.OpenRead(path), Encoding.UTF8)))
          doc = (JObject)JObject.ReadFrom(reader);
      }
      catch (IOException)
      {
        doc = JObject.FromObject(new { });
      }
      catch (Exception exc)
      {
        MessageBox.Show(exc.Message, "Unknown Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        throw;
      }
    }

    public void Save()
    {
      using (TextWriter writer = new StreamWriter(File.Create(path)))
        writer.Write(doc.ToString());
    }

    public static ConfigData instance = null;
    public static ConfigData GetInstance()
    {
      if (instance == null)
        instance = new ConfigData(Application.LocalUserAppDataPath + "\\app-config.json");
      return instance;
    }

  }
}
