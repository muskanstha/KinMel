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

    public class MobilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MobilesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Mobiles
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var mobile = from c in _context.Mobile select c;
            switch (sortOrder)
            {
                case "Price":
                    mobile = mobile.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    mobile = mobile.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    mobile = mobile.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    mobile = mobile.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    mobile = mobile.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await mobile.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Mobiles/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobile = await _context.Mobile
                .Include(m => m.CreatedByUser)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mobile == null)
            {
                return NotFound();
            }

            return View(mobile);
        }

        // GET: Mobiles/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Mobile")), "Id", "Name");
            return View();
        }

        // POST: Mobiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Mobile mobile, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    mobile.CreatedByUserId = currentUserId;
                    mobile.DateCreated = DateTimeOffset.UtcNow;
                    mobile.IsActive = true;

                    var locationRequest = new GeocodingRequest { Address = $"{mobile.Address}, {mobile.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        mobile.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        mobile.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(mobile);
                    await _context.SaveChangesAsync();

                    string forSlug = mobile.Id + " " + String.Join(" ", mobile.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    mobile.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    mobile.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        mobile.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        mobile.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }
            }

            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Mobile")), "Id", "Name");
            return View(mobile);
        }

        // GET: Mobiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobile = await _context.Mobile.SingleOrDefaultAsync(m => m.Id == id);
            if (mobile == null)
            {
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Mobile")), "Id", "Name");
            return View(mobile);
        }

        //// POST: Mobiles/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,CreatedByUserId,Title,Description,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Mobile mobile)
        //{
        //    if (id != mobile.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(mobile);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MobileExists(mobile.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", mobile.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", mobile.SubCategoryId);
        //    return View(mobile);
        //}

        //// GET: Mobiles/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var mobile = await _context.Mobile
        //        .Include(m => m.CreatedByUser)
        //        .Include(m => m.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (mobile == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(mobile);
        //}

        //// POST: Mobiles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var mobile = await _context.Mobile.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Mobile.Remove(mobile);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MobileExists(int id)
        //{
        //    return _context.Mobile.Any(e => e.Id == id);
        //}
    }
}
