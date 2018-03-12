using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using conductorbot.Services;
using Discord.WebSocket;

namespace conductorbot.RoleCommands
{
    [Group("ticket")]
    public class ticket : ModuleBase<SocketCommandContext>
    {
        [Command, RequireOwner]
        public async Task Default()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithAuthor("//conductorbot ticket vending", Settings._client.CurrentUser.GetAvatarUrl());
            embed.AddField("↓ TICKETS HERE ↓", "TICKETS VENDED THROUGH: `//ticket platform<number>`");
            embed.AddInlineField("◄ PLATFORM 1 ►", "__MAIN CHANNELS__\n");
            embed.AddInlineField("◄ PLATFORM 2 ►", "__ANIME CHANNELS__\n");
            embed.AddInlineField("◄ PLATFORM 3 ►", "__ART GALLERY__\n");
            embed.AddInlineField("◄ PLATFORM 4 ►", "__VOICE CHANNELS__\n");
            embed.AddInlineField("◄ PLATFORM 5 ►", "__GAME CHANNELS__");
            embed.AddInlineField("◄ PLATFORM 6 ►", "__STRESS RELIEF__");
            embed.WithColor(Palette.pink);

            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("platform1")]
        public async Task platform1()
        {
            var role = Context.Guild.Roles.Where(c => c.Name == "PLATFORM 1").First();
            SocketGuildUser user = Context.Guild.Users.Where(c => c.Id == Context.User.Id).First();
            await user.AddRoleAsync(role);
            EmbedBuilder successEmbed = new EmbedBuilder()
                .WithAuthor("//conductorbot ticket vending", Settings._client.CurrentUser.GetAvatarUrl())
                .WithDescription("//ticket for **<platform 1>** successfully dispensed. have a nice day.")
                .WithColor(Palette.pink);
            var message = await ReplyAsync("", false, successEmbed.Build());
            await Context.Message.DeleteAsync();
            await Task.Delay(3000);
            await message.DeleteAsync();
        }
        [Command("platform2")]
        public async Task platform2()
        {
            var role = Context.Guild.Roles.Where(c => c.Name == "PLATFORM 2").First();
            SocketGuildUser user = Context.Guild.Users.Where(c => c.Id == Context.User.Id).First();
            await user.AddRoleAsync(role);
            EmbedBuilder successEmbed = new EmbedBuilder()
                .WithAuthor("//conductorbot ticket vending", Settings._client.CurrentUser.GetAvatarUrl())
                .WithDescription("//ticket for **<platform 2>** successfully dispensed. have a nice day.")
                .WithColor(Color.Green);
            var message = await ReplyAsync("", false, successEmbed.Build());
            await Context.Message.DeleteAsync();
            await Task.Delay(5000);
            await message.DeleteAsync();
        }
        [Command("platform3")]
        public async Task platform3()
        {
            var role = Context.Guild.Roles.Where(c => c.Name == "PLATFORM 3").First();
            SocketGuildUser user = Context.Guild.Users.Where(c => c.Id == Context.User.Id).First();
            await user.AddRoleAsync(role);
            EmbedBuilder successEmbed = new EmbedBuilder()
                .WithAuthor("//conductorbot ticket vending", Settings._client.CurrentUser.GetAvatarUrl())
                .WithDescription("//ticket for **<platform 3>** successfully dispensed. have a nice day.")
                .WithColor(Color.Green);
            var message = await ReplyAsync("", false, successEmbed.Build());
            await Context.Message.DeleteAsync();
            await Task.Delay(5000);
            await message.DeleteAsync();
        }
        [Command("platform4")]
        public async Task platform4()
        {
            var role = Context.Guild.Roles.Where(c => c.Name == "PLATFORM 4").First();
            SocketGuildUser user = Context.Guild.Users.Where(c => c.Id == Context.User.Id).First();
            await user.AddRoleAsync(role);
            EmbedBuilder successEmbed = new EmbedBuilder()
                .WithAuthor("//conductorbot ticket vending", Settings._client.CurrentUser.GetAvatarUrl())
                .WithDescription("//ticket for **<platform 4>** successfully dispensed. have a nice day.")
                .WithColor(Color.Green);
            var message = await ReplyAsync("", false, successEmbed.Build());
            await Context.Message.DeleteAsync();
            await Task.Delay(5000);
            await message.DeleteAsync();
        }
        [Command("platform5")]
        public async Task platform5()
        {
            var role = Context.Guild.Roles.Where(c => c.Name == "PLATFORM 5").First();
            SocketGuildUser user = Context.Guild.Users.Where(c => c.Id == Context.User.Id).First();
            await user.AddRoleAsync(role);
            EmbedBuilder successEmbed = new EmbedBuilder()
                .WithAuthor("//conductorbot ticket vending", Settings._client.CurrentUser.GetAvatarUrl())
                .WithDescription("//ticket for **<platform 5>** successfully dispensed. have a nice day.")
                .WithColor(Color.Green);
            var message = await ReplyAsync("", false, successEmbed.Build());
            await Context.Message.DeleteAsync();
            await Task.Delay(5000);
            await message.DeleteAsync();
        }
        [Command("platform6")]
        public async Task platform6()
        {
            var role = Context.Guild.Roles.Where(c => c.Name == "PLATFORM 6").First();
            SocketGuildUser user = Context.Guild.Users.Where(c => c.Id == Context.User.Id).First();
            await user.AddRoleAsync(role);
            EmbedBuilder successEmbed = new EmbedBuilder()
                .WithAuthor("//conductorbot ticket vending", Settings._client.CurrentUser.GetAvatarUrl())
                .WithDescription("//ticket for **<platform 6>** successfully dispensed. have a nice day.")
                .WithColor(Color.Green);
            var message = await ReplyAsync("", false, successEmbed.Build());
            await Context.Message.DeleteAsync();
            await Task.Delay(5000);
            await message.DeleteAsync();
        }

    }
}
