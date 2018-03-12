using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using conductorbot;
using System.Timers;
using conductorbot.Services;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace conductorbot.Functions
{
    public class Arrival
    {

        static List<string> usernames = new List<string>();
        public static System.Timers.Timer timer = new System.Timers.Timer
        { AutoReset = true, Interval = 900000, Enabled = true, };
        static Random rand = new Random();
        

        public static Task TrainPassengers(SocketGuildUser arg)
        {
            usernames.Add(arg.Username);
            return Task.CompletedTask;
        }
        public static Task TrainArrived(List<string> passengers)
        {
            SocketTextChannel channel = Settings._client.GetGuild(422532116040122372).TextChannels.Where(c => c.Name == "arrivals").FirstOrDefault();
            EmbedBuilder embed = new EmbedBuilder();
            string s = "s";
            string s2 = "all step";
            string passengersNames = "";
            if (passengers.Count > 0)
            {
                foreach (var passenger in passengers)
                {
                    passengersNames += $"**{passenger}**, ";
                }
                if (passengers.Count == 1)
                {
                    passengersNames.Replace(',', ' ');
                    s = "";
                    s2 = "steps";
                }
                embed.WithTitle($"A new train has arrived on __PLATFORM 0__, carrying {passengers.Count} passenger{s}");
                embed.WithDescription(passengersNames + $"{s2} off the train onto the platform.");
                embed.WithColor(Palette.pink);
                channel.SendMessageAsync("", false, embed.Build());
            }
            else
            {
                Debugging.Log("TrainArrived", "A train arrived, but no passengers were on board. No message was sent.");
            }
            passengers.Clear();
            return Task.CompletedTask;
        }

        internal static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            TrainArrived(usernames);
        }
    }
}
