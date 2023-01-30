using Bot_Discord.Twitch.Functions;
using Bot_Discord.Twitch.Models;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        
        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _client.GuildMemberUpdated += _client_GuildMemberUpdated;
            _client.UserJoined += _client_UserJoined; ;

        }

        [Command(RunMode = RunMode.Async)]
        private async Task _client_UserJoined(SocketGuildUser arg)
        {
            if (arg == null) return;
            await Roles.GetInstance().AddRole(Roles.Joueur, arg);
        }

        private async Task _client_GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            if (arg1.Roles?.Where(r => r.Id == 780080878713831424).Count() > 0) // SI Role == Streamer
            {
                switch(arg2.Activity?.Type)
                {
                    case ActivityType.Playing:
                        if(arg1.Id == SystemConstant.DiscordIdEithing || arg1.Id == SystemConstant.DiscordIdToukan)
                            await TwitchApi.GetInstance().UpdateStream(arg2);
                        break;
                    case ActivityType.Streaming:
                        await TwitchApi.GetInstance().NotifyDiscordWhenLiveOn(this._client, arg1);
                        break;
                }

                if(arg1.Activity?.Type == ActivityType.Streaming && arg2.Activity?.Type != ActivityType.Streaming) //si arret du stream
                {
                    await TwitchApi.GetInstance().StopStream(arg2);
                }
            }  
        }
    }
}
