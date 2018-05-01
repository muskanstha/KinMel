using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inflector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using KinMel.Services;
using Microsoft.EntityFrameworkCore.Design;

namespace KinMel.Controllers
{
    public class ClassifiedAdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassifiedAdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClassifiedAds
        public async Task<IActionResult> Index()
        {
            //BlobStorageHelper.UploadBlobs();
            //string imageUris = await BlobStorageHelper.ListBlobsFolder("3-s8-like-for-sale");


            var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClassifiedAds/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var classifiedAd = await _context.ClassifiedAd
        //        .Include(c => c.CreatedByUser)
        //        .Include(c => c.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (classifiedAd == null)
        //    {
        //        return NotFound();
        //    }

        //    switch (classifiedAd.SubCategory.Name)
        //    {
        //        case "Car":
        //            return RedirectToAction("Details", "Cars", new { id = classifiedAd.Id });
        //        default:
        //            return View("Error");
        //    }
        //}

        // GET: ClassifiedAds/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classifiedAd = await _context.ClassifiedAd
                .Include(c => c.SubCategory).
                Include(c=> c.SubCategory.Category)
                .SingleOrDefaultAsync(m => m.Slug == id);
            if (classifiedAd == null)
            {
                return NotFound();
            }

            switch (classifiedAd.SubCategory.Category.Name)
            {
                case "Car":
                    var car = await _context.Car
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Cars/Details.cshtml", car);
                case "Mobile":
                    var mobile = await _context.Mobile
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Mobiles/Details.cshtml", mobile);
                default:
                    return View("Error");
            }
        }


        // GET: ClassifiedAds/Create
        public IActionResult Create()
        {
            ViewData["CategoryName"] = new SelectList(_context.Set<Category>(), "Name", "Name");
            return View();
        }

        // POST: ClassifiedAds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName")] ClassifiedAdCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Create", model.CategoryName.Pluralize());
            }
            ViewData["CategoryName"] = new SelectList(_context.Set<Category>(), "Name", "Name", model.CategoryName);

            return View(model);
        }

        //// GET: ClassifiedAds/Create
        //public IActionResult Create()
        //{
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id");
        //    return View();
        //}

        //// POST: ClassifiedAds/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive")] ClassifiedAd classifiedAd)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(classifiedAd);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", classifiedAd.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", classifiedAd.SubCategoryId);
        //    return View(classifiedAd);
        //}

        //// GET: ClassifiedAds/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var classifiedAd = await _context.ClassifiedAd.SingleOrDefaultAsync(m => m.Id == id);
        //    if (classifiedAd == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", classifiedAd.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", classifiedAd.SubCategoryId);
        //    return View(classifiedAd);
        //}

        //// POST: ClassifiedAds/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive")] ClassifiedAd classifiedAd)
        //{
        //    if (id != classifiedAd.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(classifiedAd);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClassifiedAdExists(classifiedAd.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", classifiedAd.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", classifiedAd.SubCategoryId);
        //    return View(classifiedAd);
        //}

        //// GET: ClassifiedAds/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var classifiedAd = await _context.ClassifiedAd
        //        .Include(c => c.CreatedByUser)
        //        .Include(c => c.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (classifiedAd == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(classifiedAd);
        //}

        //// POST: ClassifiedAds/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var classifiedAd = await _context.ClassifiedAd.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.ClassifiedAd.Remove(classifiedAd);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ClassifiedAdExists(int id)
        //{
        //    return _context.ClassifiedAd.Any(e => e.Id == id);
        //}
    }
}
