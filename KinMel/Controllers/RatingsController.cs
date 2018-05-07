using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers
{
    [AllowAnonymous]
    public class RatingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(this.User);

            var applicationDbContext = _context.Rating.Where(r => r.RatedForId.Equals(currentUserId)).Include(r => r.RatedFor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.Message = "We will provide error info later!";
                return View("Info");
            }

            var rating = await _context.Rating
                .Include(r => r.RatedFor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (rating == null)
            {
                ViewBag.Message = "We will provide error info later!";
                return View("Info");
            }

            return View(rating);
        }

        // GET: Ratings/Create
        [Authorize]
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                ViewBag.Message = "We will provide error info later!";
                return View("Info");
            }
            string currentUserId = _userManager.GetUserId(User);

            if (currentUserId.Equals(id))
            {
                ViewBag.Message = "We will provide error info later!";
                return View("Info");
            }
            ApplicationUser theUser = await _context.Users.Include(u => u.Ratings).Include(u => u.ClassifiedAds)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (theUser == null)
            {
                ViewBag.Message = "The User was not found";
                return View("Info");
            }

            Rating foundRating =
                theUser.Ratings?.FirstOrDefault(r => r.RatedById.Equals(currentUserId) && r.RatedForId.Equals(id));
            if (foundRating != null)
            {
                return RedirectToAction("Edit", new { id = foundRating.Id });
            }

            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Stars,Review")] Rating rating, string id)
        {
            if (ModelState.IsValid)
            {
                rating.RatedForId = id;

                string currentUserId = _userManager.GetUserId(User);
                if (currentUserId.Equals(id))
                {
                    ViewBag.Message = "We will provide error info later!";
                    return View("Info");
                }
                rating.RatedById = currentUserId;

                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = rating.Id });
            }
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Message = "We will provide error info later!";
                return View("Info");
            }

            var rating = await _context.Rating.SingleOrDefaultAsync(m => m.Id == id);
            if (rating == null)
            {
                ViewBag.Message = "We will provide error info later!";
                return View("Info");
            }
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Stars,Review")] Rating rating)
        {
            if (id != rating.Id)
            {
                ViewBag.Message = "We will provide error info later!";
                return View("Info");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Rating originalRating = await _context.Rating.SingleOrDefaultAsync(r => r.Id.Equals(id));
                    originalRating.Stars = rating.Stars;
                    originalRating.Review = rating.Review;
                    _context.Update(originalRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.Id))
                    {
                        ViewBag.Message = "We will provide error info later!";
                        return View("Info");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }

        //// GET: Ratings/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var rating = await _context.Rating
        //        .Include(r => r.RatedFor)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (rating == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(rating);
        //}

        //// POST: Ratings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var rating = await _context.Rating.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Rating.Remove(rating);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.Id == id);
        }
    }
}
