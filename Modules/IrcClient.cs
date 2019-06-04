using System;
using System.Net.Sockets;
using System.IO;

namespace Bot.Modules
{
    // Reference: https://www.youtube.com/watch?v=Ss-OzV9aUZg
    public class IrcClient
    {
        public string userName;
        public static string[] channels;
        private int port;
        private string password;
        private string ip;

        public TcpClient _tcpClient = new TcpClient();
        private StreamReader _inputStream;
        private StreamWriter _outputStream;

        public IrcClient(string ip, int port, string userName, string password, string[] _channels)
        {
            try
            {
                this.userName = userName;
                channels = _channels;
                this.ip = ip;
                this.port = port;
                this.password = password;

                ReconnectIfNeeded();
                /*_tcpClient = new TcpClient(ip, port);
                _inputStream = new StreamReader(_tcpClient.GetStream());
                _outputStream = new StreamWriter(_tcpClient.GetStream());

                // Try to join the room
                _outputStream.WriteLine("PASS " + password);
                _outputStream.WriteLine("NICK " + userName);
                _outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
                _outputStream.WriteLine("JOIN #" + channel);
                _outputStream.Flush(); */
            }
            catch (Exception ex)
            {
                Reconnect();
            }
        }

        public void ReconnectIfNeeded()
        {
            try
            {
                if (!_tcpClient.Connected)
                {
                    _tcpClient = new TcpClient(ip, port);
                    _inputStream = new StreamReader(_tcpClient.GetStream());
                    _outputStream = new StreamWriter(_tcpClient.GetStream());

                    _outputStream.WriteLine("PASS " + password);
                    _outputStream.WriteLine("NICK " + userName);
                    _outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
                    foreach (string channel in channels)
                        _outputStream.WriteLine("JOIN #" + channel);
                    //_outputStream.WriteLine("CAP REQ :twitch.tv/commands"); //whispers
                    _outputStream.Flush();
                }
            }
            catch (Exception ex)
            {
                Reconnect();
            }

        }

        public void Reconnect()
        {
            try
            {
                _tcpClient = new TcpClient(ip, port);
                _inputStream = new StreamReader(_tcpClient.GetStream());
                _outputStream = new StreamWriter(_tcpClient.GetStream());

                _outputStream.WriteLine("PASS " + password);
                _outputStream.WriteLine("NICK " + userName);
                _outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
                foreach (string channel in channels)
                    _outputStream.WriteLine("JOIN #" + channel);
                _outputStream.Flush();
            }
            catch (Exception ex)
            {
                Reconnect();
            }

        }

        public void SendRawIrcMessage(string message)
        {
            try
            {
                _outputStream.WriteLine(message);
                _outputStream.Flush();
            }
            catch (Exception ex)
            {
                Reconnect();
            }
        }

        public void SendPublicChatMessage(string channel, string message)
        {
            try
            {
                SendRawIrcMessage(":" + userName + "!" + userName + "@" + userName +
                ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
            }
            catch (Exception ex)
            {
                Reconnect();
            }
        }

        public void SendPrivateChatMessage(string channel, string message)
        {
            try
            { // PRIVMSG #jtv :/w [TargetUser][whitespace][message]
                SendRawIrcMessage("PRIVMSG jtv :/w " + channel + " " + message);
            }
            catch (Exception ex)
            {
                Reconnect();
            }
        }

        public string ReadMessage()
        {
            try
            {
                string message = _inputStream.ReadLine();
                if (message.Equals("PING :tmi.twitch.tv"))
                {
                    SendRawIrcMessage("PONG :tmi.twitch.tv");
                }
                return message;
            }
            catch (Exception ex)
            {
                Reconnect();
                return "Error while reading message.";
            }
        }
    }
}