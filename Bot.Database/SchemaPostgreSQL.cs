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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
	        //optionsBuilder.UseLoggerFactory(loggerFactory)  //tie-up DbContext with LoggerFactory object
            //.EnableSensitiveDataLogging().UseNpgsql("");
            optionsBuilder.UseNpgsql("Host=194.182.73.164;Database=Bot_1;Username=precbot;Password=SEvPF7jjqZpQ%T+g;Port=5432");
        }
    }
}