using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Bot.Database.SQLite {
    public class StreamsContext : DbContext 
    {
        public DbSet<Stream> Streams {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=streams.db");
        }
    }

    public class Stream 
    {
        public int StreamId {get;set;}
        public List<User> Users {get;set;}
        public string PointsName {get;set;}
        public DateTime LastGiveaway {get;set;}
        public string channelName {get;set;}
        
    }

    public class User 
    {
        public int UserId {get;set;}
        public string Name {get; set;}
        public long Points {get;set;}
        public long TotalPoints {get;set;}
        public TimeSpan TotalTimeSpend {get;set;}
        public DateTime LastSeen {get;set;}

    }
}