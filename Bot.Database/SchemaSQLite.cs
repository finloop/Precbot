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
    //TODO: Add phonetic name for points.
}