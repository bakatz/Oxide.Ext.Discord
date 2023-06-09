using System;
using System.Collections.Generic;
using Oxide.Core.Plugins;
using Oxide.Ext.Discord.Entities.Interactions.ApplicationCommands;
using Oxide.Ext.Discord.Entities.Permissions;
using Oxide.Ext.Discord.Exceptions.Builders;
using Oxide.Ext.Discord.Exceptions.Entities.Interactions.ApplicationCommands;
using Oxide.Ext.Discord.Exceptions.Libraries.DiscordLocale;
using Oxide.Ext.Discord.Libraries.Locale;
using Oxide.Ext.Discord.Libraries.Templates.Commands;
using Oxide.Plugins;

namespace Oxide.Ext.Discord.Builders.ApplicationCommands
{
    /// <summary>
    /// Builder to use when building application commands
    /// </summary>
    public class ApplicationCommandBuilder
    {
        internal readonly CommandCreate Command;
        private CommandOptionType? _chosenType;
        private readonly ServerLocale _defaultLanguage;

        /// <summary>
        /// Creates a new Application Command Builder
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <param name="description">Description of the command</param>
        /// <param name="type">Command type</param>
        public ApplicationCommandBuilder(string name, string description, ApplicationCommandType type): this(name, description, type, ServerLocale.Default) {}
        
        /// <summary>
        /// Creates a new Application Command Builder
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <param name="description">Description of the command</param>
        /// <param name="type">Command type</param>
        /// <param name="defaultLanguage">Language the application command is being created in</param>
        public ApplicationCommandBuilder(string name, string description, ApplicationCommandType type, ServerLocale defaultLanguage)
        {
            if (!defaultLanguage.IsValid) ServerLocaleNotFoundException.ThrowNotFound(defaultLanguage);
            InvalidApplicationCommandException.ThrowIfInvalidName(name, false);
            InvalidApplicationCommandException.ThrowIfInvalidDescription(description, type);
            
            Command = new CommandCreate(name, description, type, new List<CommandOption>());
            _defaultLanguage = defaultLanguage;
            
            AddNameLocalization(Command.Name, _defaultLanguage);
            AddDescriptionLocalization(Command.Description, _defaultLanguage);
        }
        
        /// <summary>
        /// Adds default command permissions
        /// </summary>
        /// <param name="permissions">Default Permissions for the command</param>
        /// <returns></returns>
        public ApplicationCommandBuilder AddDefaultPermissions(PermissionFlags permissions)
        {
            Command.DefaultMemberPermissions = permissions;
            return this;
        }
        
        /// <summary>
        /// Allows the command to be used in a direct message
        /// </summary>
        /// <param name="allowInDm">Allows a command to be used in a direct message</param>
        /// <returns></returns>
        public ApplicationCommandBuilder AddDirectMessagePermission(bool allowInDm)
        {
            Command.DmPermission = allowInDm;
            return this;
        }
        
        /// <summary>
        /// Set whether the command is enabled by default when the app is added to a guild
        /// </summary>
        /// <param name="enabled">If the command is enabled</param>
        /// <returns>This</returns>
        [Obsolete("This field has been deprecated by discord and will be removed in a future update. Please use AddDefaultPermissions instead.")]
        public ApplicationCommandBuilder SetEnabled(bool enabled)
        {
            Command.DefaultPermissions = enabled;
            return this;
        }

        /// <summary>
        /// Adds command name localizations for a given plugin and lang key
        /// </summary>
        /// <param name="plugin">Plugin containing the localizations</param>
        /// <param name="langKey">Lang Key containing the localized text</param>
        /// <returns></returns>
        [Obsolete("AddNameLocalizations(Plugin plugin, string langKey) has been deprecated and will be removed in the future. Please use AddNameLocalization(string name, string lang) instead")]
        public ApplicationCommandBuilder AddNameLocalizations(Plugin plugin, string langKey)
        {
            Command.NameLocalizations =  DiscordLocales.Instance.GetDiscordLocalizations(plugin, langKey);
            return this;
        }

        /// <summary>
        /// Adds Application Command Name Localizations
        /// </summary>
        /// <param name="name">Localized name value</param>
        /// <param name="serverLocale">Oxide lang the value is in</param>
        /// <returns>This</returns>
        public ApplicationCommandBuilder AddNameLocalization(string name, ServerLocale serverLocale)
        {
            if (Command.NameLocalizations == null)
            {
                Command.NameLocalizations = new Hash<string, string>();
            }

            DiscordLocale discordLocale = serverLocale.GetDiscordLocale();
            if (discordLocale.IsValid)
            {
                Command.NameLocalizations[discordLocale.Id] = name;
            }
            
            return this;
        }
        
        /// <summary>
        /// Adds command description localizations for a given plugin and lang key
        /// </summary>
        /// <param name="plugin">Plugin containing the localizations</param>
        /// <param name="langKey">Lang Key containing the localized text</param>
        /// <returns></returns>
        [Obsolete("AddDescriptionLocalizations(Plugin plugin, string langKey) has been deprecated and will be removed in the future. Please use AddDescriptionLocalization(string name, string lang) instead")]
        public ApplicationCommandBuilder AddDescriptionLocalizations(Plugin plugin, string langKey)
        {
            Command.DescriptionLocalizations =  DiscordLocales.Instance.GetDiscordLocalizations(plugin, langKey);
            return this;
        }
        
        /// <summary>
        /// Adds Application Command Description Localizations
        /// </summary>
        /// <param name="description">Localized description value</param>
        /// <param name="serverLocale">Oxide lang the value is in</param>
        /// <returns>This</returns>
        public ApplicationCommandBuilder AddDescriptionLocalization(string description, ServerLocale serverLocale)
        {
            if (Command.DescriptionLocalizations == null)
            {
                Command.DescriptionLocalizations = new Hash<string, string>();
            }

            DiscordLocale discordLocale = serverLocale.GetDiscordLocale();
            if (discordLocale.IsValid)
            {
                Command.DescriptionLocalizations[discordLocale.Id] = description;
            }

            return this;
        }

        /// <summary>
        /// Creates a new SubCommandGroup
        /// SubCommandGroups contain subcommands
        /// Your root command can only contain 
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <param name="description">Description of the command</param>
        /// <returns><see cref="SubCommandGroupBuilder"/></returns>
        /// <exception cref="Exception">Thrown if trying to add a subcommand group to</exception>
        public SubCommandGroupBuilder AddSubCommandGroup(string name, string description)
        {
            InvalidCommandOptionException.ThrowIfInvalidName(name, false);
            InvalidCommandOptionException.ThrowIfInvalidDescription(description, false);

            ApplicationCommandBuilderException.ThrowIfMixingSubCommandGroups(_chosenType);
            ApplicationCommandBuilderException.ThrowIfAddingSubCommandToMessageOrUser(this);

            _chosenType = CommandOptionType.SubCommandGroup;

            return new SubCommandGroupBuilder(name, description, this, _defaultLanguage);
        }

        /// <summary>
        /// Adds a sub command to the root command
        /// </summary>
        /// <param name="name">Name of the sub command</param>
        /// <param name="description">Description for the sub command</param>
        /// <returns><see cref="ApplicationSubCommandBuilder"/></returns>
        /// <exception cref="Exception">Thrown if previous type was not SubCommand or Creation type is not ChatInput</exception>
        public ApplicationSubCommandBuilder AddSubCommand(string name, string description)
        {
            InvalidCommandOptionException.ThrowIfInvalidName(name, false);
            InvalidCommandOptionException.ThrowIfInvalidDescription(description, false);

            ApplicationCommandBuilderException.ThrowIfMixingSubCommandGroups(_chosenType);
            ApplicationCommandBuilderException.ThrowIfAddingSubCommandToMessageOrUser(this);

            _chosenType = CommandOptionType.SubCommand;

            return new ApplicationSubCommandBuilder(Command.Options, name, description, this, _defaultLanguage);
        }

        /// <summary>
        /// Adds a command option.
        /// </summary>
        /// <param name="type">The type of option. Cannot be SubCommand or SubCommandGroup</param>
        /// <param name="name">Name of the option</param>
        /// <param name="description">Description for the option</param>
        /// <returns><see cref="ApplicationCommandOptionBuilder"/></returns>
        public ApplicationCommandOptionBuilder AddOption(CommandOptionType type, string name, string description)
        {
            ApplicationCommandBuilderException.ThrowIfMixingCommandOptions(_chosenType);

            return new ApplicationCommandOptionBuilder(Command.Options, type, name, description, this, _defaultLanguage);
        }

        /// <summary>
        /// Returns the created command
        /// </summary>
        /// <returns></returns>
        public CommandCreate Build()
        {
            return Command;
        }

        /// <summary>
        /// Returns a built <see cref="DiscordCommandLocalization"/> using the provided name / descriptions as the default
        /// </summary>
        /// <returns></returns>
        public DiscordCommandLocalization BuildCommandLocalization(string lang = DiscordLocales.DefaultServerLanguage)
        {
            return new DiscordCommandLocalization(Command, ServerLocale.Parse(lang));
        }
    }
}