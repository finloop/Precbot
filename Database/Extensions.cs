using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using Bot.Database.SQLite;

namespace Bot.Database
{
    // TODO: Find a better name for this class
    public static class Extensions
    {
        // TODO: Add fun: AddPoints, RemovePoints, AddPointsToEverone, 
        public static bool CheckIfStreamExists(StreamsContext db, string channelName)
        {
            var li = db.Streams.Where(x => x.channelName.Equals(channelName)).ToList();
            if (li.Count == 0)
                return false;
            return true;
        }

        // Returns index of a user, -1 if index not found
        public static int GetUserIndex(StreamsContext db, string channelName, string username)
        {
            var stream = db.Streams.Where(x => x.channelName.Equals(channelName)).First();
            int userIndex = stream.Users.FindIndex(x => x.Name.Equals(username));
            return userIndex;
        }

        public static long GetUserPoints(StreamsContext db, string channelName, string username)
        {
            var stream = db.Streams.Where(x => x.channelName.Equals(channelName)).First();
            int userIndex = stream.Users.FindIndex(x => x.Name.Equals(username));
            return stream.Users[userIndex].Points;
        }
        public static long GetUserTotalPoints(StreamsContext db, string channelName, string username)
        {
            var stream = db.Streams.Where(x => x.channelName.Equals(channelName)).First();
            int userIndex = stream.Users.FindIndex(x => x.Name.Equals(username));
            return stream.Users[userIndex].TotalPoints;
        }
    }
}