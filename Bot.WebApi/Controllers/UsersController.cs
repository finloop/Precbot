using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bot.Database.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bot.WebApi.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase

    
    {
        [HttpGet("api/users/{channel}/{username}")]
        public string GetUser(string channel, string username)
        {
            string ret = "Błąd";
            using(var db = new StreamsContext())
            {
                
                var streamList = db.Streams.Where(x => x.channelName.Equals(channel)).Include(x => x.Users).ToList();
                if(streamList.Count > 0)
                {
                    var stream = streamList.First();
                    var userId = stream.Users.FindIndex(x => x.Name.Equals(username));
                    if(userId != -1)
                    {
                        ret = JsonConvert.SerializeObject(stream.Users[userId]);
                    }
                }
            }
            return ret;
        }
    }
}
