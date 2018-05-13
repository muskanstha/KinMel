using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinMel.Data;
using KinMel.Models;
using KinMel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KinMel.Controllers.Categories
{
    [Authorize]
    public class TabletsAndIPadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TabletsAndIPadsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: TabletsAndIPads
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var tabletsAndIPads = from c in _context.TabletsAndIPads select c;
            switch (sortOrder)
            {
                case "Price":
                    tabletsAndIPads = tabletsAndIPads.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    tabletsAndIPads = tabletsAndIPads.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    tabletsAndIPads = tabletsAndIPads.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    tabletsAndIPads = tabletsAndIPads.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    tabletsAndIPads = tabletsAndIPads.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return View(await tabletsAndIPads.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync());
            //var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: TabletsAndIPads/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabletsAndIPads = await _context.TabletsAndIPads
                .Include(t => t.CreatedByUser)
                .Include(t => t.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tabletsAndIPads == null)
            {
                return NotFound();
            }

            return View(tabletsAndIPads);
        }

        // GET: TabletsAndIPads/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Car")), "Id", "Name");
            return View();
        }

        // POST: TabletsAndIPads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] TabletsAndIPads tabletsAndIPads, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    tabletsAndIPads.CreatedByUserId = currentUserId;

                    tabletsAndIPads.DateCreated = DateTime.Now;
                    _context.Add(tabletsAndIPads);
                    await _context.SaveChangesAsync();

                    string forSlug = tabletsAndIPads.Id + " " + String.Join(" ", tabletsAndIPads.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    tabletsAndIPads.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    tabletsAndIPads.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Car")), "Id", "Name", tabletsAndIPads.SubCategoryId);
            return View(tabletsAndIPads);
        }

        //// GET: TabletsAndIPads/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tabletsAndIPads = await _context.TabletsAndIPads.SingleOrDefaultAsync(m => m.Id == id);
        //    if (tabletsAndIPads == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", tabletsAndIPads.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", tabletsAndIPads.SubCategoryId);
        //    return View(tabletsAndIPads);
        //}

        //// POST: TabletsAndIPads/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] TabletsAndIPads tabletsAndIPads)
        //{
        //    if (id != tabletsAndIPads.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tabletsAndIPads);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TabletsAndIPadsExists(tabletsAndIPads.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", tabletsAndIPads.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", tabletsAndIPads.SubCategoryId);
        //    return View(tabletsAndIPads);
        //}

        //// GET: TabletsAndIPads/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tabletsAndIPads = await _context.TabletsAndIPads
        //        .Include(t => t.CreatedByUser)
        //        .Include(t => t.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (tabletsAndIPads == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tabletsAndIPads);
        //}

        //// POST: TabletsAndIPads/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var tabletsAndIPads = await _context.TabletsAndIPads.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.TabletsAndIPads.Remove(tabletsAndIPads);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TabletsAndIPadsExists(int id)
        //{
        //    return _context.TabletsAndIPads.Any(e => e.Id == id);
        //}
    }
}
