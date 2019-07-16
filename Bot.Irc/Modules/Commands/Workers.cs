using System;
using System.Collections.Generic;
using Bot.Database.PostgreSQL;
using Bot.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Bot.Modules.Commands.Helpers;

namespace Bot.Modules.Commands
{
    public static class Workers
    {
        public static Thread points_thread;
        private static void SendSingleIrcMsg(string channel = "gragasgoesgym", string msg = "test test test 123")
        {
            Config.Read();
            List<string> channels = new List<string>();
            using (var db = new StreamsContext())
            {
                channels = db.Streams.Where(x => x.channelName != "").Select(p => p.channelName).ToList();
            }
            IrcClient irc = new IrcClient(ConfigParams.ip, ConfigParams.port, ConfigParams.userName, ConfigParams.TwitchAuth, channels);
            irc.SendPublicChatMessage(channel, msg);
        }
        public static void StartGiveawayTimer(string channel)
        {
            Thread.Sleep(45 * 1000);


            using (var db = new StreamsContext())
            {
                var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                // END GIVEAWAY
                List<string> users = new List<string>(stream.giveaway_users.Split(","));
                if(users.Count > 0)
                {

                    Random rnd = new Random();
                    int rand = rnd.Next(0, users.Count);
                    int winner = stream.Users.FindIndex(x => x.Name.Equals(users[rand]));
                    if(winner != -1)
                    {

                        SendSingleIrcMsg(stream.channelName,$"@{stream.Users[winner].Name} wygrał {stream.giveaway_pool} {stream.PointsName} PogChamp");
                        stream.Users[winner].Points += stream.giveaway_pool;
                        stream.Users[winner].TotalPoints += stream.giveaway_pool;
                        stream.giveaway_pool = -1;
                        stream.giveaway_users = "";
                    } 
                }
                db.SaveChanges();
            }
        }
        public static void BackgroundWorker5min()
        {
            while (true)
            {
                Console.WriteLine($"Trying to add points... + {DateTime.Now.ToShortTimeString()}");
                //TODO: get channels from db
                AddPointsIfStreamIsRunning("preclak");
                AddPointsIfStreamIsRunning("gragasgoesgym");
                Thread.Sleep(1000 * 60 * 5);
            }
        }
        public static void AddPointsIfStreamIsRunning(string channel)
        {
            List<string> viewers = Chatters.GetViewers(channel);
            List<User> b_users = new List<User>();
            using (var db = new StreamsContext())
            {
                var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                stream.LastLive = DateTime.Now;
                foreach (string viewer in viewers)
                {
                    int userId = stream.Users.FindIndex(x => x.Name.Equals(viewer));
                    if (userId != -1)
                    {
                        if (CheckStream.isRunning(channel) || ConfigParams.Debug)
                        {
                            stream.Users[userId].Points += 1;
                            stream.Users[userId].TotalPoints += 1;
                            stream.Users[userId].TotalTimeSpend += TimeSpan.FromMinutes(5);
                        }
                        stream.Users[userId].LastSeen = DateTime.Now;
                    }
                    else
                    {
                        var user = new User()
                        {
                            Name = viewer,
                            Points = 1,
                            TotalPoints = 1,
                            TotalTimeSpend = new TimeSpan(0, 0, 0),
                            LastSeen = DateTime.Now,
                            Attacker = "",
                            pool = 0
                        };
                        b_users.Add(user);
                    }

                }
                db.SaveChanges();
            }
            using (var db = new StreamsContext())
            {
                var stream = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).First();
                int startIndex = stream.Users.Max(x => x.UserId) + 1;
                for(int i = 0; i < b_users.Count; i++)
                {
                    b_users[i].UserId = startIndex + i;
                    stream.Users.Add(b_users[i]);
                }
                db.SaveChanges();
            }
        }
    }
}