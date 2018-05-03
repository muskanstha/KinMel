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
    public class ApparelsAndAccessoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApparelsAndAccessoriesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApparelsAndAccessories
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ApparelsAndAccessories.Include(a => a.CreatedByUser).Include(a => a.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ApparelsAndAccessories/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apparelsAndAccessories = await _context.ApparelsAndAccessories
                .Include(a => a.CreatedByUser)
                .Include(a => a.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (apparelsAndAccessories == null)
            {
                return NotFound();
            }

            return View(apparelsAndAccessories);
        }

        // GET: ApparelsAndAccessories/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ApparelsAndAccessories")), "Id", "Name");
            return View();
        }

        // POST: ApparelsAndAccessories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] ApparelsAndAccessories apparelsAndAccessories, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    apparelsAndAccessories.CreatedByUserId = currentUserId;

                    apparelsAndAccessories.DateCreated = DateTime.Now;
                    _context.Add(apparelsAndAccessories);
                    await _context.SaveChangesAsync();

                    string forSlug = apparelsAndAccessories.Id + " " + String.Join(" ", apparelsAndAccessories.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    apparelsAndAccessories.Slug = slug;

                    await BlobStorageHelper.UploadBlobs(slug, imageFiles);

                    apparelsAndAccessories.ImageUrls = await BlobStorageHelper.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("ApparelsAndAccessories")), "Id", "Name", apparelsAndAccessories.SubCategoryId);
            return View(apparelsAndAccessories);
        }

        //// GET: ApparelsAndAccessories/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var apparelsAndAccessories = await _context.ApparelsAndAccessories.SingleOrDefaultAsync(m => m.Id == id);
        //    if (apparelsAndAccessories == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", apparelsAndAccessories.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", apparelsAndAccessories.SubCategoryId);
        //    return View(apparelsAndAccessories);
        //}

        //// POST: ApparelsAndAccessories/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] ApparelsAndAccessories apparelsAndAccessories)
        //{
        //    if (id != apparelsAndAccessories.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(apparelsAndAccessories);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ApparelsAndAccessoriesExists(apparelsAndAccessories.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", apparelsAndAccessories.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", apparelsAndAccessories.SubCategoryId);
        //    return View(apparelsAndAccessories);
        //}

        //// GET: ApparelsAndAccessories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var apparelsAndAccessories = await _context.ApparelsAndAccessories
        //        .Include(a => a.CreatedByUser)
        //        .Include(a => a.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (apparelsAndAccessories == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(apparelsAndAccessories);
        //}

        //// POST: ApparelsAndAccessories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var apparelsAndAccessories = await _context.ApparelsAndAccessories.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.ApparelsAndAccessories.Remove(apparelsAndAccessories);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ApparelsAndAccessoriesExists(int id)
        //{
        //    return _context.ApparelsAndAccessories.Any(e => e.Id == id);
        //}
    }
}
