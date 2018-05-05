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

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PetsAndPetCare.Include(p => p.CreatedByUser).Include(p => p.SubCategory);
            return View(await applicationDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] PetsAndPetCare petsAndPetCare, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    petsAndPetCare.CreatedByUserId = currentUserId;

                    petsAndPetCare.DateCreated = DateTime.Now;
                    _context.Add(petsAndPetCare);
                    await _context.SaveChangesAsync();

                    string forSlug = petsAndPetCare.Id + " " + String.Join(" ", petsAndPetCare.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    petsAndPetCare.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    petsAndPetCare.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

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
