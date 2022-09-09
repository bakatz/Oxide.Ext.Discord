using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oxide.Core;
using Oxide.Core.Libraries;
using Oxide.Core.Plugins;
using Oxide.Ext.Discord.Cache;
using Oxide.Ext.Discord.Exceptions.Libraries;
using Oxide.Ext.Discord.Extensions;
using Oxide.Ext.Discord.Json.Serialization;
using Oxide.Ext.Discord.Logging;

namespace Oxide.Ext.Discord.Libraries.Templates
{
    /// <summary>
    /// Oxide Library for Discord Templates
    /// </summary>
    public abstract class BaseTemplateLibrary : Library
    {
        private readonly string _rootDir = Path.Combine(Interface.Oxide.InstanceDirectory, "discord", "templates");
        private readonly JsonSerializer _serializer = JsonSerializer.CreateDefault();
        protected readonly ILogger Logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        protected BaseTemplateLibrary(ILogger logger)
        {
            Logger = logger;
            if (!Directory.Exists(_rootDir))
            {
                Directory.CreateDirectory(_rootDir);
            }
        }

        internal async Task<T> LoadTemplate<T>(TemplateType type, TemplateId id) where T : BaseTemplate
        {
            string path = GetTemplatePath(type, id);
            if (!File.Exists(path))
            {
                return null;
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return await DiscordJsonReader.DeserializeFromAsync<T>(_serializer, stream).ConfigureAwait(false);
            }
        }

        internal Task<T> LoadTemplate<T>(TemplateType type, TemplateId id, string language) where T : BaseTemplate
        {
            return LoadTemplate<T>(type, new TemplateId(id, language));
        }

        internal Task CreateFile<T>(string path, T template) where T : BaseTemplate
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileMode mode = File.Exists(path) ? FileMode.Truncate : FileMode.Create;

            using (FileStream stream = new FileStream(path, mode))
            {
                DiscordJsonWriter writer = new DiscordJsonWriter();
                writer.Write(_serializer, template);
                writer.Stream.CopyToPooled(stream);
                writer.Dispose();
            }

            return Task.CompletedTask;
        }

        internal async Task MoveFiles<T>(TemplateType type, TemplateId id, TemplateVersion minSupportedVersion) where T : BaseTemplate
        {
            foreach (string dir in Directory.EnumerateDirectories(GetTemplateFolder(type, id.PluginName)))
            {
                string lang = Path.GetDirectoryName(dir);
                TemplateId langId = new TemplateId(id, lang);
                
                string oldPath = GetTemplatePath(type, langId);
                if (!File.Exists(oldPath))
                {
                    continue;
                }
                
                T template = await LoadTemplate<T>(type, langId).ConfigureAwait(false);
                if (template == null)
                {
                    continue;
                }

                if (template.Version >= minSupportedVersion)
                {
                    continue;
                }

                string newPath = GetRenamePath(oldPath, template.Version);
                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }
                
                File.Move(oldPath, newPath);
            }
        }

        internal string GetTemplateFolder(TemplateType type, string plugin)
        {
            return Path.Combine(_rootDir, plugin, GetTemplateTypePath(type));
        }
        
        internal string GetTemplatePath(TemplateType type, TemplateId id)
        {
            DiscordTemplateException.ThrowIfInvalidTemplateName(id.TemplateName);

            if (string.IsNullOrEmpty(id.Language))
            {
                return Path.Combine(_rootDir, id.PluginName, GetTemplateTypePath(type), $"{id.TemplateName}.json");
            }
            return Path.Combine(_rootDir, id.PluginName, GetTemplateTypePath(type), id.Language, $"{id.TemplateName}.json");
        }

        private string GetRenamePath(string path, TemplateVersion version)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            return Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.{version}.json");
        }

        private string GetTemplateTypePath(TemplateType type) => EnumCache<TemplateType>.ToLower(type);

        internal abstract void OnPluginUnloaded(Plugin plugin);
    }
}