using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro
{
    class DefaultValuedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dic;

        public DefaultValuedDictionary()
        {
            _dic = new Dictionary<TKey, TValue>();
        }

        public DefaultValuedDictionary(IEqualityComparer<TKey> comparer)
        {
            _dic = new Dictionary<TKey, TValue>(comparer);
        }

        public DefaultValuedDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dic = new Dictionary<TKey, TValue>(dictionary);
        }

        public DefaultValuedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dic = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public ICollection<TKey> Keys
        {
            get { return _dic.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _dic.Values; }
        }

        public virtual void Add(TKey key, TValue value)
        {
            _dic.Add(key, value);
        }

        public virtual bool Remove(TKey key)
        {
            return _dic.Remove(key);
        }

        public virtual void Clear()
        {
            _dic.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return _dic.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dic.TryGetValue(key, out value);
        }

        public virtual TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (_dic.TryGetValue(key, out value))
                {
                    return value;
                }

                return default(TValue);
            }
            set
            {
                if (_dic.ContainsKey(key))
                {
                    _dic[key] = value;
                }
                else
                {
                    _dic.Add(key, value);
                }
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            (_dic as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return (_dic as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var collection = _dic as ICollection<KeyValuePair<TKey, TValue>>;
            collection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _dic.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return (_dic as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
