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
    public class ApparelsAndAccessoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApparelsAndAccessoriesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApparelsAndAccessories
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var apparelsAndAccessories = from c in _context.ApparelsAndAccessories select c;
            switch (sortOrder)
            {
                case "Price":
                    apparelsAndAccessories = apparelsAndAccessories.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    apparelsAndAccessories = apparelsAndAccessories.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    apparelsAndAccessories = apparelsAndAccessories.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    apparelsAndAccessories = apparelsAndAccessories.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    apparelsAndAccessories = apparelsAndAccessories.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await apparelsAndAccessories.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: ApparelsAndAccessories/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apparelsAndAccessories = await _context.ApparelsAndAccessories
                .Include(a => a.CreatedByUser)
                .Include(a => a.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (apparelsAndAccessories == null)
            {
                return NotFound();
            }

            return View(apparelsAndAccessories);
        }

        // GET: ApparelsAndAccessories/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ApparelsAndAccessories")), "Id", "Name");
            return View();
        }

        // POST: ApparelsAndAccessories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] ApparelsAndAccessories apparelsAndAccessories, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    apparelsAndAccessories.CreatedByUserId = currentUserId;
                    apparelsAndAccessories.DateCreated = DateTimeOffset.UtcNow;
                    apparelsAndAccessories.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{apparelsAndAccessories.Address}, {apparelsAndAccessories.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        apparelsAndAccessories.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        apparelsAndAccessories.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(apparelsAndAccessories);
                    await _context.SaveChangesAsync();

                    string forSlug = apparelsAndAccessories.Id + " " + String.Join(" ", apparelsAndAccessories.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    apparelsAndAccessories.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    apparelsAndAccessories.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        apparelsAndAccessories.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        apparelsAndAccessories.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ApparelsAndAccessories")), "Id", "Name", apparelsAndAccessories.SubCategoryId);
            return View(apparelsAndAccessories);
        }

        //// GET: ApparelsAndAccessories/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var apparelsAndAccessories = await _context.ApparelsAndAccessories.SingleOrDefaultAsync(m => m.Id == id);
        //    if (apparelsAndAccessories == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", apparelsAndAccessories.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", apparelsAndAccessories.SubCategoryId);
        //    return View(apparelsAndAccessories);
        //}

        //// POST: ApparelsAndAccessories/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] ApparelsAndAccessories apparelsAndAccessories)
        //{
        //    if (id != apparelsAndAccessories.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(apparelsAndAccessories);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ApparelsAndAccessoriesExists(apparelsAndAccessories.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", apparelsAndAccessories.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", apparelsAndAccessories.SubCategoryId);
        //    return View(apparelsAndAccessories);
        //}

        //// GET: ApparelsAndAccessories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var apparelsAndAccessories = await _context.ApparelsAndAccessories
        //        .Include(a => a.CreatedByUser)
        //        .Include(a => a.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (apparelsAndAccessories == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(apparelsAndAccessories);
        //}

        //// POST: ApparelsAndAccessories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var apparelsAndAccessories = await _context.ApparelsAndAccessories.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.ApparelsAndAccessories.Remove(apparelsAndAccessories);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ApparelsAndAccessoriesExists(int id)
        //{
        //    return _context.ApparelsAndAccessories.Any(e => e.Id == id);
        //}
    }
}
