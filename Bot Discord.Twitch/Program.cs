﻿using Bot_Discord.Twitch.Interop;
using Discord;
using Discord.WebSocket;
using Refit;
using System;
using System.Threading.Tasks;

namespace Bot_Discord.Twitch
{
    class Program
    {
		private DiscordSocketClient _client;
		private CommandHandler _handler;

		public static void Main(string[] args)
			   => new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();

			_client.Log += Log;
            #region login token HIDE
            await _client.LoginAsync(TokenType.Bot, "*token*");
            #endregion
            await _client.StartAsync();
			await _client.SetGameAsync("Marabouter Freyja");
			_handler = new CommandHandler(_client);

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

        private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
