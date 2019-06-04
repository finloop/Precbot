using System;
using Bot.Modules;
using Bot.Database.SQLite;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bot.Database;

namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var db = new StreamsContext())
            {
                DoSomeWork(db);
                db.SaveChanges();
                foreach(var stream in db.Streams.ToList())
                    Console.WriteLine(stream.channelName);
                Console.WriteLine(Extensions.CheckIfStreamExists(db, "Gragas"));
            }
        }
        public static void DoSomeWork(StreamsContext db) 
        {
            db.Streams.Add(new Stream() {
                PointsName = "Precelki",
                channelName = "Preclak"
            });
        }
    }

    
}
