using Oxide.Core.Plugins;

namespace Oxide.Ext.Discord.Pooling
{
    /// <summary>
    /// Represents a pool
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// Resizes the pool
        /// </summary>
        /// <param name="newSize"></param>
        void Resize(int newSize);
        
        /// <summary>
        /// Called on a pool when a plugin is unloaded
        /// </summary>
        /// <param name="plugin"></param>
        void OnPluginUnloaded(Plugin plugin);

        /// <summary>
        /// Clears the pool of all items
        /// </summary>
        void Clear();

        /// <summary>
        /// Wipes all pools of the given type
        /// </summary>
        void Wipe();
    }
}