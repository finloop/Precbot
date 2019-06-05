using System;
using Bot.Modules;
using Bot.Database.SQLite;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bot.Database;
using System.Threading;

namespace Bot
{
    public class Program
    {
        static void Main(string[] args)
        {
            using(var db = new StreamsContext())
            {
                //DoSomeWork(db);
                //db.SaveChanges();
                /*foreach(var stream in db.Streams.ToList())
                {
                    stream.PointsCommand = "beczki";
                    stream.PointsName = "beczek";
                    db.SaveChanges();
                }*/
                //Console.WriteLine(Extensions.CheckIfStreamExists(db, "Preclak"));
                // update 'User' set Name = "lordozopl" where Name = "Lordozopl"
                List<string> channels = db.Streams.Where(x => x.channelName != "").Select(p => p.channelName).ToList();
                Config.Read();

                IrcClient irc = new IrcClient(ConfigParams.ip, ConfigParams.port, ConfigParams.userName, ConfigParams.TwitchAuth, channels);
                while(true){
                    string msg = irc.ReadMessage();
                    Console.WriteLine(msg);
                    CommandsHandler.MessageHandler(irc, msg);
                    Thread.Sleep(50);
                }
                //Console.WriteLine(ConfigParams.Debug);
                //Console.WriteLine(Extensions.GetUserPoints(db, "Gragasgoesgym", "Lordozopl"));
                //Console.WriteLine(Extensions.GetUserTotalPoints(db, "Gragasgoesgym", "Kappa"));
                //Console.WriteLine(Extensions.GetUserTotalPoints(db, "Gragasgoesgym", "Gragasgoesgym"));
            }
        }
        public static void DoSomeWork(StreamsContext db) 
        {   
            var user1 = new User() {
                Name = "lordozopl",
                Points = 500,
                TotalPoints = 1000
            };

            var user2 = new User() {
                Name = "preclak",
                Points = 600,
                TotalPoints = 2000
            };

            var user3 = new User() {
                Name = "gragasgoesgym",
                Points = 700,
                TotalPoints = 3000
            };

            db.Streams.Add(new Stream() {
                PointsName = "Beczki",
                channelName = "gragasgoesgym",
                LastGiveaway = DateTime.UtcNow,
                Users = new List<User>() {user1, user2, user3}
            });

        }
    }

    
}
