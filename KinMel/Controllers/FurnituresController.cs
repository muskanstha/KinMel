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

    public class FurnituresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FurnituresController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Furnitures
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Furnitures.Include(f => f.CreatedByUser).Include(f => f.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Furnitures/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnitures = await _context.Furnitures
                .Include(f => f.CreatedByUser)
                .Include(f => f.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (furnitures == null)
            {
                return NotFound();
            }

            return View(furnitures);
        }

        // GET: Furnitures/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Furnitures")), "Id", "Name");
            return View();
        }

        // POST: Furnitures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Furnitures furnitures, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    furnitures.CreatedByUserId = currentUserId;

                    furnitures.DateCreated = DateTime.Now;
                    _context.Add(furnitures);
                    await _context.SaveChangesAsync();

                    string forSlug = furnitures.Id + " " + String.Join(" ", furnitures.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    furnitures.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    furnitures.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Furnitures")), "Id", "Name", furnitures.SubCategoryId);
            return View(furnitures);
        }

        //// GET: Furnitures/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var furnitures = await _context.Furnitures.SingleOrDefaultAsync(m => m.Id == id);
        //    if (furnitures == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", furnitures.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", furnitures.SubCategoryId);
        //    return View(furnitures);
        //}

        //// POST: Furnitures/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Furnitures furnitures)
        //{
        //    if (id != furnitures.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(furnitures);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FurnituresExists(furnitures.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", furnitures.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", furnitures.SubCategoryId);
        //    return View(furnitures);
        //}

        //// GET: Furnitures/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var furnitures = await _context.Furnitures
        //        .Include(f => f.CreatedByUser)
        //        .Include(f => f.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (furnitures == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(furnitures);
        //}

        //// POST: Furnitures/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var furnitures = await _context.Furnitures.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Furnitures.Remove(furnitures);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool FurnituresExists(int id)
        //{
        //    return _context.Furnitures.Any(e => e.Id == id);
        //}
    }
}
