using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Bot.Modules.Commands.Helpers
{
    class Chatters
    {
        static private JsonChatters json = null;
        
        static private void read(string channel)
        {
            try
            { 
                String text;
                MyWebClient web = new MyWebClient();
                System.IO.Stream stream = web.OpenRead("https://tmi.twitch.tv/group/user/"+channel+"/chatters?client_id="+ ConfigParams.TwitchAuth);
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject<JsonChatters>(text);
                }

            }
            catch (System.Net.WebException w)
            {
                read(channel);
            }
        }

        static public List<string> GetViewers(string channel)
        {
            read(channel);
            if(json != null) {
                List<string> k = json.chatters.broadcaster;
                List<string> l = json.chatters.vips;
                List<string> m = json.chatters.moderators;
                List<string> n = json.chatters.staff;
                List<string> o = json.chatters.admins;
                List<string> p = json.chatters.global_mods;
                List<string> r = json.chatters.viewers;
                k.AddRange(l);
                k.AddRange(l);
                k.AddRange(m);
                k.AddRange(n);
                k.AddRange(o);
                k.AddRange(p);
                k.AddRange(r);
                return k;
            } else 
                return null;
        }


        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 30 * 1000;
                
                return w;
            }
        }
    }
}
