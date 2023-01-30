using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch.Models
{
    public class GameList
    {
        [JsonProperty("data")]
        public List<GameInfo> Datas { get; set; }
    }
}
