using System.Collections;
using System.Collections.Generic;

namespace RefactorMuch.Parse
{
  public class CrossCompareList : IList<CrossCompare>
  {
    private float minValue = 0;
    private List<CrossCompare> list = new List<CrossCompare>();

    public CrossCompareList(float minValue)
    {
      this.minValue = minValue;
    }

    public override string ToString()
    {
      return $"Cross Compare List: {list.Count}";
    }

    public CrossCompare this[int index] { get => list[index]; set => list[index] = value; }

    public int Count => list.Count;

    public bool IsReadOnly => false;

    public void Add(CrossCompare item)
    {
      if (item.similarity > minValue)
      {
        foreach (var e in list)
        {
          if (e.similarity < item.similarity)
          {
            list.Insert(list.IndexOf(e), item);
            return;
          }
        }

        list.Add(item);
      }
    }

    public void Clear()
    {
      list.Clear();
    }

    public bool Contains(CrossCompare item) => list.Contains(item);
    public void CopyTo(CrossCompare[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
    public IEnumerator<CrossCompare> GetEnumerator() => list.GetEnumerator();
    public int IndexOf(CrossCompare item) => list.IndexOf(item);
    public void Insert(int index, CrossCompare item) => list.Insert(index, item);
    public bool Remove(CrossCompare item) => list.Remove(item);
    public void RemoveAt(int index) => list.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
  }
}
