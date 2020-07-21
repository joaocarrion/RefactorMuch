using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Threading;

namespace InteractiveMerge.Configuration
{
  public class ConfigData
  {
    public DictionaryProperty config { get; private set; }

    private readonly string path;

    public ConfigData(string path)
    {
      this.path = path;

      try
      {
        using (BufferedStream bs = new BufferedStream(File.OpenRead(path)))
        {
          bs.Seek(0, SeekOrigin.End);
          var size = bs.Position;
          bs.Seek(0, SeekOrigin.Begin);

          byte[] data = new byte[size + 1];
          bs.Read(data, 0, (int)size + 1);

          Utf8JsonReader reader = new Utf8JsonReader(new ReadOnlySpan<byte>(data));
          config = JsonSerializer.Deserialize<DictionaryProperty>(ref reader);
        }
      }
      catch (IOException)
      {
        config = new DictionaryProperty { name = "config" };
      }
      catch (Exception exc)
      {
        MessageBox.Show(exc.Message, "Unknown Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    /// <summary>
    /// Explicitly locked save function, expecting maintenance on properties from only the main thread
    /// </summary>
    public void Save()
    {
      using (BufferedStream bs = new BufferedStream(File.Create(path)))
      {
        var task = JsonSerializer.SerializeAsync(bs, config);
        while (!task.IsCompleted)
          Thread.Sleep(20);
      }
    }
  }
}
