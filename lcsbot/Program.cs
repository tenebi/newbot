﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using conductorbot.Services;
using Discord.Rest;

namespace conductorbot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private Random rand = new Random();

        public async Task RunBotAsync()
        {
            Settings._client = new DiscordSocketClient(Settings._config);
            Settings._commands = new CommandService();

            Settings._services = new ServiceCollection()
                .AddSingleton(Settings._client)
                .AddSingleton(Settings._commands)
                .BuildServiceProvider();

            Settings.LoadJson();
            Debugging.Log("TODO", "make it so functions that use db check if db is enabled in settings");

            //subs
            Settings._client.Log += Debugging.Log;
            Settings._commands.Log += Debugging.Log;
            Settings._client.MessageUpdated += MessageUpdated;
            Settings._client.UserJoined += lcsbot.Functions.Arrival.TrainPassengers;
            lcsbot.Functions.Arrival.timer.Elapsed += lcsbot.Functions.Arrival.TimerElapsed;

            await RegisterCommandAsync();
            await Settings._client.LoginAsync(TokenType.Bot, Settings.BotToken);
            await Settings._client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task RegisterCommandAsync()
        {
            Settings._client.MessageReceived += HandleCommandAsync;

            await Settings._commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            MessageHandler handler = new MessageHandler();
            RestUserMessage message1;
            var message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot) return;

            int argPos = 0;
            if(!message.HasStringPrefix("//", ref argPos) && message.Channel.Name == "ticket-station")
            {
                await message.DeleteAsync();
            }
            if ((message.HasStringPrefix("//", ref argPos) || message.HasMentionPrefix(Settings._client.CurrentUser, ref argPos)) && !CheckPrivate(message))
            {
                var context = new SocketCommandContext(Settings._client, message);


                Debugging.Log("Command Handler", $"{context.User.Username} called {message}");

                var result = await Settings._commands.ExecuteAsync(context, argPos, Settings._services);
                if (!result.IsSuccess && result.Error != CommandError.ObjectNotFound || result.Error != CommandError.Exception || result.ErrorReason != "The server responded with error 400: BadRequest")
                {
                    Debugging.Log("Command Handler", $"Error with command {message}: {result.ErrorReason.Replace(".", "")}", LogSeverity.Warning);

                    if (result.ErrorReason == "Invalid context for command; accepted contexts: DM")
                        await arg.Channel.SendMessageAsync("", false, handler.BuildEmbed(":no_entry_sign:  ERROR!", "Try PMing that command instead.").Build());
                    else
                    {
                        message1 = await arg.Channel.SendMessageAsync("", false, handler.BuildEmbed(":no_entry_sign:  Error!", result.ErrorReason).Build());
                        await Task.Delay(5000);
                        await message1.DeleteAsync();
                        await message.DeleteAsync();
                    }

                   
                }
            }

            else if (CheckPrivate(message)) //direct message
            {
                bool AddingToDB = false;
                var context = new SocketCommandContext(Settings._client, message);
                Debugging.Log("Command Handler, DM", $"{context.User.Username} sent {message}");

                var result = await Settings._commands.ExecuteAsync(context, argPos, Settings._services);
                if (!result.IsSuccess && result.Error != CommandError.ObjectNotFound || result.Error != CommandError.Exception)
                {
                    if (AddingToDB)
                    {
                        AddingToDB = false;
                    }
                    else
                    {
                        Debugging.Log("Command Handler, DM", $"Error with command {message}: {result.ErrorReason.Replace(".", "")}", LogSeverity.Warning);
                        await arg.Channel.SendMessageAsync("", false, handler.BuildEmbed(":no_entry_sign:  Error!", result.ErrorReason).Build());
                    }
                }
            }
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var message = await before.GetOrDownloadAsync();
        }

        private bool CheckPrivate(SocketUserMessage message) => message.Channel.ToString().Contains(message.Author.ToString());
    }
}
