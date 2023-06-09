using System;
using Oxide.Ext.Discord.Entities;

namespace Oxide.Ext.Discord.Connections
{
    /// <summary>
    /// Represents the parsed Bot Token data
    /// </summary>
    public class BotTokenData
    {
        /// <summary>
        /// Hidden Token. Used when Displaying
        /// </summary>
        public readonly string HiddenToken;
        
        /// <summary>
        /// Application ID of the token
        /// </summary>
        public readonly Snowflake ApplicationId;
        
        /// <summary>
        /// Creation Date of the token
        /// </summary>
        internal readonly DateTimeOffset CreationDate;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hiddenToken"></param>
        /// <param name="applicationId"></param>
        /// <param name="creationDate"></param>
        public BotTokenData(string hiddenToken, Snowflake applicationId, DateTimeOffset creationDate)
        {
            HiddenToken = hiddenToken;
            ApplicationId = applicationId;
            CreationDate = creationDate;
        }
    }
}