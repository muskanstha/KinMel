using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CarsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Cars
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var car = from c in _context.Car select c;
            switch (sortOrder)
            {
                case "Price":
                    car = car.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    car = car.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    car = car.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    car = car.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    car = car.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await car.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.CreatedByUser)
                .Include(c => c.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Car")), "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Brand,Model,ModelYear,Color,TotalKm,FuelType,Features,DoorsNo,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,Engine,Mileage,Transmission,RegisteredDistrict,LotNo,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Car car, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    car.CreatedByUserId = currentUserId;
                    car.DateCreated = DateTimeOffset.UtcNow;
                    car.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{car.Address}, {car.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        car.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        car.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(car);
                    await _context.SaveChangesAsync();

                    string forSlug = car.Id + " " + String.Join(" ", car.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    car.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    car.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        car.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        car.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Car")), "Id", "Name", car.SubCategoryId);
            return View(car);
        }

        //// GET: Cars/Edit/5

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var car = await _context.Car.SingleOrDefaultAsync(m => m.Id == id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", car.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", car.SubCategoryId);
        //    return View(car);
        //}

        //// POST: Cars/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Type,Brand,Model,ModelYear,Color,TotalKm,FuelType,Features,DoorsNo,Id,SubCategoryId,CreatedByUserId,Title,Description,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Car car)
        //{
        //    if (id != car.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(car);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CarExists(car.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", car.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", car.SubCategoryId);
        //    return View(car);
        //}

        //// GET: Cars/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var car = await _context.Car
        //        .Include(c => c.CreatedByUser)
        //        .Include(c => c.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(car);
        //}

        //// POST: Cars/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var car = await _context.Car.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Car.Remove(car);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CarExists(int id)
        //{
        //    return _context.Car.Any(e => e.Id == id);
        //}
    }
}
