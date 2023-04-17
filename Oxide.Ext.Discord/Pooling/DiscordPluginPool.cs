using System.Collections.Generic;
using System.IO;
using System.Text;
using Oxide.Core.Plugins;
using Oxide.Ext.Discord.Libraries.Placeholders;
using Oxide.Ext.Discord.Pooling.Entities;
using Oxide.Plugins;

namespace Oxide.Ext.Discord.Pooling
{
    /// <summary>
    /// Built in pooling for discord entities
    /// </summary>
    public class DiscordPluginPool
    {
        internal readonly Plugin Plugin;
        private readonly List<IPool> _pools = new List<IPool>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="plugin">Plugin the pool is for</param>
        public DiscordPluginPool(Plugin plugin)
        {
            Plugin = plugin;
        }

        internal void AddPool(IPool pool) => _pools.Add(pool);
        
        /// <summary>
        /// Returns a pooled object of type T
        /// Must inherit from <see cref="BasePoolable"/> and have an empty default constructor
        /// </summary>
        /// <typeparam name="T">Type to be returned</typeparam>
        /// <returns>Pooled object of type T</returns>
        public T Get<T>() where T : BasePoolable, new()
        {
            return (T)ObjectPool<T>.ForPlugin<ObjectPool<T>>(this).Get();
        }

        /// <summary>
        /// Returns a <see cref="BasePoolable"/> back into the pool
        /// </summary>
        /// <param name="value">Object to free</param>
        /// <typeparam name="T">Type of object being freed</typeparam>
        internal void Free<T>(T value) where T : BasePoolable, new()
        {
            ObjectPool<T>.ForPlugin<ObjectPool<T>>(this).Free(value);
        }

        /// <summary>
        /// Returns a pooled <see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T">Type for the list</typeparam>
        /// <returns>Pooled List</returns>
        public List<T> GetList<T>()
        {
            return ListPool<T>.ForPlugin<ListPool<T>>(this).Get();
        }

        /// <summary>
        /// Returns a pooled <see cref="Hash{TKey, TValue}"/>
        /// </summary>
        /// <typeparam name="TKey">Type for the key</typeparam>
        /// <typeparam name="TValue">Type for the value</typeparam>
        /// <returns>Pooled Hash</returns>
        public Hash<TKey, TValue> GetHash<TKey, TValue>()
        {
            return HashPool<TKey, TValue>.ForPlugin<HashPool<TKey, TValue>>(this).Get();
        }
        
        /// <summary>
        /// Returns a pooled <see cref="HashSet{T}"/>
        /// </summary>
        /// <typeparam name="T">Type for the HashSet</typeparam>
        /// <returns>Pooled List</returns>
        public HashSet<T> GetHashSet<T>()
        {
            return HashSetPool<T>.ForPlugin<HashSetPool<T>>(this).Get();
        }

        /// <summary>
        /// Returns a pooled <see cref="StringBuilder"/>
        /// </summary>
        /// <returns>Pooled <see cref="StringBuilder"/></returns>
        public StringBuilder GetStringBuilder()
        {
            return StringBuilderPool.ForPlugin<StringBuilderPool>(this).Get();
        }
        
        /// <summary>
        /// Returns a pooled <see cref="StringBuilder"/>
        /// </summary>
        /// <param name="initial">Initial text for the builder</param>
        /// <returns>Pooled <see cref="StringBuilder"/></returns>
        public StringBuilder GetStringBuilder(string initial)
        {
            StringBuilder builder = StringBuilderPool.ForPlugin<StringBuilderPool>(this).Get();
            builder.Append(initial);
            return builder;
        }

        /// <summary>
        /// Returns a pooled <see cref="MemoryStream"/>
        /// </summary>
        /// <returns>Pooled <see cref="MemoryStream"/></returns>
        public MemoryStream GetMemoryStream()
        {
            return MemoryStreamPool.ForPlugin<MemoryStreamPool>(this).Get();
        }

        /// <summary>
        /// Returns a pooled <see cref="PlaceholderData"/>
        /// </summary>
        /// <returns>Pooled <see cref="PlaceholderData"/></returns>
        public PlaceholderData GetPlaceholderData()
        {
            return (PlaceholderData)PlaceholderDataPool.ForPlugin<PlaceholderDataPool>(this).Get();
        }
        
        /// <summary>
        /// Returns a pooled <see cref="Boxed{T}"/>
        /// </summary>
        /// <typeparam name="T">Type for the Boxed</typeparam>
        /// <returns>Pooled Boxed</returns>
        internal Boxed<T> GetBoxed<T>(T value)
        {
            Boxed<T> boxed = BoxedPool<T>.ForPlugin<BoxedPool<T>>(this).Get();
            boxed.Value = value;
            return boxed;
        }

        /// <summary>
        /// Free's a pooled <see cref="List{T}"/>
        /// </summary>
        /// <param name="list">List to be freed</param>
        /// <typeparam name="T">Type of the list</typeparam>
        public void FreeList<T>(List<T> list)
        {
            ListPool<T>.ForPlugin<ListPool<T>>(this).Free(list);
        }

        /// <summary>
        /// Frees a pooled <see cref="Hash{TKey, TValue}"/>
        /// </summary>
        /// <param name="hash">Hash to be freed</param>
        /// <typeparam name="TKey">Type for key</typeparam>
        /// <typeparam name="TValue">Type for value</typeparam>
        public void FreeHash<TKey, TValue>(Hash<TKey, TValue> hash)
        {
            HashPool<TKey, TValue>.ForPlugin<HashPool<TKey, TValue>>(this).Free(hash);
        }
        
        /// <summary>
        /// Free's a pooled <see cref="HashSet{T}"/>
        /// </summary>
        /// <param name="list">HashSet to be freed</param>
        /// <typeparam name="T">Type of the HashSet</typeparam>
        public void FreeHashSet<T>(HashSet<T> list)
        {
            HashSetPool<T>.ForPlugin<HashSetPool<T>>(this).Free(list);
        }

        /// <summary>
        /// Frees a <see cref="StringBuilder"/> back to the pool
        /// </summary>
        /// <param name="sb">StringBuilder being freed</param>
        public void FreeStringBuilder(StringBuilder sb)
        {
            StringBuilderPool.ForPlugin<StringBuilderPool>(this).Free(sb);
        }

        /// <summary>
        /// Frees a <see cref="StringBuilder"/> back to the pool returning the built <see cref="string"/>
        /// </summary>
        /// <param name="sb"><see cref="StringBuilder"/> being freed</param>
        public string FreeStringBuilderToString(StringBuilder sb)
        {
            string result = sb?.ToString();
            FreeStringBuilder(sb);
            return result;
        }

        /// <summary>
        /// Frees a <see cref="MemoryStream"/> back to the pool
        /// </summary>
        /// <param name="stream"><see cref="MemoryStream"/> being freed</param>
        public void FreeMemoryStream(MemoryStream stream)
        {
            MemoryStreamPool.ForPlugin<MemoryStreamPool>(this).Free(stream);
        }

        /// <summary>
        /// Frees a <see cref="PlaceholderData"/> back to the pool
        /// </summary>
        /// <param name="data"><see cref="PlaceholderData"/> being freed</param>
        public void FreePlaceholderData(PlaceholderData data)
        {
            PlaceholderDataPool.ForPlugin<PlaceholderDataPool>(this).Free(data);
        }
        
        /// <summary>
        /// Free's a pooled <see cref="BoxedPool{T}"/>
        /// </summary>
        /// <param name="boxed">Boxed to be freed</param>
        /// <typeparam name="T">Type of the Boxed</typeparam>
        internal void FreeBoxed<T>(Boxed<T> boxed)
        {
            BoxedPool<T>.ForPlugin<BoxedPool<T>>(this).Free(boxed);
        }

        internal void OnPluginUnloaded(Plugin plugin)
        {
            for (int index = 0; index < _pools.Count; index++)
            {
                IPool pool = _pools[index];
                pool.OnPluginUnloaded(plugin);
            }
        }
        
        internal void Clear()
        {
            for (int index = 0; index < _pools.Count; index++)
            {
                IPool pool = _pools[index];
                pool.Clear();
            }
        }

        internal void Wipe()
        {
            for (int index = 0; index < _pools.Count; index++)
            {
                IPool pool = _pools[index];
                pool.Wipe();
            }
        }
    }
}