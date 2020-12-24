using Newtonsoft.Json;

namespace Oxide.Ext.Discord.Entities.Messages.Embeds
{
    public class EmbedVideo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }
    }
}