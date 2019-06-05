using System;
using System.Collections.Generic;
using Bot.Database.SQLite;
using Bot.Database;
using System.Linq;

namespace Bot.Modules
{
    public static class CommandsHandler
    {
        public static void MessageHandler(IrcClient irc, string message)
        {
            using (var db = new StreamsContext())
            {

                if (message.Contains("PRIVMSG"))
                {

                    List<string> vs = new List<string>();
                    int intIndexParseSign = message.IndexOf('!');
                    vs.Add(message.Substring(1, intIndexParseSign - 1));
                    intIndexParseSign = message.IndexOf(" :");
                    int len = intIndexParseSign - message.IndexOf("#") - 1;
                    vs.Add(message.Substring(message.IndexOf("#") + 1, len));
                    vs.Add(message.Substring(intIndexParseSign + 2));

                    // returns [sender, channel, message]

                    string sender = vs[0];
                    string channel = vs[1];
                    string msg = vs[2];

                    List<string> s_msg = new List<string>(msg.Split(" "));

                    switch (s_msg.Count)
                    {
                        // Show your points depending on what was send
                        case (1):
                            s_msg[0] = s_msg[0].Replace("!", "");
                            if (s_msg[0] == "points")
                            {
                                if (Extensions.GetUserIndex(db, channel, sender) != -1)
                                {
                                    var stream = db.Streams.Where(x => x.channelName.Equals(channel)).First();
                                    long points = Extensions.GetUserPoints(db, channel, sender);
                                    irc.SendPublicChatMessage(channel, $"{sender} posiada {points} {stream.PointsName}.");
                                }

                                else
                                    if (ConfigParams.Debug)
                                    irc.SendPublicChatMessage(channel, $"Points: Case(1): User {sender} not found in {channel}");
                            }

                            break;
                    }
                }

            }
        }
    }
}