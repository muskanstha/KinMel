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

    public class MusicInstrumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MusicInstrumentsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: MusicInstruments
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var musicInstruments = from c in _context.MusicInstruments select c;
            switch (sortOrder)
            {
                case "Price":
                    musicInstruments = musicInstruments.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    musicInstruments = musicInstruments.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    musicInstruments = musicInstruments.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    musicInstruments = musicInstruments.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    musicInstruments = musicInstruments.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await musicInstruments.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: MusicInstruments/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicInstruments = await _context.MusicInstruments
                .Include(m => m.CreatedByUser)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (musicInstruments == null)
            {
                return NotFound();
            }

            return View(musicInstruments);
        }

        // GET: MusicInstruments/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("MusicInstruments")), "Id", "Name");
            return View();
        }

        // POST: MusicInstruments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] MusicInstruments musicInstruments, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    musicInstruments.CreatedByUserId = currentUserId;
                    musicInstruments.DateCreated = DateTimeOffset.UtcNow;
                    musicInstruments.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{musicInstruments.Address}, {musicInstruments.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        musicInstruments.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        musicInstruments.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(musicInstruments);
                    await _context.SaveChangesAsync();

                    string forSlug = musicInstruments.Id + " " + String.Join(" ", musicInstruments.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    musicInstruments.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    musicInstruments.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        musicInstruments.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        musicInstruments.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("MusicInstruments")), "Id", "Name", musicInstruments.SubCategoryId);
            return View(musicInstruments);
        }

        //// GET: MusicInstruments/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var musicInstruments = await _context.MusicInstruments.SingleOrDefaultAsync(m => m.Id == id);
        //    if (musicInstruments == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", musicInstruments.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", musicInstruments.SubCategoryId);
        //    return View(musicInstruments);
        //}

        //// POST: MusicInstruments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] MusicInstruments musicInstruments)
        //{
        //    if (id != musicInstruments.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(musicInstruments);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MusicInstrumentsExists(musicInstruments.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", musicInstruments.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", musicInstruments.SubCategoryId);
        //    return View(musicInstruments);
        //}

        //// GET: MusicInstruments/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var musicInstruments = await _context.MusicInstruments
        //        .Include(m => m.CreatedByUser)
        //        .Include(m => m.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (musicInstruments == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(musicInstruments);
        //}

        //// POST: MusicInstruments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var musicInstruments = await _context.MusicInstruments.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.MusicInstruments.Remove(musicInstruments);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MusicInstrumentsExists(int id)
        //{
        //    return _context.MusicInstruments.Any(e => e.Id == id);
        //}
    }
}
