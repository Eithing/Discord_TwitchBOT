using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch.Models.Twitch.ChanelInfos
{
    public class TwitchChanelsInfos
    {
        [JsonProperty("broadcaster_language")]
        public string BroadcasterLanguage { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("is_live")]
        public bool IsLive { get; set; }
        [JsonProperty("tag_ids")]
        public List<string> TagIds { get; set; }
        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("started_at")]
        public object StartedAt { get; set; }
    }
}
