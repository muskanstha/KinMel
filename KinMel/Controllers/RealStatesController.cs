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
    public class RealStatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RealStatesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RealStates
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var realState = from c in _context.RealState select c;
            switch (sortOrder)
            {
                case "Price":
                    realState = realState.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    realState = realState.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    realState = realState.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    realState = realState.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    realState = realState.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await realState.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: RealStates/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var realState = await _context.RealState
                .Include(r => r.CreatedByUser)
                .Include(r => r.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (realState == null)
            {
                return NotFound();
            }

            return View(realState);
        }

        // GET: RealStates/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Realstate")), "Id", "Name");
            return View();
        }

        // POST: RealStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropertyType,LandSize,Floors,TotalRooms,Furnishing,Features,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] RealState realState, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    realState.CreatedByUserId = currentUserId;

                    realState.DateCreated = DateTime.Now;
                    _context.Add(realState);
                    await _context.SaveChangesAsync();

                    string forSlug = realState.Id + " " + String.Join(" ", realState.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    realState.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    realState.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Realstate")), "Id", "Name", realState.SubCategoryId);
            return View(realState);
        }

        //// GET: RealStates/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var realState = await _context.RealState.SingleOrDefaultAsync(m => m.Id == id);
        //    if (realState == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", realState.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", realState.SubCategoryId);
        //    return View(realState);
        //}

        //// POST: RealStates/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("PropertyType,LandSize,Floors,TotalRooms,Furnishing,Features,Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] RealState realState)
        //{
        //    if (id != realState.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(realState);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RealStateExists(realState.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", realState.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", realState.SubCategoryId);
        //    return View(realState);
        //}

        //// GET: RealStates/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var realState = await _context.RealState
        //        .Include(r => r.CreatedByUser)
        //        .Include(r => r.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (realState == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(realState);
        //}

        //// POST: RealStates/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var realState = await _context.RealState.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.RealState.Remove(realState);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool RealStateExists(int id)
        //{
        //    return _context.RealState.Any(e => e.Id == id);
        //}
    }
}
