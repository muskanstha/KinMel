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
    public class ComputerPartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComputerPartsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ComputerParts
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var computerParts = from c in _context.ComputerParts select c;
            switch (sortOrder)
            {
                case "Price":
                    computerParts = computerParts.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    computerParts = computerParts.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    computerParts = computerParts.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    computerParts = computerParts.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    computerParts = computerParts.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await computerParts.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: ComputerParts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerParts = await _context.ComputerParts
                .Include(c => c.CreatedByUser)
                .Include(c => c.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (computerParts == null)
            {
                return NotFound();
            }

            return View(computerParts);
        }

        // GET: ComputerParts/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ComputerParts")), "Id", "Name");
            return View();
        }

        // POST: ComputerParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] ComputerParts computerParts, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    computerParts.CreatedByUserId = currentUserId;
                    computerParts.DateCreated = DateTimeOffset.UtcNow;
                    computerParts.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{computerParts.Address}, {computerParts.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        computerParts.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        computerParts.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(computerParts);
                    await _context.SaveChangesAsync();

                    string forSlug = computerParts.Id + " " + String.Join(" ", computerParts.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    computerParts.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    computerParts.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        computerParts.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        computerParts.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ComputerParts")), "Id", "Name", computerParts.SubCategoryId);
            return View(computerParts);
        }

        //// GET: ComputerParts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var computerParts = await _context.ComputerParts.SingleOrDefaultAsync(m => m.Id == id);
        //    if (computerParts == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", computerParts.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", computerParts.SubCategoryId);
        //    return View(computerParts);
        //}

        //// POST: ComputerParts/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] ComputerParts computerParts)
        //{
        //    if (id != computerParts.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(computerParts);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ComputerPartsExists(computerParts.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", computerParts.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", computerParts.SubCategoryId);
        //    return View(computerParts);
        //}

        //// GET: ComputerParts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var computerParts = await _context.ComputerParts
        //        .Include(c => c.CreatedByUser)
        //        .Include(c => c.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (computerParts == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(computerParts);
        //}

        //// POST: ComputerParts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var computerParts = await _context.ComputerParts.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.ComputerParts.Remove(computerParts);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ComputerPartsExists(int id)
        //{
        //    return _context.ComputerParts.Any(e => e.Id == id);
        //}
    }
}
