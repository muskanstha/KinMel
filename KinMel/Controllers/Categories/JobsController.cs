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
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public JobsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jobs
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var jobs = from c in _context.Jobs select c;
            switch (sortOrder)
            {
                case "Price":
                    jobs = jobs.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    jobs = jobs.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    jobs = jobs.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    jobs = jobs.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    jobs = jobs.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await jobs.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Jobs/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobs = await _context.Jobs
                .Include(j => j.CreatedByUser)
                .Include(j => j.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (jobs == null)
            {
                return NotFound();
            }

            return View(jobs);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Jobs")), "Id", "Name");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Salary,WorkingDays,ContractFor,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Jobs jobs, List<IFormFile> imageFiles, IFormFile primaryImage)
        {
            if (ModelState.IsValid)
            {
                long? primaryImageLength = primaryImage?.Length;
                if (primaryImageLength > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    jobs.CreatedByUserId = currentUserId;
                    jobs.DateCreated = DateTimeOffset.UtcNow;
                    jobs.IsActive = true;


                    var locationRequest = new GeocodingRequest { Address = $"{jobs.Address}, {jobs.City}" };
                    var locationResponse = new GeocodingService().GetResponse(locationRequest);
                    if (locationResponse.Results.Length > 0)
                    {
                        jobs.Latitude = locationResponse.Results.First().Geometry.Location.Latitude;
                        jobs.Longitude = locationResponse.Results.First().Geometry.Location.Longitude;
                    }

                    _context.Add(jobs);
                    await _context.SaveChangesAsync();

                    string forSlug = jobs.Id + " " + String.Join(" ", jobs.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    jobs.Slug = slug;

                    BlobStorageUploader blobStorageUploader = new BlobStorageUploader();
                    jobs.PrimaryImageUrl = await blobStorageUploader.UploadMainBlob(slug, primaryImage);

                    long? imageFilesLength = imageFiles?.Sum(f => f.Length);
                    if (imageFilesLength > 0)
                    {
                        jobs.ImageUrls = await blobStorageUploader.UploadBlobs(slug, imageFiles);
                    }
                    else
                    {
                        jobs.ImageUrls = await blobStorageUploader.ListBlobsFolder(slug);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Jobs")), "Id", "Name", jobs.SubCategoryId);
            return View(jobs);
        }

        //        // GET: Jobs/Edit/5
        //        public async Task<IActionResult> Edit(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var jobs = await _context.Jobs.SingleOrDefaultAsync(m => m.Id == id);
        //            if (jobs == null)
        //            {
        //                return NotFound();
        //            }
        //            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", jobs.SubCategoryId);
        //            return View(jobs);
        //        }

        //        // POST: Jobs/Edit/5
        //        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Edit(int id, [Bind("Salary,WorkingDays,ContractFor,Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Jobs jobs)
        //        {
        //            if (id != jobs.Id)
        //            {
        //                return NotFound();
        //            }

        //            if (ModelState.IsValid)
        //            {
        //                try
        //                {
        //                    _context.Update(jobs);
        //                    await _context.SaveChangesAsync();
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    if (!JobsExists(jobs.Id))
        //                    {
        //                        return NotFound();
        //                    }
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //                return RedirectToAction(nameof(Index));
        //            }
        //            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", jobs.CreatedByUserId);
        //            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", jobs.SubCategoryId);
        //            return View(jobs);
        //        }

        //        // GET: Jobs/Delete/5
        //        public async Task<IActionResult> Delete(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var jobs = await _context.Jobs
        //                .Include(j => j.CreatedByUser)
        //                .Include(j => j.SubCategory)
        //                .SingleOrDefaultAsync(m => m.Id == id);
        //            if (jobs == null)
        //            {
        //                return NotFound();
        //            }

        //            return View(jobs);
        //        }

        //        // POST: Jobs/Delete/5
        //        [HttpPost, ActionName("Delete")]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> DeleteConfirmed(int id)
        //        {
        //            var jobs = await _context.Jobs.SingleOrDefaultAsync(m => m.Id == id);
        //            _context.Jobs.Remove(jobs);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }

        //        private bool JobsExists(int id)
        //        {
        //            return _context.Jobs.Any(e => e.Id == id);
        //        }
    }
}
