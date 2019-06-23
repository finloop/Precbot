using System;
using System.Collections.Generic;
using Bot.Database.SQLite;
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
        public static void BackgroundWorker5min()
        {
            while (true)
            {
                Console.WriteLine($"____________Trying to add points... + {DateTime.Now.ToShortTimeString()}");
                //TODO: get channels from db
                AddPointsIfStreamIsRunning("preclak");
                AddPointsIfStreamIsRunning("gragasgoesgym");
                Thread.Sleep(1000 * 60 * 5);
            }
        }
        public static void AddPointsIfStreamIsRunning(string channel)
        {
            List<string> viewers = Chatters.GetViewers(channel);
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
                        stream.Users.Add(user);
                    }

                }
                db.SaveChanges();
            }
        }
    }
}