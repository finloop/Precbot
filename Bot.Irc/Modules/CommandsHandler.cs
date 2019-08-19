using System;
using System.Collections.Generic;
using Bot.Database.PostgreSQL;
using Bot.Database;
using Bot.Modules.Commands;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                        msg = msg.ToLower().Replace("@", "");



                        if (msg.StartsWith("!"))
                        {
                            msg = msg.Replace("!", "");
                            List<string> s_msg = new List<string>(msg.Split(" "));
                            switch (s_msg.Count)
                            {
                                // Show your points depending on what was send
                                case (1):
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
                                    if (s_msg[0] == "pointsall")
                                    {
                                        Points.UserTotalPointsOnThisChannel(db, irc, channel, sender, msg);
                                    }
                                    else
                                    ////
                                    if (s_msg[0] == "precelkiall")
                                    {
                                        Points.UserTotalPointsOnOtherChannel(db, irc, channel, sender, msg, s_msg[0]);
                                    }
                                    else
                                    if (s_msg[0] == "beczkiall")
                                    {
                                        Points.UserTotalPointsOnOtherChannel(db, irc, channel, sender, msg, s_msg[0]);
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
                                    else
                                    if (s_msg[0] == "lastseen")
                                    {
                                        Utils.GetUserLastSeen(db, irc, channel, sender, channel);
                                    }
                                    else
                                    if (s_msg[0] == "join")
                                    {
                                        Points.TryToEnterGiveaway(db, irc, channel, sender, msg);
                                    }
                                    else
                                    if (s_msg[0] == "ranking")
                                    {
                                        Utils.GetUserRank(db, irc, channel, sender, channel);
                                    }
                                    else
                                    if (s_msg[0] == "help")
                                    {
                                        irc.SendPublicChatMessage(channel, "!help [komenda] - użyj aby otrzymać jej opis. Dostępne: !points, !precelki, " +
                                        "!beczki, !watchtime, !widzowie, !matma, !halp, !salto, !bot3, !monika, !znikam, !uptime, !lastseen, !ruletka, !giveaway, !join, !donejt, !ranking, !pointsall" +
                                        " !precelkiall, !beczkiall, !losowanie");
                                    }
                                    if (s_msg[0] == "losowanie")
                                    {
                                        var streams = db.Streams.Where(x => x.channelName.Equals(channel));
                                        if (streams.Count() >= 1)
                                        {
                                            var stream = streams.First();
                                            irc.SendPublicChatMessage(channel, $"Weź udział w losowaniu nagród! Wpisz '!losowanie dołącz' aby wziąć udział. Koszt: {stream.sub_raffle_points} {stream.PointsName}.");
                                        }
                                        
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
                                    if (s_msg[0] == "pointsall")
                                    {
                                        Points.UserTotalPointsOnThisChannel(db, irc, channel, s_msg[1], msg);
                                    }
                                    else
                                    ////////
                                    if (s_msg[0] == "precelkiall")
                                    {
                                        Points.UserTotalPointsOnOtherChannel(db, irc, channel, s_msg[1], msg, s_msg[0]);
                                    }
                                    else
                                    if (s_msg[0] == "beczkiall")
                                    {
                                        Points.UserTotalPointsOnOtherChannel(db, irc, channel, s_msg[1], msg, s_msg[0]);
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
                                        Points.Roulette(db, irc, channel, sender, s_msg[1], channel);
                                    }
                                    else
                                    if (s_msg[0] == "lastseen")
                                    {
                                        Utils.GetUserLastSeen(db, irc, channel, s_msg[1], channel);
                                    }
                                    else
                                    if (s_msg[0] == "giveaway")
                                    {
                                        Points.TryToStartGiveaway(db, irc, channel, sender, msg);
                                    }
                                    else
                                    if (s_msg[0] == "ranking")
                                    {
                                        Utils.GetUserRank(db, irc, channel, s_msg[1], channel);
                                    }
                                    else
                                    if (s_msg[0] == "losowanie")
                                    {
                                        if(s_msg[1] == "dołącz")
                                            Utils.TryToJoinRaffle(db, irc, channel, sender);
                                        else if (s_msg[1] == "info")
                                            Utils.ShowRaffleInfo(db, irc, channel, sender);
                                        else if (s_msg[1] == "lista")
                                            Utils.ShowRaffleUsers(db, irc, channel, sender);
                                        else if (s_msg[1] == "start")
                                            Utils.TryToStartRaffle(db, irc, channel, sender);
                                    }
                                    else
                                    if (s_msg[0] == "help")
                                    {
                                        string comm = s_msg[1];
                                        if (comm == "points")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile masz punktów: !points - twoje, !points [nick]");
                                        else if (comm == "precelki")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile masz precelków: !precelki - twoje, !precelki [nick]");
                                        else if (comm == "beczki")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile masz beczek: !beczki - twoje, !beczki [nick]");
                                        else if (comm == "watchtime")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile czasu spędziłeś na kanale: !watchtime - twój, !watchtime [nick], !watchtime [nick] [kanał]");
                                        else if (comm == "viewers" || comm == "widzowie")
                                            irc.SendPublicChatMessage(channel, "Wyświetla listę widzów: !viewers, !widzowie");
                                        else if (comm == "up" || comm == "uptime")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile trwa stream: !beczki - twoje, !beczki [nick]");
                                        else if (comm == "lastseen")
                                            irc.SendPublicChatMessage(channel, "Wyświetla kiedy ostatnio byłeś na streamie: !lastseen - ty, !lastseen [nick], !lastseen [nick] [kanał]");
                                        else if (comm == "ruletka")
                                            irc.SendPublicChatMessage(channel, "Spróbuj szczęścia i zgarnij punkty: !ruletka [ilość], !ruletka [ilość] [kanał]");
                                        else if (comm == "donejt" || comm == "donate")
                                            irc.SendPublicChatMessage(channel, "Przekaż swoje punkty innym: !donejt [nick] [ilość], !donate [nick] [ilość], !donate [nick] [ilość] [kanał], !donejt [nick] [ilość] [kanał]");
                                        else if (comm == "giveaway")
                                            irc.SendPublicChatMessage(channel, "Rozdaj swoje punkty losowej osobie: !giveaway [ilość]");
                                        else if (comm == "join")
                                            irc.SendPublicChatMessage(channel, "Dołączasz do loterii: !join");
                                        else if (comm == "ranking")
                                            irc.SendPublicChatMessage(channel, "Wyświetla który jesteś w rankngu: !ranking, !ranking [nick], !ranking [nick] [kanał], !ranking [top3/top5/top10]");
                                        else if (comm == "pointsall")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile łącznie zdobyłeś punktów: !pointsall - twoje, !pointsall [nick]");
                                        else if (comm == "precelkiall")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile łącznie zdobyłeś precelków: !precelkiall - twoje, !precelkiall [nick]");
                                        else if (comm == "beczkiall")
                                            irc.SendPublicChatMessage(channel, "Wyświetla ile łącznie zdobyłeś beczek: !beczkiall - twoje, !beczkiall [nick]");
                                        else if (comm == "losowanie")
                                            irc.SendPublicChatMessage(channel, "Wyświetla informacje i steruje losowaniem: !losowanie - ogólne info, !losowanie dołącz, !losowanie info - bieżące info, !losowanie start - rozpoczyna losowanie, !losowanie lista - pokazuje kto jest zapisany");
                                    }
                                    break;
                                case (3):
                                    s_msg[0] = s_msg[0].Replace("!", "");
                                    if (s_msg[0] == "donejt")
                                    {
                                        Points.GivePointsFromSenderToReciv(db, irc, channel, sender, s_msg[1], s_msg, channel);
                                    }
                                    else
                                    if (s_msg[0] == "donate")
                                    {
                                        Points.GivePointsFromSenderToReciv(db, irc, channel, sender, s_msg[1], s_msg, channel);
                                    }
                                    else
                                    // watchtime who where
                                    if (s_msg[0] == "watchtime")
                                    {
                                        Utils.GetUserWatchtime(db, irc, channel, s_msg[1], s_msg[2]);
                                    }
                                    else
                                    if (s_msg[0] == "lastseen")
                                    {
                                        Utils.GetUserLastSeen(db, irc, channel, s_msg[1], s_msg[2]);
                                    }
                                    else
                                    if (s_msg[0] == "ranking")
                                    {
                                        Utils.GetUserRank(db, irc, channel, s_msg[1], s_msg[2]);
                                    }
                                    else
                                    // !ruletka [ilość] [kanał]
                                    if (s_msg[0] == "ruletka")
                                    {
                                        Points.Roulette(db, irc, channel, sender, s_msg[1], s_msg[2]);
                                    }
                                    break;
                                case (4):
                                    //    0       1     2        3 
                                    // !donejt preclak 100 gragasgoesgym
                                    s_msg[0] = s_msg[0].Replace("!", "");
                                    if (s_msg[0] == "donejt")
                                    {
                                        Points.GivePointsFromSenderToReciv(db, irc, channel, sender, s_msg[1], s_msg, s_msg[3]);
                                    }
                                    else
                                    if (s_msg[0] == "donate")
                                    {
                                        Points.GivePointsFromSenderToReciv(db, irc, channel, sender, s_msg[1], s_msg, s_msg[3]);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            var streams = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users);
                            if (streams.Count() >= 1)
                            {
                                var stream = streams.First();

                                if ((DateTime.Now - stream.LastLive).TotalMinutes <= 15)
                                {
                                    int userId = stream.Users.FindIndex(x => x.Name.Equals(sender));
                                    // Manage adding points every stream.
                                    if (userId != -1)
                                    {
                                        var user = stream.Users[userId];
                                        if ((DateTime.Now - user.LastMsgTime).TotalSeconds >= 120)
                                        {
                                            Extensions.AddPointsToMaxLimit(stream, user, 1);
                                            Console.WriteLine($"Added msg point to {user.Name}");
                                        }
                                    }
                                }

                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}