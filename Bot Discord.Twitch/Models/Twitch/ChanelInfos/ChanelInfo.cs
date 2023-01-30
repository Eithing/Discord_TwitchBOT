using Bot_Discord.Twitch.Models.Twitch.ChanelInfos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch.Models.Twitch
{
    public class ChanelInfo
    {
        [JsonProperty("data")]
        public List<TwitchChanelsInfos> Data { get; set; }
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
