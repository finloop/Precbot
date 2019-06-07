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
            if (Extensions.CheckIfStreamExists(db, channel))
            {
                int userId = Extensions.GetUserIndex(db, channel, user);
                if (userId != -1)
                {
                    if (ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"UserPointsOnThisChannel: {user} was found in this channel.");
                    var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                    irc.SendPublicChatMessage(channel, $"{user} posiada {stream.Users[userId].Points} {stream.PointsName}");
                }
                else
                    if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, "UserPointsOnThisChannel: user was not found in this channel.");
            }
            else
            {
                if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, "UserPointsOnThisChannel: stream was not found in this channel..");
            }
        }
        public static void UserPointsOnOtherChannel(StreamsContext db, IrcClient irc, string channel, string sender, string msg, string pointsCommand)
        {
            var streams = db.Streams.Where(x => x.PointsCommand.Equals(pointsCommand)).Include(x => x.Users);
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                int userId = stream.Users.FindIndex(x => x.Name.Equals(sender));
                if (userId != -1)
                {
                    if (ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"UserPointsOnOtherChannel: {sender} was found in {stream.channelName}.");
                    irc.SendPublicChatMessage(channel, $"{sender} posiada {stream.Users[userId].Points} {stream.PointsName}");
                }
                else
                    if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"UserPointsOnOtherChannel: user was not found in {stream.channelName}.");
            }
            else
            {
                if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"UserPointsOnOtherChannel: stream with {pointsCommand} was not found ..");
            }
        }
        // !donejt user 100
        public static void GivePointsFromSenderToReciv(StreamsContext db, IrcClient irc, string channel, string sender, string reciv, List<string> msg)
        {
            if (Extensions.CheckIfStreamExists(db, channel))
            {
                var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                int senderId = stream.Users.FindIndex(x => x.Name.Equals(sender));
                int recivId = stream.Users.FindIndex(x => x.Name.Equals(reciv));
                long amount = long.Parse(msg[2]);
                if (senderId != -1)
                {
                    if (recivId != -1)
                    {
                        long senderPoints = stream.Users[senderId].Points;

                        if (senderPoints >= amount && amount > 0)
                        {
                            var sender_user = stream.Users[senderId];
                            var reciv_user = stream.Users[recivId];
                            sender_user.Points -= amount;
                            reciv_user.Points += amount;
                            reciv_user.TotalPoints += amount;
                            irc.SendPublicChatMessage(channel, $"{sender} przekazał {amount} {stream.PointsName} {reciv}");
                        }
                        else
                            if (ConfigParams.Debug)
                            irc.SendPublicChatMessage(channel, $"GivePointsFromSenderToUser: {sender} doesnot have enough points.");

                    }
                    else
                        if (ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"GivePointsFromSenderToUser: {reciv} was not found in this channel.");
                }
                else
                    if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GivePointsFromSenderToUser: {sender} was not found in this channel.");

            }
        }
        public static void Roulette(StreamsContext db, IrcClient irc, string channel, string sender, string amount)
        {
            if (Extensions.CheckIfStreamExists(db, channel))
            {
                var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                int senderId = stream.Users.FindIndex(x => x.Name.Equals(sender));
                if (senderId != -1)
                {
                    long senderPoints = stream.Users[senderId].Points;
                    var sender_user = stream.Users[senderId];
                    long roulette_points = -1;
                    if (amount.Equals("all"))
                    {
                        roulette_points = sender_user.Points;
                    }
                    else
                    if (amount.Contains("%"))
                    {
                        amount = amount.Replace("%", "");
                        int procent = int.Parse(amount);
                        if (procent > 0 && procent <= 100)
                        {
                            float multpler = (float) procent / 100;
                            roulette_points = (long)(sender_user.Points * multpler);
                        }
                    }
                    else
                    {
                        long points = long.Parse(amount);
                        if (points > 0 && sender_user.Points >= points)
                        {
                            roulette_points = points;
                        }
                    }

                    if (roulette_points > 0)
                    {
                        int i = (new Random()).Next(1, 3);
                        if (i == 1)
                        {
                            //WON
                            sender_user.Points += roulette_points;
                            sender_user.TotalPoints += roulette_points;
                            irc.SendPublicChatMessage(channel, $"@{sender} wygrał {roulette_points} {stream.PointsName} PogChamp");
                        }
                        else
                        if (i == 2)
                        {
                            //LOST
                            sender_user.Points -= roulette_points;
                            irc.SendPublicChatMessage(channel, $"@{sender} przegrał {roulette_points} {stream.PointsName} FeelsBadMan");
                        }
                    }


                }
                else
                        if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GivePointsFromSenderToUser: {sender} doesnot have enough points.");

            }
            else
                    if (ConfigParams.Debug)
                irc.SendPublicChatMessage(channel, $"GivePointsFromSenderToUser: {sender} was not found in this channel.");

        }

    }
}