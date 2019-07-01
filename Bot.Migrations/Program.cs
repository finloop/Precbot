using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bot.Database;

namespace Bot.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var sqlite_db = new Database.SQLite.StreamsContext())
            using(var postgres_db = new Database.PostgreSQL.StreamsContext())
            {
                foreach(var stream in sqlite_db.Streams.Include(x => x.Users).ToList())
                {
                    postgres_db.Streams.Add(stream);
                }
                postgres_db.SaveChanges();
            }
        }
    }
}
