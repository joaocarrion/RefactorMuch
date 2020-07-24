using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace RefactorMuch.Configuration
{
  public class ConfigData
  {
    private readonly string path;

    public JObject doc { get; private set; }

    public ConfigData(string path)
    {
      this.path = path;

      try
      {
        using (var reader = new JsonTextReader(new StreamReader(File.OpenRead(path), Encoding.UTF8)))
          doc = (JObject)JObject.ReadFrom(reader);
      }
      catch (IOException)
      {
        doc = JObject.FromObject(new {});
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
  }
}
