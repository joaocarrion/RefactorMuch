using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveMerge.Configuration
{

  public class ArrayProperty : Property, IList<object>, IEnumerable<object>
  {
    private IList<object> properties;

    public ArrayProperty()
    {
      properties = new List<object>();
      value = properties;
    }

    public ArrayProperty(object []array)
    {
      properties = new List<object>(array);
      value = properties;
    }

    public object this[int index] { get => properties[index]; set => properties[index] = value; }

    public int Count => properties.Count;

    public bool IsReadOnly => false;

    public void Add(object item)
    {
      properties.Add(item);
    }

    public void Clear()
    {
      properties.Clear();
    }

    public bool Contains(object item)
    {
      return properties.Contains(item);
    }

    public void CopyTo(object[] array, int arrayIndex)
    {
      properties.CopyTo(array, arrayIndex);
    }

    public IEnumerator<object> GetEnumerator()
    {
      return properties.GetEnumerator();
    }

    public int IndexOf(object item)
    {
      return properties.IndexOf(item);
    }

    public void Insert(int index, object item)
    {
      properties.Insert(index, item);
    }

    public bool Remove(object item)
    {
      return properties.Remove(item);
    }

    public void RemoveAt(int index)
    {
      properties.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return properties.GetEnumerator();
    }
  }
}
