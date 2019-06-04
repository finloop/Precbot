using System;
using System.IO;
using Newtonsoft.Json;

namespace Bot.Modules

{
    public class Config
    {
        public static ConfigParams Read()
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("config.json"))
                {
                    // Read the stream to a string, and write the string to the console.
                    string content = sr.ReadToEnd();
                    ConfigParams cp = JsonConvert.DeserializeObject<ConfigParams>(content);
                    Console.WriteLine("Reading \'config.json\'...");
                    Console.WriteLine($"\tTwitchAuth:{cp.TwitchAuth}");
                    Console.WriteLine($"\tdbName:{cp.dbName}");
                    Console.WriteLine($"\tuserName:{cp.userName}");
                    Console.WriteLine($"\tip:{cp.ip}");
                    Console.WriteLine($"\tport:{cp.port}");
                    return cp;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file \'config.json\' could not be read:");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
                return null;
            }
        }
    }

    public class ConfigParams
    {
        public string TwitchAuth { get; set; }
        public string dbName { get; set; }
        public string userName {get;set;}
        public string ip {get;set;}
        public int port {get;set;}

    }
}