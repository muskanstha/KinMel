using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Hubs;
using KinMel.Models;
using KinMel.ViewComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;


namespace KinMel.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHubContext<NotificationHub> _notificationHubContext;


        public NotificationsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> notificationHub)
        {
            _context = context;
            _userManager = userManager;
            _notificationHubContext = notificationHub;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var applicationDbContext = _context.Notification.Where(n => n.NotificationToId.Equals(currentUser.Id)).Include(n => n.NotificationFrom).Include(n => n.NotificationTo).OrderByDescending(n => n.Date);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: NotificationsCount
        public async Task<JsonResult> NotificationCount()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            int notificationCount = NotificationCount(currentUser.Id);
            return Json(notificationCount);
        }
        // GET: Notifications/Read/5
        public async Task<ActionResult> Read(int? id)
        {
            if (id == null) { return View("Info"); }

            Notification notification = _context.Notification.Find(id);
            if (notification == null) { return View("Info"); }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();

                    //int notificationCount = NotificationCount(notification.NotificationToId);
                    //var user = _notificationHubContext.Clients.User(notification.NotificationToId);
                    //await user.SendAsync("Receivecount", notificationCount);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id)) { return NotFound(); } else { throw; }
                }
            }
            return RedirectToAction(notification.Action, notification.ActionController,
                                new { id = notification.ActionId });
        }


        // GET: Notifications/Delete/5
        public async Task<string> Delete(int? id)
        {
            if (id == null)
            {
                return "Ok";
            }

            var notification = await _context.Notification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return "Ok";
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser.Id.Equals(notification.NotificationToId))
            {
                _context.Notification.Remove(notification);
                await _context.SaveChangesAsync();

                int notificationCount = NotificationCount(notification.NotificationToId);
                var user = _notificationHubContext.Clients.User(notification.NotificationToId);
                await user.SendAsync("NotificationDeleted", notificationCount);

            }
            return "Ok";

        }

        //// GET: Notifications/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var notification = await _context.Notification
        //        .Include(n => n.NotificationFrom)
        //        .Include(n => n.NotificationTo)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (notification == null)
        //    {
        //        return NotFound();
        //    }


        //    return View(notification);
        //}

        // GET: Notifications/Create
        public IActionResult Create()
        {
            ViewData["NotificationFromId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["NotificationToId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NotificationToId,NotificationFromId,NotificationText,ActionController,Action,ActionId,IsRead,Date")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();

                int notificationCount = NotificationCount(notification.NotificationToId);
                var user = _notificationHubContext.Clients.User(notification.NotificationToId);
                await user.SendAsync("Receivecount", notificationCount);

                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationFromId"] = new SelectList(_context.Users, "Id", "Id", notification.NotificationFromId);
            ViewData["NotificationToId"] = new SelectList(_context.Users, "Id", "Id", notification.NotificationToId);
            return View(notification);
        }

        public IActionResult NotificationViewComponent()
        {
            return ViewComponent("Notification");
        }
        //// GET: Notifications/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var notification = await _context.Notification.FindAsync(id);
        //    if (notification == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["NotificationFromId"] = new SelectList(_context.Users, "Id", "Id", notification.NotificationFromId);
        //    ViewData["NotificationToId"] = new SelectList(_context.Users, "Id", "Id", notification.NotificationToId);
        //    return View(notification);
        //}

        //// POST: Notifications/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,NotificationToId,NotificationFromId,NotificationText,ActionController,Action,ActionId,IsRead,Date")] Notification notification)
        //{
        //    if (id != notification.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(notification);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!NotificationExists(notification.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["NotificationFromId"] = new SelectList(_context.Users, "Id", "Id", notification.NotificationFromId);
        //    ViewData["NotificationToId"] = new SelectList(_context.Users, "Id", "Id", notification.NotificationToId);
        //    return View(notification);
        //}

        // GET: Notifications/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var notification = await _context.Notification
        //        .Include(n => n.NotificationFrom)
        //        .Include(n => n.NotificationTo)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (notification == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(notification);
        //}

        //// POST: Notifications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var notification = await _context.Notification.FindAsync(id);
        //    _context.Notification.Remove(notification);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool NotificationExists(int id)
        {
            return _context.Notification.Any(e => e.Id == id);
        }
        private int NotificationCount(string id)
        {
            return _context.Notification.Count(n => n.NotificationToId.Equals(id) && !n.IsRead);
        }
    }
}
