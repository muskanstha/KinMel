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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MusicInstruments.Include(m => m.CreatedByUser).Include(m => m.SubCategory);
            return View(await applicationDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] MusicInstruments musicInstruments, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    musicInstruments.CreatedByUserId = currentUserId;

                    musicInstruments.DateCreated = DateTime.Now;
                    _context.Add(musicInstruments);
                    await _context.SaveChangesAsync();

                    string forSlug = musicInstruments.Id + " " + String.Join(" ", musicInstruments.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    musicInstruments.Slug = slug;

                    await BlobStorageHelper.UploadBlobs(slug, imageFiles);

                    musicInstruments.ImageUrls = await BlobStorageHelper.ListBlobsFolder(slug);

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
