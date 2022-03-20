﻿using System.Text;
using Oxide.Ext.Discord.Entities.Gatway;
using Oxide.Ext.Discord.Logging;

namespace Oxide.Ext.Discord
{
    /// <summary>
    /// Represents settings used to connect to discord
    /// </summary>
    public class DiscordSettings
    {
        /// <summary>
        /// API token for the bot
        /// </summary>
        public string ApiToken;

        /// <summary>
        /// Discord Extension Logging Level.
        /// See <see cref="LogLevel"/>
        /// </summary>
        public DiscordLogLevel LogLevel = DiscordLogLevel.Info;
        
        /// <summary>
        /// Intents that your bot needs to work
        /// See <see cref="GatewayIntents"/>
        /// </summary>
        public GatewayIntents Intents = GatewayIntents.None;
        
        private string _hiddenToken;
        
        /// <summary>
        /// Hides the token but keeps the format to allow for debugging token issues without showing the token.
        /// </summary>
        /// <returns></returns>
        public string GetHiddenToken()
        {
            if (_hiddenToken != null)
            {
                return _hiddenToken;
            }

            StringBuilder sb = new StringBuilder();
            int first = ApiToken.IndexOf('.');
            int second = ApiToken.IndexOf('.', first + 1);

            sb.Append('#', first);
            sb.Append('.');
            sb.Append('#', second - first);
            sb.Append('.');
            sb.Append(ApiToken.Substring(second + 1));

            _hiddenToken = sb.ToString();
            return _hiddenToken;
        }
    }
}