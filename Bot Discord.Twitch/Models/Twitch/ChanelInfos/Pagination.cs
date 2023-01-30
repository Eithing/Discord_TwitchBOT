using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch.Models.Twitch.ChanelInfos
{
    public class Pagination
    {
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
    }
}
