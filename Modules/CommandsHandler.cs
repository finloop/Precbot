using System;
using System.Collections.Generic;
using Bot.Database.SQLite;
using Bot.Database;
using Bot.Modules.Commands;
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
                                Points.UserPointsOnThisChannel(db, irc, channel, sender, msg);
                            } else
                            //TODO: swap this to list s_msg[0] in listofPOintscommands
                            if (s_msg[0] == "precelki")
                            {
                                Points.UserPointsOnOtherChannel(db, irc, channel, sender, msg, s_msg[0]);
                            } else 
                            if(s_msg[0] == "beczki")
                            {
                                Points.UserPointsOnOtherChannel(db, irc, channel, sender, msg, s_msg[0]);
                            }
                            break;
                    }
                }

            }
        }
    }
}