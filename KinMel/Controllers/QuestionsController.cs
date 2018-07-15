using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Question.Include(q => q.ClassifiedAd).Include(q => q.CreatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.ClassifiedAd)
                .Include(q => q.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        //// GET: Questions/Create
        //public IActionResult Create()
        //{
        //    ViewData["ClassifiedAdId"] = new SelectList(_context.ClassifiedAd, "Id", "Condition");
        //    ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Text,ClassifiedAdId")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.DateCreated = DateTimeOffset.UtcNow;

                string currentUserId = _userManager.GetUserId(User);
                question.CreatedById = currentUserId;
                _context.Add(question);
                await _context.SaveChangesAsync();
                var classifiedAd = await _context.ClassifiedAd.FirstOrDefaultAsync(c => c.Id.Equals(question.ClassifiedAdId));
                return RedirectToAction("Details", "ClassifiedAds", new { id = classifiedAd.Slug });
            }

            return View("Info");
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["ClassifiedAdId"] = new SelectList(_context.ClassifiedAd, "Id", "Condition", question.ClassifiedAdId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", question.CreatedById);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ClassifiedAdId,CreatedById,DateCreated")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassifiedAdId"] = new SelectList(_context.ClassifiedAd, "Id", "Condition", question.ClassifiedAdId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", question.CreatedById);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.ClassifiedAd)
                .Include(q => q.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
