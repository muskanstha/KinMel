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
    public class HelpAndServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HelpAndServicesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HelpAndServices
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var helpAndServices = from c in _context.HelpAndServices select c;
            switch (sortOrder)
            {
                case "Price":
                    helpAndServices = helpAndServices.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    helpAndServices = helpAndServices.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    helpAndServices = helpAndServices.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    helpAndServices = helpAndServices.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    helpAndServices = helpAndServices.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await helpAndServices.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: HelpAndServices/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpAndServices = await _context.HelpAndServices
                .Include(h => h.CreatedByUser)
                .Include(h => h.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (helpAndServices == null)
            {
                return NotFound();
            }

            return View(helpAndServices);
        }

        // GET: HelpAndServices/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("HelpAndServices")), "Id", "Name");
            return View();
        }

        // POST: HelpAndServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] HelpAndServices helpAndServices, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    helpAndServices.CreatedByUserId = currentUserId;
                    helpAndServices.DateCreated = DateTimeOffset.UtcNow;
                    helpAndServices.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{helpAndServices.Address}, {helpAndServices.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        helpAndServices.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        helpAndServices.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(helpAndServices);
                    await _context.SaveChangesAsync();

                    string forSlug = helpAndServices.Id + " " + String.Join(" ", helpAndServices.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    helpAndServices.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    helpAndServices.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        helpAndServices.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        helpAndServices.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("HelpAndServices")), "Id", "Name", helpAndServices.SubCategoryId);
            return View(helpAndServices);
        }

        //// GET: HelpAndServices/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var helpAndServices = await _context.HelpAndServices.SingleOrDefaultAsync(m => m.Id == id);
        //    if (helpAndServices == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", helpAndServices.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", helpAndServices.SubCategoryId);
        //    return View(helpAndServices);
        //}

        //// POST: HelpAndServices/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] HelpAndServices helpAndServices)
        //{
        //    if (id != helpAndServices.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(helpAndServices);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!HelpAndServicesExists(helpAndServices.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", helpAndServices.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", helpAndServices.SubCategoryId);
        //    return View(helpAndServices);
        //}

        //// GET: HelpAndServices/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var helpAndServices = await _context.HelpAndServices
        //        .Include(h => h.CreatedByUser)
        //        .Include(h => h.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (helpAndServices == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(helpAndServices);
        //}

        //// POST: HelpAndServices/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var helpAndServices = await _context.HelpAndServices.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.HelpAndServices.Remove(helpAndServices);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool HelpAndServicesExists(int id)
        //{
        //    return _context.HelpAndServices.Any(e => e.Id == id);
        //}
    }
}
