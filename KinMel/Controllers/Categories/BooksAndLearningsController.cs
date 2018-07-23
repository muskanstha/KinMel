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
    public class BooksAndLearningsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BooksAndLearningsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BooksAndLearnings
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var booksAndLearning = from c in _context.BooksAndLearning select c;
            switch (sortOrder)
            {
                case "Price":
                    booksAndLearning = booksAndLearning.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    booksAndLearning = booksAndLearning.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    booksAndLearning = booksAndLearning.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    booksAndLearning = booksAndLearning.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    booksAndLearning = booksAndLearning.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await booksAndLearning.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: BooksAndLearnings/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booksAndLearning = await _context.BooksAndLearning
                .Include(b => b.CreatedByUser)
                .Include(b => b.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (booksAndLearning == null)
            {
                return NotFound();
            }

            return View(booksAndLearning);
        }

        // GET: BooksAndLearnings/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("BooksAndLearning")), "Id", "Name");
            return View();
        }

        // POST: BooksAndLearnings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Author,Isbn,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] BooksAndLearning booksAndLearning, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    booksAndLearning.CreatedByUserId = currentUserId;
                    booksAndLearning.DateCreated = DateTimeOffset.UtcNow;
                    booksAndLearning.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{booksAndLearning.Address}, {booksAndLearning.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        booksAndLearning.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        booksAndLearning.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(booksAndLearning);
                    await _context.SaveChangesAsync();

                    string forSlug = booksAndLearning.Id + " " + String.Join(" ", booksAndLearning.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    booksAndLearning.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    booksAndLearning.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        booksAndLearning.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        booksAndLearning.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("BooksAndLearning")), "Id", "Name", booksAndLearning.SubCategoryId);
            return View(booksAndLearning);
        }

        //// GET: BooksAndLearnings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var booksAndLearning = await _context.BooksAndLearning.SingleOrDefaultAsync(m => m.Id == id);
        //    if (booksAndLearning == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", booksAndLearning.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", booksAndLearning.SubCategoryId);
        //    return View(booksAndLearning);
        //}

        //// POST: BooksAndLearnings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Author,Isbn,Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] BooksAndLearning booksAndLearning)
        //{
        //    if (id != booksAndLearning.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(booksAndLearning);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BooksAndLearningExists(booksAndLearning.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", booksAndLearning.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", booksAndLearning.SubCategoryId);
        //    return View(booksAndLearning);
        //}

        //// GET: BooksAndLearnings/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var booksAndLearning = await _context.BooksAndLearning
        //        .Include(b => b.CreatedByUser)
        //        .Include(b => b.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (booksAndLearning == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(booksAndLearning);
        //}

        //// POST: BooksAndLearnings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var booksAndLearning = await _context.BooksAndLearning.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.BooksAndLearning.Remove(booksAndLearning);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool BooksAndLearningExists(int id)
        //{
        //    return _context.BooksAndLearning.Any(e => e.Id == id);
        //}
    }
}
