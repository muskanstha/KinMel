using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using KinMel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers
{
    [Authorize]
    public class TravelAndToursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TravelAndToursController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TravelAndTours
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TravelAndTours.Include(t => t.CreatedByUser).Include(t => t.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TravelAndTours/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travelAndTours = await _context.TravelAndTours
                .Include(t => t.CreatedByUser)
                .Include(t => t.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (travelAndTours == null)
            {
                return NotFound();
            }

            return View(travelAndTours);
        }

        // GET: TravelAndTours/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("TravelAndTours")), "Id", "Name");
            return View();
        }

        // POST: TravelAndTours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive")] TravelAndTours travelAndTours, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    travelAndTours.CreatedByUserId = currentUserId;

                    travelAndTours.DateCreated = DateTime.Now;
                    _context.Add(travelAndTours);
                    await _context.SaveChangesAsync();

                    string forSlug = travelAndTours.Id + " " + String.Join(" ", travelAndTours.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    travelAndTours.Slug = slug;

                    await BlobStorageHelper.UploadBlobs(slug, imageFiles);

                    travelAndTours.ImageUrls = await BlobStorageHelper.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("TravelAndTours")), "Id", "Name", travelAndTours.SubCategoryId);
            return View(travelAndTours);
        }

        //// GET: TravelAndTours/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var travelAndTours = await _context.TravelAndTours.SingleOrDefaultAsync(m => m.Id == id);
        //    if (travelAndTours == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", travelAndTours.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", travelAndTours.SubCategoryId);
        //    return View(travelAndTours);
        //}

        //// POST: TravelAndTours/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] TravelAndTours travelAndTours)
        //{
        //    if (id != travelAndTours.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(travelAndTours);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TravelAndToursExists(travelAndTours.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", travelAndTours.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", travelAndTours.SubCategoryId);
        //    return View(travelAndTours);
        //}

        //// GET: TravelAndTours/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var travelAndTours = await _context.TravelAndTours
        //        .Include(t => t.CreatedByUser)
        //        .Include(t => t.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (travelAndTours == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(travelAndTours);
        //}

        //// POST: TravelAndTours/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var travelAndTours = await _context.TravelAndTours.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.TravelAndTours.Remove(travelAndTours);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TravelAndToursExists(int id)
        //{
        //    return _context.TravelAndTours.Any(e => e.Id == id);
        //}
    }
}
