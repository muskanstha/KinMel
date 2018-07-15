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

    public class PetsAndPetCaresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PetsAndPetCaresController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PetsAndPetCares
        [AllowAnonymous]

        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var petsAndPetCare = from c in _context.PetsAndPetCare select c;
            switch (sortOrder)
            {
                case "Price":
                    petsAndPetCare = petsAndPetCare.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    petsAndPetCare = petsAndPetCare.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    petsAndPetCare = petsAndPetCare.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    petsAndPetCare = petsAndPetCare.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    petsAndPetCare = petsAndPetCare.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await petsAndPetCare.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: PetsAndPetCares/Details/5
        [AllowAnonymous]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petsAndPetCare = await _context.PetsAndPetCare
                .Include(p => p.CreatedByUser)
                .Include(p => p.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (petsAndPetCare == null)
            {
                return NotFound();
            }

            return View(petsAndPetCare);
        }

        // GET: PetsAndPetCares/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("PetsAndPetCare")), "Id", "Name");
            return View();
        }

        // POST: PetsAndPetCares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] PetsAndPetCare petsAndPetCare, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    petsAndPetCare.CreatedByUserId = currentUserId;
                    petsAndPetCare.DateCreated = DateTimeOffset.UtcNow;
                    petsAndPetCare.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{petsAndPetCare.Address}, {petsAndPetCare.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        petsAndPetCare.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        petsAndPetCare.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(petsAndPetCare);
                    await _context.SaveChangesAsync();

                    string forSlug = petsAndPetCare.Id + " " + String.Join(" ", petsAndPetCare.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    petsAndPetCare.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    petsAndPetCare.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        petsAndPetCare.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        petsAndPetCare.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("PetsAndPetCare")), "Id", "Name", petsAndPetCare.SubCategoryId);
            return View(petsAndPetCare);
        }

        //// GET: PetsAndPetCares/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var petsAndPetCare = await _context.PetsAndPetCare.SingleOrDefaultAsync(m => m.Id == id);
        //    if (petsAndPetCare == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", petsAndPetCare.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", petsAndPetCare.SubCategoryId);
        //    return View(petsAndPetCare);
        //}

        //// POST: PetsAndPetCares/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] PetsAndPetCare petsAndPetCare)
        //{
        //    if (id != petsAndPetCare.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(petsAndPetCare);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PetsAndPetCareExists(petsAndPetCare.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", petsAndPetCare.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", petsAndPetCare.SubCategoryId);
        //    return View(petsAndPetCare);
        //}

        //// GET: PetsAndPetCares/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var petsAndPetCare = await _context.PetsAndPetCare
        //        .Include(p => p.CreatedByUser)
        //        .Include(p => p.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (petsAndPetCare == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(petsAndPetCare);
        //}

        //// POST: PetsAndPetCares/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var petsAndPetCare = await _context.PetsAndPetCare.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.PetsAndPetCare.Remove(petsAndPetCare);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PetsAndPetCareExists(int id)
        //{
        //    return _context.PetsAndPetCare.Any(e => e.Id == id);
        //}
    }
}
