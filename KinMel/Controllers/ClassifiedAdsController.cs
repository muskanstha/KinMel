using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public readonly ApplicationDbContext _context;
       

        public ClassifiedAdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClassifiedAds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ClassifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            //var applicationDbContext = _context.ClassifiedAd.Where(c => c.IsActive && !c.IsSold).Include(c => c.CreatedByUser).Include(c => c.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        //filter
        public ActionResult Search(ClassifiedAdSearchModel searchModel)
        {
            var filter = new ClassifiedAdLogic(_context);
            var model = filter.GetProducts(searchModel);
            return View(model);

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
                case "Motorcycle":
                    var motorcycle = await _context.Motorcycle
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Motorcycles/Details.cshtml", motorcycle);
                case "Realstate":
                    var realState = await _context.RealState
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/RealStates/Details.cshtml", realState);
                case "Computer":
                    var computer = await _context.Computer
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Computers/Details.cshtml", computer);
                case "Jobs":
                    var jobs = await _context.Jobs
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Jobs/Details.cshtml", jobs);
                case "BeautyAndHealth":
                    var beautyandhealth = await _context.BeautyAndHealth
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/BeautyAndHealths/Details.cshtml", beautyandhealth);
                case "BooksAndLearning":
                    var booksAndLearning = await _context.BooksAndLearning
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/BooksAndLearnings/Details.cshtml", booksAndLearning);
                case "Electronics":
                    var electronics = await _context.Electronics
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Electronics/Details.cshtml", electronics);
                case "Furnitures":
                    var furnitures = await _context.Furnitures
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Furnitures/Details.cshtml", furnitures);
                case "Camera":
                    var camera = await _context.Camera
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/Cameras/Details.cshtml", camera);
                case "MusicInstruments":
                    var musicInstruments = await _context.MusicInstruments
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/MusicInstruments/Details.cshtml", musicInstruments);
                case "PetsAndPetCare":
                    var petsAndPetCare = await _context.PetsAndPetCare
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/PetsAndPetCares/Details.cshtml", petsAndPetCare);
                case "SportsAndFitness":
                    var sportsAndFitness = await _context.SportsAndFitness
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/SportsAndFitnesses/Details.cshtml", sportsAndFitness);
                case "TabletsAndIPads":
                    var tabletsAndIPads = await _context.TabletsAndIPads
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/TabletsAndIPads/Details.cshtml", tabletsAndIPads);
                case "ToysAndGames":
                    var toysAndGames = await _context.ToysAndGames
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/ToysAndGames/Details.cshtml", toysAndGames);
                case "TravelAndTours":
                    var travelAndTours = await _context.TravelAndTours
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/TravelAndTours/Details.cshtml", travelAndTours);
                case "HelpAndServices":
                    var helpAndServices = await _context.HelpAndServices
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/HelpAndServices/Details.cshtml", helpAndServices);
                case "MobileAccessories":
                    var mobileAccessories = await _context.MobileAccessories
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/MobileAccessories/Details.cshtml", mobileAccessories);
                case "ComputerParts":
                    var computerParts = await _context.ComputerParts
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/ComputerParts/Details.cshtml", computerParts);
                case "ApparelsAndAccessories":
                    var apparelsAndAccessories = await _context.ApparelsAndAccessories
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/ApparelsAndAccessories/Details.cshtml", apparelsAndAccessories);
                case "VehiclesParts":
                    var vehiclesParts = await _context.VehiclesParts
                        .Include(c => c.CreatedByUser)
                        .Include(c => c.SubCategory)
                        .SingleOrDefaultAsync(m => m.Slug == id);
                    return View("~/Views/VehiclesParts/Details.cshtml", vehiclesParts);
                default:
                    return View("Error");
            }
        }


        // GET: ClassifiedAds/Create
        public IActionResult Create()
        {
            ViewData["CategoryName"] = new SelectList(_context.Set<Category>().OrderBy(c => c.Name), "Name", "Name");
            return View();
        }

        // POST: ClassifiedAds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CategoryName")] ClassifiedAdCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Create", model.CategoryName.Pluralize());
            }
            ViewData["CategoryName"] = new SelectList(_context.Set<Category>().OrderBy(c => c.Name), "Name", "Name", model.CategoryName);

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
