using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveMerge.Configuration
{
  public class DictionaryProperty : Property, IDictionary<string, Property>
  {
    private IDictionary<string, Property> properties;

    public DictionaryProperty()
    {
      properties = new Dictionary<string, Property>();
      value = properties;
    }

    public Property this[string key] { get => properties[key]; set => properties[key] = value; }

    public ICollection<string> Keys => properties.Keys;

    public ICollection<Property> Values => properties.Values;

    public int Count => properties.Count;

    public bool IsReadOnly => false;

    public void Add(string key, Property value)
    {
      properties.Add(key, value);
    }

    public void Add(KeyValuePair<string, Property> item)
    {
      properties.Add(item);
    }

    public void Clear()
    {
      properties.Clear();
    }

    public bool Contains(KeyValuePair<string, Property> item)
    {
      return properties.Contains(item);
    }

    public bool ContainsKey(string key)
    {
      return properties.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, Property>[] array, int arrayIndex)
    {
      properties.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<string, Property>> GetEnumerator()
    {
      return properties.GetEnumerator();
    }

    public bool Remove(string key)
    {
      return properties.Remove(key);
    }

    public bool Remove(KeyValuePair<string, Property> item)
    {
      return properties.Remove(item);
    }

    public bool TryGetValue(string key, out Property value)
    {
      return properties.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return properties.GetEnumerator();
    }
  }

}
