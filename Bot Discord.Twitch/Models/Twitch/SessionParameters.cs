using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch.Models
{
    public class SessionParameters
    {
        [JsonProperty("game_id")]
        public string GameId { get; set; }

        [JsonProperty("broadcaster_language")]
        public string BroadcasterLanguage { get; set; }
    }
}
