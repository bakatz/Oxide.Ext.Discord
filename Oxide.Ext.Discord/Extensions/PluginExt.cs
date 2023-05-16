using System;
using System.Runtime.CompilerServices;
using Oxide.Core.Plugins;
using Oxide.Ext.Discord.Libraries.Pooling;
using Oxide.Ext.Discord.Plugins;
using Oxide.Ext.Discord.Pooling;
using Oxide.Ext.Discord.Types;
using Oxide.Plugins;

namespace Oxide.Ext.Discord.Extensions
{
    /// <summary>
    /// Extension methods for plugins
    /// </summary>
    public static class PluginExt
    {
        private static readonly Hash<PluginId, string> FullNameCache = new Hash<PluginId, string>();
        private static readonly BidirectionalDictionary<PluginId, Plugin> PluginIds = new BidirectionalDictionary<PluginId, Plugin>();

        /// <summary>
        /// Returns a pool for the given plugin
        /// </summary>
        /// <param name="plugin">Plugin the pool is for</param>
        /// <returns></returns>
        public static DiscordPluginPool GetPool(this Plugin plugin) => DiscordPool.Instance.GetOrCreate(plugin);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static PluginId Id(this Plugin plugin)
        {
            if (PluginIds.TryGetValue(plugin, out PluginId id))
            {
                return id;
            }

            id = new PluginId(plugin);
            PluginIds[plugin] = id;
            return id;
        }

        internal static string PluginName(this Plugin plugin) => plugin?.Name ?? throw new ArgumentNullException(nameof(plugin));
        internal static string PluginName(this PluginId pluginId)
        {
            if (pluginId.IsValid && PluginIds.TryGetValue(pluginId, out Plugin plugin))
            {
                return plugin.Name;
            }
            
            return $"Invalid Plugin ID: {pluginId.Id}";
        }

        internal static string FullName(this Plugin plugin)
        {
            if (plugin == null) throw new ArgumentNullException(nameof(plugin));
            PluginId pluginId = plugin.Id();
            string name = FullNameCache[pluginId];
            if (name == null)
            {
                name = CreatePluginFullName(plugin);
                FullNameCache[pluginId] = name;
            }

            return name;
        }
        
        internal static string FullName(this PluginId pluginId)
        {
            if (pluginId.IsValid && PluginIds.TryGetValue(pluginId, out Plugin plugin))
            {
                return plugin.FullName();
            }
            
            return $"Invalid Plugin ID: {pluginId.Id}";
        }

        internal static string GetFullName(PluginId pluginId)
        {
            if (pluginId.IsValid && PluginIds.TryGetValue(pluginId, out Plugin plugin))
            {
                return plugin.FullName();
            }

            return $"Invalid Plugin ID: {pluginId.Id}";
        }

        internal static void OnPluginLoaded(Plugin plugin)
        {
            FullNameCache[plugin.Id()] = CreatePluginFullName(plugin);
        }

        internal static void OnPluginUnloaded(Plugin plugin)
        {
            FullNameCache.Remove(plugin.Id());
        }

        private static string CreatePluginFullName(Plugin plugin) => $"{plugin.Name} by {plugin.Author} v{plugin.Version}";
    }
}