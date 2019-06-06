using System;
using System.Collections.Generic;
using Bot.Database.SQLite;
using Bot.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Bot.Modules.Commands
{
    public static class Utils
    {
        public static void GetUserWatchtime(StreamsContext db,IrcClient irc, string channel, string user)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users);
            if(streams.Count() >= 1)
            {
                var stream = streams.First();
                int userId = stream.Users.FindIndex(x => x.Name.Equals(user));
                if(userId != -1)
                {
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"GetUserWatchtime: {user} was found in {stream.channelName}.");
                    var user_d = stream.Users[userId];
                    irc.SendPublicChatMessage(channel, $"{user} oglądał {channel} przez {user_d.TotalTimeSpend.Hours} godzin {user_d.TotalTimeSpend.Minutes} minut");
                }
                else
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"GetUserWatchtime: user was not found in {stream.channelName}.");
            } else
            {
                if(ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GetUserWatchtime: stream with {channel} was not found ..");
            }
        }
    }
}