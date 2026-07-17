using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class BiDictionary<TKey1, TKey2>
{
    private Dictionary<TKey1, TKey2> _K1K2 = new Dictionary<TKey1, TKey2>();
    private Dictionary<TKey2, TKey1> _K2K1 = new Dictionary<TKey2, TKey1>();

    public bool TryGetByValue(TKey1 value, out TKey2 key1)
    {
        return _K1K2.TryGetValue(value, out key1);
    }

    public bool TryGetByValue(TKey2 value, out TKey1 key1)
    {
        return _K2K1.TryGetValue(value, out key1);
    }
    
    public void AddOrUpdate(TKey1 key1, TKey2 key2)
    {
        _K1K2[key1] = key2;
        _K2K1[key2] = key1;
    }

    public void Remove(TKey1 key1, TKey2 key2)
    {
        _K1K2.Remove(key1);
        _K2K1.Remove(key2);
    }

    public Dictionary<TKey1, TKey2> GetKeyDictionary()
    {
        return _K1K2;
    }

    public Dictionary<TKey2, TKey1> GetValueDictionary()
    {
        return _K2K1;
    }

    public bool Contains(TKey1 key1)
    {
        return _K1K2.ContainsKey(key1);
    }
    
    public bool Contains(TKey2 key2)
    {
        return _K2K1.ContainsKey(key2);
    }

    public List<TKey1> Keys1()
    {
        return _K1K2.Keys.ToList();
    }

    public Dictionary<TKey1, TKey2> AsDictionaryKey1()
    {
        return _K1K2;
    }
    
    public Dictionary<TKey2, TKey1> AsDictionaryKey2()
    {
        return _K2K1;
    }
    
    internal void RemoveByKey(TKey1 k)
    {
        if(_K1K2.ContainsKey(k))
            Remove(k, _K1K2[k]);
    }
    
}