using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using KinMel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers
{
    [Authorize]
    public class SportsAndFitnessesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SportsAndFitnessesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SportsAndFitnesses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SportsAndFitness.Include(s => s.CreatedByUser).Include(s => s.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SportsAndFitnesses/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportsAndFitness = await _context.SportsAndFitness
                .Include(s => s.CreatedByUser)
                .Include(s => s.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sportsAndFitness == null)
            {
                return NotFound();
            }

            return View(sportsAndFitness);
        }

        // GET: SportsAndFitnesses/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("SportsAndFitness")), "Id", "Name");
            return View();
        }

        // POST: SportsAndFitnesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] SportsAndFitness sportsAndFitness, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    sportsAndFitness.CreatedByUserId = currentUserId;

                    sportsAndFitness.DateCreated = DateTime.Now;
                    _context.Add(sportsAndFitness);
                    await _context.SaveChangesAsync();

                    string forSlug = sportsAndFitness.Id + " " + String.Join(" ", sportsAndFitness.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    sportsAndFitness.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    sportsAndFitness.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("SportsAndFitness")), "Id", "Name", sportsAndFitness.SubCategoryId);
            return View(sportsAndFitness);
        }

        //// GET: SportsAndFitnesses/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sportsAndFitness = await _context.SportsAndFitness.SingleOrDefaultAsync(m => m.Id == id);
        //    if (sportsAndFitness == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", sportsAndFitness.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", sportsAndFitness.SubCategoryId);
        //    return View(sportsAndFitness);
        //}

        //// POST: SportsAndFitnesses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] SportsAndFitness sportsAndFitness)
        //{
        //    if (id != sportsAndFitness.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(sportsAndFitness);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SportsAndFitnessExists(sportsAndFitness.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", sportsAndFitness.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", sportsAndFitness.SubCategoryId);
        //    return View(sportsAndFitness);
        //}

        //// GET: SportsAndFitnesses/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sportsAndFitness = await _context.SportsAndFitness
        //        .Include(s => s.CreatedByUser)
        //        .Include(s => s.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (sportsAndFitness == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(sportsAndFitness);
        //}

        //// POST: SportsAndFitnesses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var sportsAndFitness = await _context.SportsAndFitness.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.SportsAndFitness.Remove(sportsAndFitness);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool SportsAndFitnessExists(int id)
        //{
        //    return _context.SportsAndFitness.Any(e => e.Id == id);
        //}
    }
}
