using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using conductorbot.Services;
using Discord.WebSocket;
using conductorbot.Functions;

namespace conductorbot.RoleCommands
{
    public class SetColor : ModuleBase<SocketCommandContext>
    {
        private List<string> ColorRoles = new List<string>
        {
            "[red]", "[orange]", "[yellow]", "[green]", "[blue]", "[purple]"
        };

        [Command("setcolor")]
        public async Task SetColorCommand(string color)
        {
            SocketGuildUser user = Context.Guild.Users.Where(c => c.Id == Context.User.Id).First();
            MessageHandler handler = new MessageHandler();

            if (!CheckPermissions.CheckUserInChannel(user, Context.Channel as SocketTextChannel, "changing-room"))
            {
                await ReplyAsync("", false, handler.BuildEmbed("uwu Oopsie whoopsie", $"I'm sowwy :3 but it wooks wike you haw to call thish command fwom <#422886537068412929> owo").Build());
                return;
            }

            if (color == "none")
            {
                await user.RemoveRolesAsync(Context.Guild.Roles.Where(c => ColorRoles.Contains(c.Name)));
                await ReplyAsync("", false, handler.BuildEmbed("Removed color", $"Not so shiny.").Build());

                Debugging.Log("SetColorCommand", $"Removed colors for {user.Username}");
                return;
            }

            color = $"[{color}]";
            if (ColorRoles.Contains(color))
            {
                await user.RemoveRolesAsync(Context.Guild.Roles.Where(c => ColorRoles.Contains(c.Name)));
                await user.AddRoleAsync(Context.Guild.Roles.Where(c => c.Name == color).First());
                await ReplyAsync("", false, handler.BuildEmbed("New color set!", $"Shiny.").Build());

                Debugging.Log("SetColorCommand", $"Set new color for {user.Username}");
                return;
            }
            else
            {
                await ReplyAsync("", false, handler.BuildEmbed("Color doesn't exist", $"Try one of these: `red`, `orange`, `yellow`, `green`, `blue`, `purple` or `none` to reset").Build());
                return;
            }
        }
    }
}
