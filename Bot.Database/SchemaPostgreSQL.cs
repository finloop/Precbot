using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;


namespace Bot.Database.PostgreSQL {
    public class StreamsContext : DbContext 
    {
	public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
              new ConsoleLoggerProvider((_, __) => true, true)
        });
	
        public DbSet<Stream> Streams {get;set;}
        public DbSet<User> Users {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
	        //optionsBuilder.UseLoggerFactory(loggerFactory)  //tie-up DbContext with LoggerFactory object
            //.EnableSensitiveDataLogging().UseNpgsql("Host=194.182.73.164;Database=Bot;Username=Lederman;Password=?R&Xq-5N5W?95P#Q;Port=5432");
            optionsBuilder.UseNpgsql("Host=194.182.73.164;Database=Bot;Username=Lederman;Password=?R&Xq-5N5W?95P#Q;Port=5432");
        }
    }
}