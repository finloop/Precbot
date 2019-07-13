using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Bot.Database.PostgreSQL {
    public class StreamsContext : DbContext 
    {
        public DbSet<Stream> Streams {get;set;}
        public DbSet<User> Users {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=postgres;Database=Bot;Username=Lederman;Password=?R&Xq-5N5W?95P#Q;Port=5432");
        }
    }
}