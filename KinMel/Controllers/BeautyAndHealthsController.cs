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
    public class BeautyAndHealthsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BeautyAndHealthsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BeautyAndHealths
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BeautyAndHealth.Include(b => b.CreatedByUser).Include(b => b.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BeautyAndHealths/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beautyAndHealth = await _context.BeautyAndHealth
                .Include(b => b.CreatedByUser)
                .Include(b => b.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (beautyAndHealth == null)
            {
                return NotFound();
            }

            return View(beautyAndHealth);
        }

        // GET: BeautyAndHealths/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("BeautyAndHealth")), "Id", "Name");
            return View();
        }

        // POST: BeautyAndHealths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] BeautyAndHealth beautyAndHealth, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    beautyAndHealth.CreatedByUserId = currentUserId;

                    beautyAndHealth.DateCreated = DateTime.Now;
                    _context.Add(beautyAndHealth);
                    await _context.SaveChangesAsync();

                    string forSlug = beautyAndHealth.Id + " " + String.Join(" ", beautyAndHealth.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    beautyAndHealth.Slug = slug;

                    await BlobStorageHelper.UploadBlobs(slug, imageFiles);

                    beautyAndHealth.ImageUrls = await BlobStorageHelper.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("BeautyAndHealth")), "Id", "Name", beautyAndHealth.SubCategoryId);
            return View(beautyAndHealth);
        }

        //// GET: BeautyAndHealths/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var beautyAndHealth = await _context.BeautyAndHealth.SingleOrDefaultAsync(m => m.Id == id);
        //    if (beautyAndHealth == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", beautyAndHealth.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", beautyAndHealth.SubCategoryId);
        //    return View(beautyAndHealth);
        //}

        //// POST: BeautyAndHealths/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] BeautyAndHealth beautyAndHealth)
        //{
        //    if (id != beautyAndHealth.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(beautyAndHealth);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BeautyAndHealthExists(beautyAndHealth.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", beautyAndHealth.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", beautyAndHealth.SubCategoryId);
        //    return View(beautyAndHealth);
        //}

        //// GET: BeautyAndHealths/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var beautyAndHealth = await _context.BeautyAndHealth
        //        .Include(b => b.CreatedByUser)
        //        .Include(b => b.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (beautyAndHealth == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(beautyAndHealth);
        //}

        //// POST: BeautyAndHealths/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var beautyAndHealth = await _context.BeautyAndHealth.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.BeautyAndHealth.Remove(beautyAndHealth);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool BeautyAndHealthExists(int id)
        //{
        //    return _context.BeautyAndHealth.Any(e => e.Id == id);
        //}
    }
}
