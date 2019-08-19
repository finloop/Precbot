using System;
using System.Collections.Generic;
namespace Bot.Database {
    public class Stream 
    {
        public int StreamId {get;set;}
        public List<User> Users {get;set;}
        public string PointsName {get;set;}
        public string PointsCommand {get;set;}
        public DateTime LastGiveaway {get;set;}
        public string channelName {get;set;}
        public long giveaway_pool {get;set;}
        public string giveaway_users {get;set;}
        public DateTime LastLive {get;set;}
        public string sub_raffle_users {get;set;}
        public long sub_raffle_points {get;set;}
        public long points_limit {get;set;}
        public TimeSpan points_msg_timeout {get;set;}
        public string sub_raffle_info {get;set;}
    }

    public class User 
    {
        public int UserId {get;set;}
        public string Name {get; set;}
        public long Points {get;set;}
        public long TotalPoints {get;set;}
        public TimeSpan TotalTimeSpend {get;set;}
        public DateTime LastSeen {get;set;}
        public string Attacker {get;set;}
        public long pool {get;set;}
        public DateTime LastMsgTime {get;set;}
        public DateTime LasSubRaffleWon {get;set;}
    }
}