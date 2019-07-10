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
            optionsBuilder.UseNpgsql("Host=localhost;Database=Bot;Username=postgres;Password=mysecretpassword;Port=5433");
        }
    }
}