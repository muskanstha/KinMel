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

    public class ElectronicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ElectronicsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Electronics
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var electronics = from c in _context.Electronics select c;
            switch (sortOrder)
            {
                case "Price":
                    electronics = electronics.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    electronics = electronics.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    electronics = electronics.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    electronics = electronics.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    electronics = electronics.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await electronics.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Electronics/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electronics = await _context.Electronics
                .Include(e => e.CreatedByUser)
                .Include(e => e.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (electronics == null)
            {
                return NotFound();
            }

            return View(electronics);
        }

        // GET: Electronics/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Electronics")), "Id", "Name");
            return View();
        }

        // POST: Electronics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Electronics electronics, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    electronics.CreatedByUserId = currentUserId;

                    electronics.DateCreated = DateTime.Now;
                    _context.Add(electronics);
                    await _context.SaveChangesAsync();

                    string forSlug = electronics.Id + " " + String.Join(" ", electronics.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    electronics.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    electronics.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Electronics")), "Id", "Name", electronics.SubCategoryId);
            return View(electronics);
        }

        //// GET: Electronics/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var electronics = await _context.Electronics.SingleOrDefaultAsync(m => m.Id == id);
        //    if (electronics == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", electronics.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", electronics.SubCategoryId);
        //    return View(electronics);
        //}

        //// POST: Electronics/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Electronics electronics)
        //{
        //    if (id != electronics.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(electronics);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ElectronicsExists(electronics.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", electronics.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", electronics.SubCategoryId);
        //    return View(electronics);
        //}

        //// GET: Electronics/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var electronics = await _context.Electronics
        //        .Include(e => e.CreatedByUser)
        //        .Include(e => e.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (electronics == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(electronics);
        //}

        //// POST: Electronics/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var electronics = await _context.Electronics.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Electronics.Remove(electronics);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ElectronicsExists(int id)
        //{
        //    return _context.Electronics.Any(e => e.Id == id);
        //}
    }
}
