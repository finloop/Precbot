using System;
using Bot.Modules;
using Bot.Database.PostgreSQL;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bot.Database;
using System.Threading;
using System.IO;
using Bot.Modules.Commands;
using System.Threading.Tasks;

namespace Bot
{
    public class Program
    {
        static void Main(string[] args)
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

            //RebuildDatabase();

            Config.Read();
            StartWorkers();
            //Console.WriteLine(".......");
            //var t = Task.Run(() => Workers.SendSingleIrcMsg());
            StartBot();



            //Console.WriteLine(ConfigParams.Debug);
            //Console.WriteLine(Extensions.GetUserPoints(db, "Gragasgoesgym", "Lordozopl"));
            //Console.WriteLine(Extensions.GetUserTotalPoints(db, "Gragasgoesgym", "Kappa"));
            //Console.WriteLine(Extensions.GetUserTotalPoints(db, "Gragasgoesgym", "Gragasgoesgym"));

        }
        public static void StartBot()
        {
            List<string> channels = new List<string>();
            using (var db = new StreamsContext())
            {
                channels = db.Streams.Where(x => x.channelName != "").Select(p => p.channelName).ToList();
            }
            IrcClient irc = new IrcClient(ConfigParams.ip, ConfigParams.port, ConfigParams.userName, ConfigParams.TwitchAuth, channels);
            while (true)
            {
                string msg = irc.ReadMessage();
                Console.WriteLine(msg);
                CommandsHandler.MessageHandler(irc, msg);
                Thread.Sleep(20);
                Console.WriteLine(DateTime.Now.ToString());
            }
        }

        public static void StartWorkers()
        {
            Workers.points_thread = new Thread(new ThreadStart(Workers.BackgroundWorker5min));
            Workers.points_thread.IsBackground = true;
            Workers.points_thread.Start();
        }
        public static void RebuildDatabase()
        {
            // DOESNT BUILD
            /* if (File.Exists("streams.db"))
            {
                File.Delete("streams.db");
                Directory.Delete("Migrations", true);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C dotnet ef migrations add Initial";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                process = new System.Diagnostics.Process();
                startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C dotnet ef database update";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            } */

            List<User> g_users = new List<User>();
            List<User> p_users = new List<User>();
            var lines = File.ReadLines("POINTS.txt");
            foreach (var line in lines)
            {
                // username, points, totalpoints
                var data = line.Split(';');
                var user = new User()
                {
                    Name = data[0],
                    Points = long.Parse(data[1])/2,
                    TotalPoints = long.Parse((data[2]))/2,
                    TotalTimeSpend = new TimeSpan(0, 0, 0),
                    LastSeen = DateTime.Now,
                    Attacker = "",
                    pool = 0
                };
                var user2 = new User()
                {
                    Name = data[0],
                    Points = long.Parse(data[1])/2,
                    TotalPoints = long.Parse((data[2]))/2,
                    TotalTimeSpend = new TimeSpan(0, 0, 0),
                    LastSeen = DateTime.Now,
                    Attacker = "",
                    pool = 0
                };
                g_users.Add(user);
                p_users.Add(user2);
            }
            using (var db = new StreamsContext())
            {
                db.Streams.Add(new Bot.Database.Stream()
                {
                    PointsName = "beczek",
                    channelName = "gragasgoesgym",
                    LastGiveaway = DateTime.Now,
                    Users = g_users,
                    PointsCommand = "beczki",
                    giveaway_pool = 0
                });

                db.Streams.Add(new Bot.Database.Stream()
                {
                    PointsName = "precelków",
                    channelName = "preclak",
                    LastGiveaway = DateTime.Now,
                    Users = p_users,
                    PointsCommand = "precelki",
                    giveaway_pool = 0
                });
                db.SaveChanges();
            }
        }
    }


}
