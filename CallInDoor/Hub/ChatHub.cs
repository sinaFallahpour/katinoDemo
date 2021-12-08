using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Katino.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Katino.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DataContext _context;
        public ChatHub(DataContext context)
        {
            _context = context;
        }
        public async Task GetConnection(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            user.ConecctionId = Context.ConnectionId;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        //public async Task RequestConnection(int id)
        //{
        //    if(id != 0)
        //    {
        //        var request = await _context.RestaurantRequests.SingleOrDefaultAsync(x => x.Id == id);

        //        if (request != null)
        //        {
        //            request.ConnectionId = Context.ConnectionId;
        //            _context.Update(request);
        //            await _context.SaveChangesAsync();
        //        }
        //    }
        //}
        //public async Task NewRequest(string code, string table)
        //{
        //    var u = await _context.Users.SingleOrDefaultAsync(x => x.RestaurantCode == code);
        //    if (u != null)
        //    {
        //        await Clients.Client(u.ConnectionId).SendAsync("NewRequest");
        //    }
        //}
        //public async Task Bell(string code,string table)
        //{
        //    var u = await _context.Users.SingleOrDefaultAsync(x => x.RestaurantCode == code);
        //    await Clients.Client(u.ConnectionId).SendAsync("Bell",table);
        //}
        //public async Task Paid(int id)
        //{
        //    var request = await _context.RestaurantRequests.SingleOrDefaultAsync(x => x.Id == id);

        //    if (request != null)
        //    {
        //        await Clients.Client(request.ConnectionId).SendAsync("Paid");
        //    }
        //}
    }
}
