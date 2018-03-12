using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using conductorbot.Services;

namespace conductorbot.Functions
{
    public static class CheckPermissions
    {
        public static bool CheckForRole(SocketGuildUser user, string checkRole)
        {
            foreach (SocketRole role in user.Roles)
            {
                if (role.Name == checkRole)
                    return true;
            }

            return false;
        }

        public static bool CheckUserInChannel(SocketGuildUser user, SocketTextChannel channel, string checkForChannel)
        {
            if (channel.Name == checkForChannel)
                return true;
            else
                return false;
        }
    }
}
