using System;
using System.IO;
using Newtonsoft.Json;

namespace Bot.Modules

{
    public class Config
    {
        public static ConfigParamsNS Read()
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("config.json"))
                {
                    // Read the stream to a string, and write the string to the console.
                    string content = sr.ReadToEnd();

                    ConfigParamsNS cp = JsonConvert.DeserializeObject<ConfigParamsNS>(content);
                    ConfigParams.TwitchAuth = cp.TwitchAuth;
                    ConfigParams.Debug = cp.Debug;
                    ConfigParams.dbName = cp.dbName;
                    ConfigParams.ip = cp.ip;
                    ConfigParams.port = cp.port;
                    ConfigParams.userName = cp.userName;

                    Console.WriteLine("Reading \'config.json\'...");
                    Console.WriteLine($"\tTwitchAuth:{ConfigParams.TwitchAuth}");
                    Console.WriteLine($"\tdbName:{ConfigParams.dbName}");
                    Console.WriteLine($"\tuserName:{ConfigParams.userName}");
                    Console.WriteLine($"\tip:{ConfigParams.ip}");
                    Console.WriteLine($"\tport:{ConfigParams.port}");
                    Console.WriteLine($"\tport:{ConfigParams.Debug}");
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

    public static class ConfigParams
    {
        public static string TwitchAuth { get; set; }
        public static string dbName { get; set; }
        public static string userName {get;set;}
        public static string ip {get;set;}
        public static int port {get;set;}
        public static bool Debug {get;set;}

    }

    public  class ConfigParamsNS
    {
        public  string TwitchAuth { get; set; }
        public  string dbName { get; set; }
        public  string userName {get;set;}
        public  string ip {get;set;}
        public  int port {get;set;}
        public  bool Debug {get;set;}

    }
}