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
    public class ComputersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ComputersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Computers
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var computer = from c in _context.Computer select c;
            switch (sortOrder)
            {
                case "Price":
                    computer = computer.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    computer = computer.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    computer = computer.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    computer = computer.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    computer = computer.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await computer.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Computers/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer
                .Include(c => c.CreatedByUser)
                .Include(c => c.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // GET: Computers/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Computer")), "Id", "Name");
            return View();
        }

        // POST: Computers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Processor,ProcessorGeneration,Ram,GraphicsCard,HDD,SSD,ScreenType,ScreenSize,Battery,Features,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Computer computer, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    computer.CreatedByUserId = currentUserId;
                    computer.DateCreated = DateTimeOffset.UtcNow;
                    computer.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{computer.Address}, {computer.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        computer.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        computer.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(computer);
                    await _context.SaveChangesAsync();

                    string forSlug = computer.Id + " " + String.Join(" ", computer.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    computer.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    computer.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        computer.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        computer.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Computer")), "Id", "Name", computer.SubCategoryId);
            return View(computer);
        }

        //// GET: Computers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var computer = await _context.Computer.SingleOrDefaultAsync(m => m.Id == id);
        //    if (computer == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", computer.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", computer.SubCategoryId);
        //    return View(computer);
        //}

        //// POST: Computers/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Type,Processor,ProcessorGeneration,Ram,GraphicsCard,HDD,SSD,ScreenType,ScreenSize,Battery,Features,Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Computer computer)
        //{
        //    if (id != computer.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(computer);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ComputerExists(computer.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", computer.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", computer.SubCategoryId);
        //    return View(computer);
        //}

        //// GET: Computers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var computer = await _context.Computer
        //        .Include(c => c.CreatedByUser)
        //        .Include(c => c.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (computer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(computer);
        //}

        //// POST: Computers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var computer = await _context.Computer.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Computer.Remove(computer);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ComputerExists(int id)
        //{
        //    return _context.Computer.Any(e => e.Id == id);
        //}
    }
}
