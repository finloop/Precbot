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
            try
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
                        msg = msg.ToLower();


                        if (msg.StartsWith("!"))
                        {
                            List<string> s_msg = new List<string>(msg.Split(" "));
                            switch (s_msg.Count)
                            {
                                // Show your points depending on what was send
                                case (1):
                                    s_msg[0] = s_msg[0].Replace("!", "");
                                    if (s_msg[0] == "points")
                                    {
                                        Points.UserPointsOnThisChannel(db, irc, channel, sender, msg);
                                    }
                                    else
                                    //TODO: swap this to list s_msg[0] in listofPOintscommands
                                    if (s_msg[0] == "precelki")
                                    {
                                        Points.UserPointsOnOtherChannel(db, irc, channel, sender, msg, s_msg[0]);
                                    }
                                    else
                                    if (s_msg[0] == "beczki")
                                    {
                                        Points.UserPointsOnOtherChannel(db, irc, channel, sender, msg, s_msg[0]);
                                    }
                                    else
                                    if (s_msg[0] == "watchtime")
                                    {
                                        Utils.GetUserWatchtime(db, irc, channel, sender, channel);
                                    }
                                    else
                                    if (s_msg[0] == "viewers")
                                    {
                                        Utils.GetUsersOnChannel(irc, channel);
                                    }
                                    else
                                    if (s_msg[0] == "widzowie")
                                    {
                                        Utils.GetUsersOnChannel(irc, channel);
                                    }
                                    else
                                    if (s_msg[0] == "matma")
                                    {
                                        irc.SendPublicChatMessage(channel, "MingLee https://clips.twitch.tv/AwkwardCleanKiwiHoneyBadger");
                                    }
                                    else
                                    if (s_msg[0] == "halo")
                                    {
                                        irc.SendPublicChatMessage(channel, "Cześć " + sender + " KappaPride");
                                    }
                                    else
                                    if (s_msg[0] == "salto")
                                    {
                                        irc.SendPublicChatMessage(channel, "https://clips.twitch.tv/DirtyGleamingKiwiHassanChop 4Head");
                                    }
                                    else
                                    if (s_msg[0] == "bot3")
                                    {
                                        irc.SendPublicChatMessage(channel, "@" + sender + " wcale nie jestem botem v3 Keepo");
                                    }
                                    else
                                    if (s_msg[0] == "monika")
                                    {
                                        irc.SendPublicChatMessage(channel, "<3 YuriComfy <3 > Monika LUL");
                                    }
                                    else
                                    if (s_msg[0] == "znikam")
                                    {
                                        irc.SendPublicChatMessage(channel, "/timeout " + sender + " 10");
                                    }
                                    else
                                    if (s_msg[0] == "up")
                                    {
                                        Utils.CheckStreamUptime(irc, channel);
                                    }
                                    else
                                    if (s_msg[0] == "uptime")
                                    {
                                        Utils.CheckStreamUptime(irc, channel);
                                    }
                                    break;

                                // Check someones elses points
                                case (2):
                                    s_msg[0] = s_msg[0].Replace("!", "");
                                    if (s_msg[0] == "points")
                                    {
                                        Points.UserPointsOnThisChannel(db, irc, channel, s_msg[1], msg);
                                    }
                                    else
                                    //TODO: swap this to list s_msg[0] in listofPOintscommands
                                    if (s_msg[0] == "precelki")
                                    {
                                        Points.UserPointsOnOtherChannel(db, irc, channel, s_msg[1], msg, s_msg[0]);
                                    }
                                    else
                                    if (s_msg[0] == "beczki")
                                    {
                                        Points.UserPointsOnOtherChannel(db, irc, channel, s_msg[1], msg, s_msg[0]);
                                    }
                                    else
                                    if (s_msg[0] == "watchtime")
                                    {
                                        Utils.GetUserWatchtime(db, irc, channel, s_msg[1], channel);
                                    }
                                    else
                                    // ruletka amount
                                    if (s_msg[0] == "ruletka")
                                    {
                                        Points.Roulette(db, irc, channel, sender, s_msg[1]);
                                    }
                                    break;
                                case (3):
                                    s_msg[0] = s_msg[0].Replace("!", "");
                                    if (s_msg[0] == "donejt")
                                    {
                                        Points.GivePointsFromSenderToReciv(db, irc, channel, sender, s_msg[1], s_msg);
                                    }
                                    else
                                    if (s_msg[0] == "donate")
                                    {
                                        Points.GivePointsFromSenderToReciv(db, irc, channel, sender, s_msg[1], s_msg);
                                    }
                                    else
                                    // watchtime who where
                                    if (s_msg[0] == "watchtime")
                                    {
                                        Utils.GetUserWatchtime(db, irc, channel, s_msg[1], s_msg[2]);
                                    }
                                    break;
                            }
                        }

                    }
                    db.SaveChanges();
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}