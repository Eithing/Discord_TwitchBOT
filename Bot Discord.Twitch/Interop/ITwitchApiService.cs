
using Bot_Discord.Twitch.Models;
using Bot_Discord.Twitch.Models.Twitch;
using Refit;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch.Interop
{
    //Client-Id = identification de l'app sur twitch developer
    [Headers("Client-Id:" + "l018hupxji48bv3mtxjkj38ibh468j", "Content-Type:" + "application/json")]
    public interface ITwitchApiService
    {
        [Patch("/channels?broadcaster_id={broadcastId}")]
        Task PatchSessions([Body] SessionParameters param, [Header("Authorization")] string token, string broadcastId);

        [Get("/games?name={name}")]
        Task<GameList> GetGameInfos([Header("Authorization")] string token, string name);

        [Get("/games?id={id}")]
        Task<GameList> GetGameInfosById([Header("Authorization")] string token, string id);

        [Get("/search/channels?query={name}")]
        Task<ChanelInfo> GetChanelInfos([Header("Authorization")] string token, string name);
    }
}
