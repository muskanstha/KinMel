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

namespace KinMel.Controllers.Categories.Categories
{
    [Authorize]
    public class SportsAndFitnessesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SportsAndFitnessesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SportsAndFitnesses
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var sportsAndFitness = from c in _context.SportsAndFitness select c;
            switch (sortOrder)
            {
                case "Price":
                    sportsAndFitness = sportsAndFitness.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    sportsAndFitness = sportsAndFitness.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    sportsAndFitness = sportsAndFitness.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    sportsAndFitness = sportsAndFitness.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    sportsAndFitness = sportsAndFitness.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await sportsAndFitness.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: SportsAndFitnesses/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportsAndFitness = await _context.SportsAndFitness
                .Include(s => s.CreatedByUser)
                .Include(s => s.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sportsAndFitness == null)
            {
                return NotFound();
            }

            return View(sportsAndFitness);
        }

        // GET: SportsAndFitnesses/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("SportsAndFitness")), "Id", "Name");
            return View();
        }

        // POST: SportsAndFitnesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] SportsAndFitness sportsAndFitness, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    sportsAndFitness.CreatedByUserId = currentUserId;
                    sportsAndFitness.DateCreated = DateTimeOffset.UtcNow;
                    sportsAndFitness.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{sportsAndFitness.Address}, {sportsAndFitness.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        sportsAndFitness.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        sportsAndFitness.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(sportsAndFitness);
                    await _context.SaveChangesAsync();

                    string forSlug = sportsAndFitness.Id + " " + String.Join(" ", sportsAndFitness.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    sportsAndFitness.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    sportsAndFitness.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        sportsAndFitness.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        sportsAndFitness.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("SportsAndFitness")), "Id", "Name", sportsAndFitness.SubCategoryId);
            return View(sportsAndFitness);
        }

        //// GET: SportsAndFitnesses/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sportsAndFitness = await _context.SportsAndFitness.SingleOrDefaultAsync(m => m.Id == id);
        //    if (sportsAndFitness == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", sportsAndFitness.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", sportsAndFitness.SubCategoryId);
        //    return View(sportsAndFitness);
        //}

        //// POST: SportsAndFitnesses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] SportsAndFitness sportsAndFitness)
        //{
        //    if (id != sportsAndFitness.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(sportsAndFitness);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SportsAndFitnessExists(sportsAndFitness.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", sportsAndFitness.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", sportsAndFitness.SubCategoryId);
        //    return View(sportsAndFitness);
        //}

        //// GET: SportsAndFitnesses/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sportsAndFitness = await _context.SportsAndFitness
        //        .Include(s => s.CreatedByUser)
        //        .Include(s => s.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (sportsAndFitness == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(sportsAndFitness);
        //}

        //// POST: SportsAndFitnesses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var sportsAndFitness = await _context.SportsAndFitness.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.SportsAndFitness.Remove(sportsAndFitness);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool SportsAndFitnessExists(int id)
        //{
        //    return _context.SportsAndFitness.Any(e => e.Id == id);
        //}
    }
}
