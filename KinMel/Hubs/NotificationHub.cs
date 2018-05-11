using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace KinMel.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        //private readonly ApplicationDbContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;



        //public NotificationHub(ApplicationDbContext context,
        //    UserManager<ApplicationUser> userManager)
        //{
        //    _context = context;
        //    _userManager = userManager;

        //}

        //public NotificationHub()
        //{

        //}
       
        //public override Task OnConnectedAsync()
        //{

        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}

        //public async Task NotificationCount(string id, int count)
        //{
        //    //int count = await this.GetNotificationsCount(string id);
        //    await Clients.User(id).SendAsync("Receivecount", count);

        //}

        //private  async Task<int> GetNotificationsCount(string id)
        //{
        //    var currentUser = await _userManager.FindByNameAsync(Context.User.Identity.Name);
        //    int notificationCount = _context.Notification.Count(n => n.NotificationToId.Equals(currentUser.Id) && !n.IsRead);
        //    return notificationCount;
        //}
    }
}
