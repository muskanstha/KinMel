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
    public class MobileAccessoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MobileAccessoriesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MobileAccessories
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MobileAccessories.Include(m => m.CreatedByUser).Include(m => m.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MobileAccessories/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileAccessories = await _context.MobileAccessories
                .Include(m => m.CreatedByUser)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mobileAccessories == null)
            {
                return NotFound();
            }

            return View(mobileAccessories);
        }

        // GET: MobileAccessories/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("MobileAccessories")), "Id", "Name");
            return View();
        }

        // POST: MobileAccessories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] MobileAccessories mobileAccessories, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    mobileAccessories.CreatedByUserId = currentUserId;

                    mobileAccessories.DateCreated = DateTime.Now;
                    _context.Add(mobileAccessories);
                    await _context.SaveChangesAsync();

                    string forSlug = mobileAccessories.Id + " " + String.Join(" ", mobileAccessories.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    mobileAccessories.Slug = slug;

                    await BlobStorageHelper.UploadBlobs(slug, imageFiles);

                    mobileAccessories.ImageUrls = await BlobStorageHelper.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("MobileAccessories")), "Id", "Name", mobileAccessories.SubCategoryId);
            return View(mobileAccessories);
        }

        //// GET: MobileAccessories/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var mobileAccessories = await _context.MobileAccessories.SingleOrDefaultAsync(m => m.Id == id);
        //    if (mobileAccessories == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", mobileAccessories.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", mobileAccessories.SubCategoryId);
        //    return View(mobileAccessories);
        //}

        //// POST: MobileAccessories/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] MobileAccessories mobileAccessories)
        //{
        //    if (id != mobileAccessories.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(mobileAccessories);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MobileAccessoriesExists(mobileAccessories.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", mobileAccessories.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", mobileAccessories.SubCategoryId);
        //    return View(mobileAccessories);
        //}

        //// GET: MobileAccessories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var mobileAccessories = await _context.MobileAccessories
        //        .Include(m => m.CreatedByUser)
        //        .Include(m => m.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (mobileAccessories == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(mobileAccessories);
        //}

        //// POST: MobileAccessories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var mobileAccessories = await _context.MobileAccessories.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MobileAccessories.Remove(mobileAccessories);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MobileAccessoriesExists(int id)
        //{
        //    return _context.MobileAccessories.Any(e => e.Id == id);
        //}
    }
}
