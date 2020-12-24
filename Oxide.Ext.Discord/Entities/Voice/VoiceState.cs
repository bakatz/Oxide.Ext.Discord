﻿using Newtonsoft.Json;
using Oxide.Ext.Discord.Entities.Guilds;

namespace Oxide.Ext.Discord.Entities.Voice
{
    public class VoiceState
    {
        [JsonProperty("guild_id")]
        public string GuildId { get; set; }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
        
        [JsonProperty("member")]
        public GuildMember Member { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("deaf")]
        public bool Deaf { get; set; }

        [JsonProperty("mute")]
        public bool Mute { get; set; }

        [JsonProperty("self_deaf")]
        public bool SelfDeaf { get; set; }

        [JsonProperty("self_mute")]
        public bool SelfMute { get; set; }
        
        [JsonProperty("self_stream")]
        public bool SelfStream { get; set; }
        
        [JsonProperty("self_video")]
        public bool SelfVideo { get; set; }

        [JsonProperty("suppress")]
        public bool Suppress { get; set; }
    }
}
