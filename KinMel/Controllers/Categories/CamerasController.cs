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

    public class CamerasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CamerasController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Cameras
        [AllowAnonymous]

        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var camera = from c in _context.Camera select c;
            switch (sortOrder)
            {
                case "Price":
                    camera = camera.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    camera = camera.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    camera = camera.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    camera = camera.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    camera = camera.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await camera.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cameras/Details/5
        [AllowAnonymous]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camera = await _context.Camera
                .Include(c => c.CreatedByUser)
                .Include(c => c.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (camera == null)
            {
                return NotFound();
            }

            return View(camera);
        }

        // GET: Cameras/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Camera")), "Id", "Name");
            return View();
        }

        // POST: Cameras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Camera camera, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    camera.CreatedByUserId = currentUserId;
                    camera.DateCreated = DateTimeOffset.UtcNow;
                    camera.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{camera.Address}, {camera.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        camera.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        camera.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(camera);
                    await _context.SaveChangesAsync();

                    string forSlug = camera.Id + " " + String.Join(" ", camera.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    camera.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    camera.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        camera.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        camera.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Camera")), "Id", "Name", camera.SubCategoryId);
            return View(camera);
        }

        //// GET: Cameras/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var camera = await _context.Camera.SingleOrDefaultAsync(m => m.Id == id);
        //    if (camera == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", camera.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", camera.SubCategoryId);
        //    return View(camera);
        //}

        //// POST: Cameras/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Camera camera)
        //{
        //    if (id != camera.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(camera);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CameraExists(camera.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", camera.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", camera.SubCategoryId);
        //    return View(camera);
        //}

        //// GET: Cameras/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var camera = await _context.Camera
        //        .Include(c => c.CreatedByUser)
        //        .Include(c => c.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (camera == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(camera);
        //}

        //// POST: Cameras/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var camera = await _context.Camera.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Camera.Remove(camera);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CameraExists(int id)
        //{
        //    return _context.Camera.Any(e => e.Id == id);
        //}
    }
}
