using System;
using System.Collections.Generic;
using Bot.Database.SQLite;
using Bot.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Bot.Modules.Commands
{
    public class Points
    {
        public void CheckYourPoints(StreamsContext db, IrcClient irc, string channel, string sender, string msg)
        {
            if(Extensions.CheckIfStreamExists(db, channel))
            {
                int userId = Extensions.GetUserIndex(db, channel, sender);
                if(userId != -1)
                {
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"Points: {sender} was found in this channel.");
                    var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                    //TODO: Add phonetic name for points.
                    irc.SendPublicChatMessage(channel, $"{sender} posiada {stream.Users[userId]} {stream.PointsName}");
                }
                else
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, "Points: user was not found in this channel.");
            } else
            {
                if(ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, "Points: stream was not found in this channel..");
            }
        }
        public void CheckSomeonesPoints(StreamsContext db, IrcClient irc, string channel, string sender, string msg)
        {
            if(Extensions.CheckIfStreamExists(db, channel))
            {
                int userId = Extensions.GetUserIndex(db, channel, sender);
                if(userId != -1)
                {
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"Points: {sender} was found in this channel.");
                    var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                    //TODO: Add phonetic name for points.
                    irc.SendPublicChatMessage(channel, $"{sender} posiada {stream.Users[userId]} {stream.PointsName}");
                }
                else
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"Points: {msg.Split(' ')[1]} was not found in this channel.");
            } else
            {
                if(ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, "Points: stream was not found in this channel..");
            }
        }
    }
}