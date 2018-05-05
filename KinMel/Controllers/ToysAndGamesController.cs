﻿using System;
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
    public class ToysAndGamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ToysAndGamesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToysAndGames
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ToysAndGames.Include(t => t.CreatedByUser).Include(t => t.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ToysAndGames/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toysAndGames = await _context.ToysAndGames
                .Include(t => t.CreatedByUser)
                .Include(t => t.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (toysAndGames == null)
            {
                return NotFound();
            }

            return View(toysAndGames);
        }

        // GET: ToysAndGames/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ToysAndGames")), "Id", "Name");
            return View();
        }

        // POST: ToysAndGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] ToysAndGames toysAndGames, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    toysAndGames.CreatedByUserId = currentUserId;

                    toysAndGames.DateCreated = DateTime.Now;
                    _context.Add(toysAndGames);
                    await _context.SaveChangesAsync();

                    string forSlug = toysAndGames.Id + " " + String.Join(" ", toysAndGames.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    toysAndGames.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    toysAndGames.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ToysAndGames")), "Id", "Name", toysAndGames.SubCategoryId);
            return View(toysAndGames);
        }

        //// GET: ToysAndGames/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var toysAndGames = await _context.ToysAndGames.SingleOrDefaultAsync(m => m.Id == id);
        //    if (toysAndGames == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", toysAndGames.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", toysAndGames.SubCategoryId);
        //    return View(toysAndGames);
        //}

        //// POST: ToysAndGames/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] ToysAndGames toysAndGames)
        //{
        //    if (id != toysAndGames.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(toysAndGames);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ToysAndGamesExists(toysAndGames.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", toysAndGames.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", toysAndGames.SubCategoryId);
        //    return View(toysAndGames);
        //}

        //// GET: ToysAndGames/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var toysAndGames = await _context.ToysAndGames
        //        .Include(t => t.CreatedByUser)
        //        .Include(t => t.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (toysAndGames == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(toysAndGames);
        //}

        //// POST: ToysAndGames/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var toysAndGames = await _context.ToysAndGames.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.ToysAndGames.Remove(toysAndGames);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ToysAndGamesExists(int id)
        //{
        //    return _context.ToysAndGames.Any(e => e.Id == id);
        //}
    }
}