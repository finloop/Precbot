using System;
using System.Collections.Generic;
using Bot.Database.SQLite;
using Bot.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Bot.Modules.Commands
{
    public static class Points
    {
        //TODO: Find a better name for this like 'for' instead of on...
        public static void UserPointsOnThisChannel(StreamsContext db, IrcClient irc, string channel, string user, string msg)
        {
            if(Extensions.CheckIfStreamExists(db, channel))
            {
                int userId = Extensions.GetUserIndex(db, channel, user);
                if(userId != -1)
                {
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"UserPointsOnThisChannel: {user} was found in this channel.");
                    var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                    irc.SendPublicChatMessage(channel, $"{user} posiada {stream.Users[userId].Points} {stream.PointsName}");
                }
                else
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, "UserPointsOnThisChannel: user was not found in this channel.");
            } else
            {
                if(ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, "UserPointsOnThisChannel: stream was not found in this channel..");
            }
        }
        public static void UserPointsOnOtherChannel(StreamsContext db, IrcClient irc, string channel, string sender, string msg, string pointsCommand)
        {
            var streams = db.Streams.Where(x => x.PointsCommand.Equals(pointsCommand)).Include(x => x.Users);
            if(streams.Count() >= 1)
            {
                var stream = streams.First();
                int userId = Extensions.GetUserIndex(db, stream.channelName, sender);
                if(userId != -1)
                {
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"UserPointsOnOtherChannel: {sender} was found in {stream.channelName}.");
                    irc.SendPublicChatMessage(channel, $"{sender} posiada {stream.Users[userId].Points} {stream.PointsName}");
                }
                else
                    if(ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"UserPointsOnOtherChannel: user was not found in {stream.channelName}.");
            } else
            {
                if(ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"UserPointsOnOtherChannel: stream with {pointsCommand} was not found ..");
            }
        }
        public static void CheckSomeonesPoints(StreamsContext db, IrcClient irc, string channel, string sender, string msg)
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
                    irc.SendPublicChatMessage(channel, $"{sender} posiada {stream.Users[userId].Points} {stream.PointsName}");
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