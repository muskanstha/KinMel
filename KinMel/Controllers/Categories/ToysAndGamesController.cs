using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Maps.Geocoding;
using KinMel.Data;
using KinMel.Models;
using KinMel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KinMel.Controllers.Categories
{
    [Authorize]
    public class ToysAndGamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ToysAndGamesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToysAndGames
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var toysAndGames = from c in _context.ToysAndGames select c;
            switch (sortOrder)
            {
                case "Price":
                    toysAndGames = toysAndGames.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    toysAndGames = toysAndGames.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    toysAndGames = toysAndGames.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    toysAndGames = toysAndGames.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    toysAndGames = toysAndGames.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await toysAndGames.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: ToysAndGames/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toysAndGames = await _context.ToysAndGames
                .Include(t => t.CreatedByUser)
                .Include(t => t.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (toysAndGames == null)
            {
                return NotFound();
            }

            return View(toysAndGames);
        }

        // GET: ToysAndGames/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ToysAndGames")), "Id", "Name");
            return View();
        }

        // POST: ToysAndGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] ToysAndGames toysAndGames, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    toysAndGames.CreatedByUserId = currentUserId;
                    toysAndGames.DateCreated = DateTimeOffset.UtcNow;
                    toysAndGames.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{toysAndGames.Address}, {toysAndGames.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        toysAndGames.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        toysAndGames.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(toysAndGames);
                    await _context.SaveChangesAsync();

                    string forSlug = toysAndGames.Id + " " + String.Join(" ", toysAndGames.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    toysAndGames.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    toysAndGames.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        toysAndGames.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        toysAndGames.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ToysAndGames")), "Id", "Name", toysAndGames.SubCategoryId);
            return View(toysAndGames);
        }

        //// GET: ToysAndGames/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var toysAndGames = await _context.ToysAndGames.SingleOrDefaultAsync(m => m.Id == id);
        //    if (toysAndGames == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", toysAndGames.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", toysAndGames.SubCategoryId);
        //    return View(toysAndGames);
        //}

        //// POST: ToysAndGames/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] ToysAndGames toysAndGames)
        //{
        //    if (id != toysAndGames.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(toysAndGames);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ToysAndGamesExists(toysAndGames.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", toysAndGames.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", toysAndGames.SubCategoryId);
        //    return View(toysAndGames);
        //}

        //// GET: ToysAndGames/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var toysAndGames = await _context.ToysAndGames
        //        .Include(t => t.CreatedByUser)
        //        .Include(t => t.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (toysAndGames == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(toysAndGames);
        //}

        //// POST: ToysAndGames/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var toysAndGames = await _context.ToysAndGames.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.ToysAndGames.Remove(toysAndGames);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ToysAndGamesExists(int id)
        //{
        //    return _context.ToysAndGames.Any(e => e.Id == id);
        //}
    }
}
