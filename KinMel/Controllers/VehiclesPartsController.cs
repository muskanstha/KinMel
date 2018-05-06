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
    public class VehiclesPartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public VehiclesPartsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: VehiclesParts
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var vehiclesParts = from c in _context.VehiclesParts select c;
            switch (sortOrder)
            {
                case "Price":
                    vehiclesParts = vehiclesParts.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    vehiclesParts = vehiclesParts.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    vehiclesParts = vehiclesParts.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    vehiclesParts = vehiclesParts.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    vehiclesParts = vehiclesParts.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await vehiclesParts.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: VehiclesParts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiclesParts = await _context.VehiclesParts
                .Include(v => v.CreatedByUser)
                .Include(v => v.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vehiclesParts == null)
            {
                return NotFound();
            }

            return View(vehiclesParts);
        }

        // GET: VehiclesParts/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("VehiclesParts")), "Id", "Name");
            return View();
        }

        // POST: VehiclesParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] VehiclesParts vehiclesParts, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    vehiclesParts.CreatedByUserId = currentUserId;

                    vehiclesParts.DateCreated = DateTime.Now;
                    _context.Add(vehiclesParts);
                    await _context.SaveChangesAsync();

                    string forSlug = vehiclesParts.Id + " " + String.Join(" ", vehiclesParts.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    vehiclesParts.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    vehiclesParts.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("VehiclesParts")), "Id", "Name", vehiclesParts.SubCategoryId);
            return View(vehiclesParts);
        }

        //// GET: VehiclesParts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vehiclesParts = await _context.VehiclesParts.SingleOrDefaultAsync(m => m.Id == id);
        //    if (vehiclesParts == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", vehiclesParts.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", vehiclesParts.SubCategoryId);
        //    return View(vehiclesParts);
        //}

        //// POST: VehiclesParts/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] VehiclesParts vehiclesParts)
        //{
        //    if (id != vehiclesParts.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(vehiclesParts);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!VehiclesPartsExists(vehiclesParts.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", vehiclesParts.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", vehiclesParts.SubCategoryId);
        //    return View(vehiclesParts);
        //}

        //// GET: VehiclesParts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vehiclesParts = await _context.VehiclesParts
        //        .Include(v => v.CreatedByUser)
        //        .Include(v => v.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (vehiclesParts == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(vehiclesParts);
        //}

        //// POST: VehiclesParts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var vehiclesParts = await _context.VehiclesParts.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.VehiclesParts.Remove(vehiclesParts);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool VehiclesPartsExists(int id)
        //{
        //    return _context.VehiclesParts.Any(e => e.Id == id);
        //}
    }
}
