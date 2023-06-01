using System;
using Oxide.Ext.Discord.Entities;

namespace Oxide.Ext.Discord.Connections
{
    public class BotToken
    {
        public readonly string Token;
        public readonly string HiddenToken;
        public readonly Snowflake ApplicationId;
        internal readonly DateTimeOffset CreationDate;
        
        public BotToken(string token, string hiddenToken, Snowflake applicationId, DateTimeOffset creationDate)
        {
            Token = token;
            HiddenToken = hiddenToken;
            ApplicationId = applicationId;
            CreationDate = creationDate;
        }
    }
}