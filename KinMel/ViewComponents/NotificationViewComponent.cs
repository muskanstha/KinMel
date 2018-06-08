using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinMel.ViewComponents
{
    public class NotificationViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public NotificationViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetNotifications();
            return View(items);
        }
        private async Task<List<Notification>> GetNotifications()
        {
            var username = User.Identity.Name;
            return await  _context.Notification.AsNoTracking().Where(n=> n.NotificationTo.UserName.Equals(username) && !n.IsRead).Include(n => n.NotificationFrom).Include(n => n.NotificationTo).OrderByDescending(n=> n.Date).ToListAsync();
        }
    }
}
