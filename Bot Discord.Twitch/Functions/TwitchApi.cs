using Bot_Discord.Twitch.Interop;
using Bot_Discord.Twitch.Models;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch.Functions
{
    class TwitchApi : ModuleBase<SocketCommandContext>
    {
        private static TwitchApi instance = null;
        private ITwitchApiService twitchApiService;
        private GameList gameinfo;
        private SocketGuild eynwaGuild;

        private bool isEithingStream;
        private bool isFrieyjaStream;
        private bool isToukanStream;

        private RestUserMessage eithingMessage;
        private RestUserMessage frieyjaMessage;
        private RestUserMessage toukanMessage;

        private TwitchApi()
        {
            twitchApiService = RestService.For<ITwitchApiService>("https://api.twitch.tv/helix");
        }
        public static TwitchApi GetInstance()
        {
            return instance ?? (instance = new TwitchApi());
        }

        public async Task UpdateStream(SocketGuildUser arg2)
        {
            RichGame GameActivity = (RichGame)arg2.Activity;
            var gameName = GameActivity.Name;
            gameinfo = await twitchApiService.GetGameInfos(SystemConstant.AuthorizationTokenForTwitch, gameName);
            if (gameinfo?.Datas.Count > 0)
            {
                SessionParameters parameters = new SessionParameters { BroadcasterLanguage = "fr", GameId = gameinfo?.Datas[0].Id };
                await this.twitchApiService.PatchSessions(parameters, getTwitchToken(arg2.Id), getTwitchBroadcastId(arg2.Id));
            }
        }

        public async Task NotifyDiscordWhenLiveOn(DiscordSocketClient client, SocketGuildUser arg1)
        {
            var chaninfo = await twitchApiService.GetChanelInfos(SystemConstant.AuthorizationTokenForTwitch, getTwitchName(arg1.Id));
            DateTime test = (DateTime)chaninfo.Data.FirstOrDefault().StartedAt;
            if(!GetUserStreamStatus(arg1.Id))
            {
                gameinfo = await twitchApiService.GetGameInfosById(SystemConstant.AuthorizationTokenForEithing, chaninfo.Data.FirstOrDefault().GameId);
                eynwaGuild = client.Guilds.Where(g => g.Id == 248520271357542410).FirstOrDefault();
                var textChan = eynwaGuild.GetTextChannel(SystemConstant.AnnoncesChanId);

                var embed = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Title = chaninfo.Data.FirstOrDefault().Title
                };
                // Or with methods
                embed.AddField("Jeu", gameinfo.Datas.FirstOrDefault().Name)
                    .WithAuthor(eynwaGuild.Users.Where(u => u.Id == arg1.Id).FirstOrDefault())
                    .WithFooter(footer => footer.Text = "Click !!!!")
                    .WithColor(Color.DarkPurple)
                    .WithUrl(getTwitchUrl(arg1.Id))
                    .WithImageUrl(chaninfo.Data.FirstOrDefault().ThumbnailUrl)
                    .WithCurrentTimestamp()
                    .Build();
                
                var embedMessage = await textChan.SendMessageAsync("Hey @everyone, "+ arg1.Nickname + " est en live sur "+ getTwitchUrl(arg1.Id) + " ! Venez dire bonjour !", embed: embed.Build());
                updateUserMessage(arg1.Id, embedMessage);
                updateStreamStatus(arg1.Id, true);
            }
        }
        public async Task StopStream(SocketGuildUser arg2)
        {
            updateStreamStatus(arg2.Id, false);
            DeleteUserMessage(arg2.Id);
        }

        private string getTwitchUrl(ulong DiscordId)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    return SystemConstant.TwitchUrldEithing;
                case SystemConstant.DiscordIdFrieyja:
                    return SystemConstant.TwitchUrldFrieyja;
                case SystemConstant.DiscordIdToukan:
                    return SystemConstant.TwitchUrldToukan;
                default:
                    return "";
            }
        }
        private string getTwitchName(ulong DiscordId)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    return SystemConstant.TwitchNameEithing;
                case SystemConstant.DiscordIdFrieyja:
                    return SystemConstant.TwitchNameFrieyja;
                case SystemConstant.DiscordIdToukan:
                    return SystemConstant.TwitchNameToukan;
                default:
                    return "";
            }
        }
        private string getTwitchToken(ulong DiscordId)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    return SystemConstant.AuthorizationTokenForEithing;
                case SystemConstant.DiscordIdFrieyja:
                    return SystemConstant.AuthorizationTokenForFrieyja;
                case SystemConstant.DiscordIdToukan:
                    return SystemConstant.AuthorizationTokenForToukan;
                default:
                    return "";
            }
        }
        private string getTwitchBroadcastId(ulong DiscordId)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    return SystemConstant.BroadcastIdForEithing;
                case SystemConstant.DiscordIdFrieyja:
                    return SystemConstant.BroadcastIdForFrieyja;
                case SystemConstant.DiscordIdToukan:
                    return SystemConstant.BroadcastIdForToukan;
                default:
                    return "";
            }
        }
        private void updateStreamStatus(ulong DiscordId, bool status)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    isEithingStream = status;
                    break;
                case SystemConstant.DiscordIdFrieyja:
                    isFrieyjaStream = status;
                    break;
                case SystemConstant.DiscordIdToukan:
                    isToukanStream = status;
                    break;
            }
        }
        private void updateUserMessage(ulong DiscordId, RestUserMessage message)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    eithingMessage = message;
                    break;
                case SystemConstant.DiscordIdFrieyja:
                    frieyjaMessage = message;
                    break;
                case SystemConstant.DiscordIdToukan:
                    toukanMessage = message;
                    break;
            }
        }
        private void DeleteUserMessage(ulong DiscordId)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    eithingMessage?.DeleteAsync();
                    break;
                case SystemConstant.DiscordIdFrieyja:
                    frieyjaMessage?.DeleteAsync();
                    break;
                case SystemConstant.DiscordIdToukan:
                    toukanMessage?.DeleteAsync();
                    break;
            }
        }
        private bool GetUserStreamStatus(ulong DiscordId)
        {
            switch (DiscordId)
            {
                case SystemConstant.DiscordIdEithing:
                    return isEithingStream;
                case SystemConstant.DiscordIdFrieyja:
                    return isFrieyjaStream;
                case SystemConstant.DiscordIdToukan:
                    return isToukanStream;
                default:
                    return true;
            }
        }
    }
}
