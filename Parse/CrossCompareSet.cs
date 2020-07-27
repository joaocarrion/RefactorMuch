using System.Collections;
using System.Collections.Generic;

namespace RefactorMuch.Parse
{
  public class CrossCompareSet : ISet<CrossCompare>
  {
    private float minValue = 0f;
    private float maxValue = 1.1f;
    private SortedSet<CrossCompare> set = new SortedSet<CrossCompare>();

    public CrossCompareSet(float minValue, float maxValue = 1.1f)
    {
      this.minValue = minValue;
      this.maxValue = maxValue;
    }

    public int Count => set.Count;

    public bool IsReadOnly => false;
    public override string ToString() => $"Cross Compare List: {set.Count}";

    bool ISet<CrossCompare>.Add(CrossCompare item) => item.similarity >= minValue && item.similarity < maxValue ? set.Add(item) : false;
    public void UnionWith(IEnumerable<CrossCompare> other) => set.UnionWith(other);
    public void IntersectWith(IEnumerable<CrossCompare> other) => set.IntersectWith(other);
    public void ExceptWith(IEnumerable<CrossCompare> other) => set.ExceptWith(other);
    public void SymmetricExceptWith(IEnumerable<CrossCompare> other) => set.SymmetricExceptWith(other);
    public bool IsSubsetOf(IEnumerable<CrossCompare> other) => set.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<CrossCompare> other) => set.IsSupersetOf(other);
    public bool IsProperSupersetOf(IEnumerable<CrossCompare> other) => set.IsProperSupersetOf(other);
    public bool IsProperSubsetOf(IEnumerable<CrossCompare> other) => set.IsProperSubsetOf(other);
    public bool Overlaps(IEnumerable<CrossCompare> other) => set.Overlaps(other);
    public bool SetEquals(IEnumerable<CrossCompare> other) => set.SetEquals(other);

    public void Add(CrossCompare item) => ((ISet<CrossCompare>)this).Add(item);
    public void Clear() => set.Clear();
    public bool Contains(CrossCompare item) => set.Contains(item);
    public void CopyTo(CrossCompare[] array, int arrayIndex) => set.CopyTo(array, arrayIndex);
    public bool Remove(CrossCompare item) => set.Remove(item);
    public IEnumerator<CrossCompare> GetEnumerator() => set.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();
  }
}