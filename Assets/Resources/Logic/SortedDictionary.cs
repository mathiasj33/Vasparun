using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SortedDictionary<K, V> : IEnumerable<K>
{
    private List<K> keys = new List<K>();
    private List<V> values = new List<V>();

    public void Add(K key, V value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public V Get(K key)
    {
        return values[keys.IndexOf(key)];
    }

    public int IndexOf(K key)
    {
        return keys.IndexOf(key);
    }

    public int IndexOf(V value)
    {
        return values.IndexOf(value);
    }

    public int Count
    {
        get { return keys.Count; }
    }

    public IEnumerator<K> GetEnumerator()
    {
        return ((IEnumerable<K>)keys).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<K>)keys).GetEnumerator();
    }
}