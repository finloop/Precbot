using System;
using System.Collections.Generic;
using Bot.Database.PostgreSQL;
using Bot.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bot.Modules.Commands.Helpers;
using System.Threading.Tasks;

namespace Bot.Modules.Commands
{
    public static class Utils
    {
        public static void GetUserRank(StreamsContext db, IrcClient irc, string channel, string user, string channelToCheck)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channelToCheck)).Include(x => x.Users);
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                stream.Users.Sort((x, y) => y.Points.CompareTo(x.Points));
                int userId = stream.Users.FindIndex(x => x.Name.Equals(user));
                if (user == "top5" && stream.Users.Count > 5)
                {
                    string userList = "";
                    for (int i = 0; i < 5; i++)
                    {
                        if (i == 4)
                            userList += $"{i + 1}. {stream.Users[i].Name}: {stream.Users[i].Points}.";
                        else
                            userList += $"{i + 1}. {stream.Users[i].Name}: {stream.Users[i].Points}, ";
                    }
                    irc.SendPublicChatMessage(channel, $"Top 5 kanału {channelToCheck}: {userList}");
                }
                else
                if (user == "top3" && stream.Users.Count > 3)
                {
                    string userList = "";
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 2)
                            userList += $"{i + 1}. {stream.Users[i].Name}: {stream.Users[i].Points}.";
                        else
                            userList += $"{i + 1}. {stream.Users[i].Name}: {stream.Users[i].Points}, ";
                    }
                    irc.SendPublicChatMessage(channel, $"Top 5 kanału {channelToCheck}: {userList}");
                }
                else
                if (user == "top10" && stream.Users.Count > 10)
                {
                    string userList = "";
                    for (int i = 0; i < 10; i++)
                    {
                        if (i == 9)
                            userList += $"{i + 1}. {stream.Users[i].Name}: {stream.Users[i].Points}.";
                        else
                            userList += $"{i + 1}. {stream.Users[i].Name}: {stream.Users[i].Points}, ";
                    }
                    irc.SendPublicChatMessage(channel, $"Top 5 kanału {channelToCheck}: {userList}");
                }
                else
                if (userId != -1)
                {
                    if (ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"GetUserRank: {user} was found in {stream.channelName}.");
                    var user_d = stream.Users[userId];

                    irc.SendPublicChatMessage(channel, $"{user} jest {userId + 1} w rankingu.");
                }
                else
                    if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GetUserRank: user was not found in {stream.channelName}.");
            }
            else
            {
                if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GetUserRank: stream with {channel} was not found ..");
            }
        }
        public static void GetUserWatchtime(StreamsContext db, IrcClient irc, string channel, string user, string channelToCheck)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channelToCheck)).Include(x => x.Users);
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                int userId = stream.Users.FindIndex(x => x.Name.Equals(user));
                if (userId != -1)
                {
                    if (ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"GetUserWatchtime: {user} was found in {stream.channelName}.");
                    var user_d = stream.Users[userId];
                    irc.SendPublicChatMessage(channel, $"{user} oglądał {channelToCheck} przez {user_d.TotalTimeSpend.Days} dni {user_d.TotalTimeSpend.Hours} godzin {user_d.TotalTimeSpend.Minutes} minut");
                }
                else
                    if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GetUserWatchtime: user was not found in {stream.channelName}.");
            }
            else
            {
                if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GetUserWatchtime: stream with {channel} was not found ..");
            }
        }
        public static void GetUserLastSeen(StreamsContext db, IrcClient irc, string channel, string user, string channelToCheck)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channelToCheck)).Include(x => x.Users);
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                int userId = stream.Users.FindIndex(x => x.Name.Equals(user));
                if (userId != -1)
                {
                    if (ConfigParams.Debug)
                        irc.SendPublicChatMessage(channel, $"GetUserLastSeen: {user} was found in {stream.channelName}.");
                    var user_d = stream.Users[userId];
                    irc.SendPublicChatMessage(channel, $"{user} był ostatnio na kanale {channelToCheck} {user_d.LastSeen} ({(DateTime.Now - user_d.LastSeen).Days} dni temu)");
                }
                else
                    if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GetUserLastSeen: user was not found in {stream.channelName}.");
            }
            else
            {
                if (ConfigParams.Debug)
                    irc.SendPublicChatMessage(channel, $"GetUserLastSeen: stream with {channel} was not found ..");
            }
        }
        public static void GetUsersOnChannel(IrcClient irc, string channel)
        {
            string v = "Obecni widzowie to:";
            List<string> l = Chatters.GetViewers(channel);
            for (int i = 0; i < l.Count; i++)
            {
                if (i == l.Count - 1)
                    v += l[i] + ". ";
                else
                    v += l[i] + ", ";
            }
            irc.SendPublicChatMessage(channel, v);
        }
        public static void CheckStreamUptime(IrcClient irc, string channel)
        {
            CheckStream.Uptime(irc, channel);
        }
        public static void TryToJoinRaffle(StreamsContext db, IrcClient irc, string channel, string sender)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users);
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                int userId = stream.Users.FindIndex(x => x.Name.Equals(sender));
                if (userId != -1)
                {
                    var user = stream.Users[userId];
                    List<string> raffle_users = stream.sub_raffle_users.Split(';').ToList();
                    if (!raffle_users.Contains(user.Name) && user.Points >= stream.sub_raffle_points)
                    {
                        user.Points -= stream.sub_raffle_points;
                        raffle_users.Add(user.Name);
                        stream.sub_raffle_users = String.Join(";", raffle_users.ToArray());
                        irc.SendPublicChatMessage(channel, $"@{user.Name} zostałeś dodany do losowania :)");
                    }
                }
            }
        }
        public static void ShowRaffleUsers(StreamsContext db, IrcClient irc, string channel, string sender)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channel));
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                string li = stream.sub_raffle_users.Replace(";", ", ");
                irc.SendPublicChatMessage(channel, $"Obecnie zapisani do losowania: {li}");
            }
        }
        public static void ShowRaffleInfo(StreamsContext db, IrcClient irc, string channel, string sender)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channel));
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                irc.SendPublicChatMessage(channel, $"{stream.sub_raffle_info}");
            }
        }

        public static void TryToStartRaffle(StreamsContext db, IrcClient irc, string channel, string sender)
        {
            var streams = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users);
            if (streams.Count() >= 1)
            {
                var stream = streams.First();
                if (sender == stream.channelName || sender == ConfigParams.userName)
                {
                    List<string> raffle_users = stream.sub_raffle_users.Split(';').ToList();
                    int winner = Extensions.getrandom.Next(0, raffle_users.Count);
                    // START RAFFLE
                    irc.SendPublicChatMessage(channel, "UWAGA ZACZYNAM LOSOWANIE");
                    Task.Delay(1000).Wait();
                    irc.SendPublicChatMessage(channel, "5");
                    Task.Delay(1000).Wait();
                    irc.SendPublicChatMessage(channel, "4");
                    Task.Delay(1000).Wait();
                    irc.SendPublicChatMessage(channel, "3");
                    Task.Delay(1000).Wait();
                    irc.SendPublicChatMessage(channel, "2");
                    Task.Delay(1000).Wait();
                    irc.SendPublicChatMessage(channel, "1");
                    Task.Delay(1000).Wait();
                    irc.SendPublicChatMessage(channel, $"@{raffle_users[winner]} wygrał losowanie Pog");

                    stream.sub_raffle_users = "";

                    int userId = stream.Users.FindIndex(x => x.Name.Equals(raffle_users[winner]));
                    if (userId != -1)
                    {
                        var user = stream.Users[userId];
                        user.LasSubRaffleWon = DateTime.Now;
                    }
                }
            }
        }
    }
}