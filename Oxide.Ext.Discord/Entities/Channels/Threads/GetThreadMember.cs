﻿using Oxide.Ext.Discord.Builders;
using Oxide.Ext.Discord.Cache;
using Oxide.Ext.Discord.Interfaces;
using Oxide.Ext.Discord.Libraries.Pooling;

namespace Oxide.Ext.Discord.Entities.Channels.Threads
{
    public class GetThreadMember : IDiscordQueryString
    {
        /// <summary>
        /// Whether to include a guild member object for the thread member
        /// </summary>
        public bool WithMember { get; set; }
        
        ///<inheritdoc/>
        public string ToQueryString()
        {
            if (!WithMember)
            {
                return string.Empty;
            }
            
            QueryStringBuilder builder = QueryStringBuilder.Create(DiscordPool.Internal);
            builder.Add("with_member", StringCache<bool>.Instance.ToString(WithMember));
            return builder.ToStringAndFree();
        }
    }
}