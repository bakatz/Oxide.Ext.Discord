using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Plugins;

namespace Oxide.Ext.Discord.Extensions
{
    public static class HashExt
    {
        public static void RemoveAll<TKey, TValue>(this Hash<TKey, TValue> dict, Func<TValue, bool> predicate)
        {
            if (dict == null)
            {
                return;
            }
            
            foreach (KeyValuePair<TKey, TValue> key in dict.Where(k => predicate(k.Value)).ToList())
            {
                dict.Remove(key.Key);
            }
        }

        public static Hash<TKey, TValue> ToHash<TKey, TValue>(this List<TValue> list, Func<TValue, TKey> key)
        {
            Hash<TKey, TValue> hash = new Hash<TKey, TValue>();
            for (int i = 0; i < list.Count; i++)
            {
                TValue value = list[i];
                hash[key(value)] = value;
            }

            return hash;
        }
    }
}