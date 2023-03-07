using Oxide.Ext.Discord.Pooling.Entities;

namespace Oxide.Ext.Discord.Pooling
{
    /// <summary>
    /// Represents a pool for <see cref="Boxed{T}"/>;
    /// </summary>
    /// <typeparam name="T">Type that will be in the boxed object</typeparam>
    internal class BoxedPool<T> : BasePool<Boxed<T>>
    {
        internal static readonly IPool<Boxed<T>> Instance = new BoxedPool<T>();
        
        static BoxedPool()
        {
            DiscordPool.Pools.Add(Instance);
        }

        private BoxedPool() : base(512) { }

        protected override void OnGetItem(Boxed<T> item)
        {
            item.LeavePool();
        }

        protected override Boxed<T> CreateNew() => new Boxed<T>();
    }
}