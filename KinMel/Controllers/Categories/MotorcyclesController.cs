using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Maps.Geocoding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using KinMel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers.Categories
{
    [Authorize]

    public class MotorcyclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MotorcyclesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Motorcycles
        [AllowAnonymous]

        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var motorcycle = from c in _context.Motorcycle select c;
            switch (sortOrder)
            {
                case "Price":
                    motorcycle = motorcycle.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    motorcycle = motorcycle.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    motorcycle = motorcycle.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    motorcycle = motorcycle.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    motorcycle = motorcycle.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await motorcycle.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Motorcycles/Details/5
        [AllowAnonymous]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await _context.Motorcycle
                .Include(m => m.CreatedByUser)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (motorcycle == null)
            {
                return NotFound();
            }

            return View(motorcycle);
        }

        // GET: Motorcycles/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] =
                new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Motorcycle")), "Id",
                    "Name");
            return View();
        }

        // POST: Motorcycles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Brand,Model,Color,Engine,Mileage,TotalKm,Features,Id,SubCategoryId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes,Type,ModelYear,RegisteredDistrict,LotNo")]
            Motorcycle motorcycle, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    motorcycle.CreatedByUserId = currentUserId;
                    motorcycle.DateCreated = DateTimeOffset.UtcNow;
                    motorcycle.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{motorcycle.Address}, {motorcycle.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        motorcycle.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        motorcycle.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(motorcycle);
                    await _context.SaveChangesAsync();

                    string forSlug = motorcycle.Id + " " + String.Join(" ", motorcycle.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    motorcycle.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    motorcycle.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        motorcycle.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        motorcycle.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }

            ViewData["SubCategoryId"] =
                    new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Motorcycle")), "Id",
                        "Name");
            return View(motorcycle);
        }

        //// GET: Motorcycles/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var motorcycle = await _context.Motorcycle.SingleOrDefaultAsync(m => m.Id == id);
        //    if (motorcycle == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", motorcycle.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", motorcycle.SubCategoryId);
        //    return View(motorcycle);
        //}

        //// POST: Motorcycles/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Brand,Model,Color,Engine,Mileage,TotalKm,MadeYear,Features,Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Motorcycle motorcycle)
        //{
        //    if (id != motorcycle.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(motorcycle);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MotorcycleExists(motorcycle.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", motorcycle.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", motorcycle.SubCategoryId);
        //    return View(motorcycle);
        //}

        //// GET: Motorcycles/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var motorcycle = await _context.Motorcycle
        //        .Include(m => m.CreatedByUser)
        //        .Include(m => m.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (motorcycle == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(motorcycle);
        //}

        //// POST: Motorcycles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var motorcycle = await _context.Motorcycle.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Motorcycle.Remove(motorcycle);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MotorcycleExists(int id)
        //{
        //    return _context.Motorcycle.Any(e => e.Id == id);
        //}
    }
}
