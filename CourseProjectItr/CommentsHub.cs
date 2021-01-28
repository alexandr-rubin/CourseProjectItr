using CourseProjectItr.Data;
using CourseProjectItr.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectItr
{
    public class CommentsHub : Hub
    {
        public readonly CourseDbContext _db;

        public CommentsHub (CourseDbContext db)
        {
            _db = db;
        }
        public async Task Send(string comment, string userName)
        {
            await Clients.All.SendAsync("Send", comment, userName);
        }
    }
}
