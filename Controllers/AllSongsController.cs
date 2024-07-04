using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oshipple.Data;
using Oshipple.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Oshipple.Controllers
{
    public class AllSongsController : Controller
    {
        private readonly OshippleContext _context;

        public AllSongsController(OshippleContext context)
        {
            _context = context;
        }

        // GET: AllSongs
        /*
        public async Task<IActionResult> AdSongs()
        {
           return View(await _context.AllSongs.ToListAsync());
        }
        */

        public async Task<IActionResult> AdSongs(int? pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            var musicDetails = from a in _context.AllSongs select a;

            int pageSize = 25;
            return View(await PaginatedList<AllSongs>.CreateAsync(musicDetails.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Songs(string sortOrder, string mood, string selector, int? pageNumber)
        {
            ViewData["ListTitle"] = "タイトル";
            ViewData["ListArtist"] = "アーティスト";
            ViewData["ListRank"] = "推し度";
            ViewData["hyoudai"] = "全体";

            ViewData["SortParm"] = sortOrder;

            ViewData["hyoudai"] = "All Songs";


            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            var musicDetails = from a in _context.AllSongs select a;

            if (!string.IsNullOrEmpty(mood))
            {
                musicDetails = musicDetails.Where(m => m.Mood1 == mood || m.Mood2 == mood);
                ViewData["MoodParm"] = mood;
                ViewData["hyoudai"] = "Mood : ";
            }

            if (!string.IsNullOrEmpty(selector))
            {
                musicDetails = musicDetails.Where(m => m.Selector == selector);
                ViewData["SelectorParm"] = selector;
                ViewData["hyoudai"] = "Selector : ";
            }

            switch (sortOrder)
            {
                case "title_desc":
                    musicDetails = musicDetails.OrderByDescending(md => md.Title_Kana);
                    ViewData["ListTitle"] = "タイトル▼";
                    break;
                case "artist":
                    musicDetails = musicDetails.OrderBy(md => md.Artist_Kana);
                    ViewData["ListArtist"] = "アーティスト▲";
                    break;
                case "artist_desc":
                    musicDetails = musicDetails.OrderByDescending(md => md.Artist_Kana);
                    ViewData["ListArtist"] = "アーティスト▼";
                    break;
                case "rank":
                    musicDetails = musicDetails.OrderBy(md => md.Rank);
                    ViewData["ListRank"] = "推し度▲";
                    break;
                case "rank_desc":
                    musicDetails = musicDetails.OrderByDescending(md => md.Rank);
                    ViewData["ListRank"] = "推し度▼";
                    break;
                default:
                    musicDetails = musicDetails.OrderBy(md => md.Title_Kana);
                    ViewData["ListTitle"] = "タイトル▲";
                    break;
            }

            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["ArtistSortParm"] = sortOrder == "artist" ? "artist_desc" : "artist";
            ViewData["RankSortParm"] = sortOrder == "rank" ? "rank_desc" : "rank";

            int pageSize = 20;
            return View(await PaginatedList<AllSongs>.CreateAsync(musicDetails.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: AllSongs/Details/5
        public async Task<IActionResult> Details(string sortOrder, string mood, string selector, int? pageNumber, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allSongs = await _context.AllSongs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allSongs == null)
            {
                return NotFound();
            }
            ViewData["SortParm"] = sortOrder;
            ViewData["MoodParm"] = mood;
            ViewData["SelectorParm"] = selector;
            ViewData["PageIndex"] = pageNumber;

            return View(allSongs);
        }

        public async Task<IActionResult> AdDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allSongs = await _context.AllSongs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allSongs == null)
            {
                return NotFound();
            }

            return View(allSongs);
        }

        // GET: AllSongs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AllSongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Title_Kana,Artist,Artist_Kana,Selector,Mood1,Mood2,Rank,Comment")] AllSongs allSongs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allSongs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdSongs));
            }
            return View(allSongs);
        }

        // GET: AllSongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allSongs = await _context.AllSongs.FindAsync(id);
            if (allSongs == null)
            {
                return NotFound();
            }
            return View(allSongs);
        }

        // POST: AllSongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Title_Kana,Artist,Artist_Kana,Selector,Mood1,Mood2,Rank,Comment")] AllSongs allSongs)
        {
            if (id != allSongs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allSongs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllSongsExists(allSongs.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdSongs));
            }
            return View(allSongs);
        }

        // GET: AllSongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allSongs = await _context.AllSongs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allSongs == null)
            {
                return NotFound();
            }

            return View(allSongs);
        }

        // POST: AllSongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allSongs = await _context.AllSongs.FindAsync(id);
            if (allSongs != null)
            {
                _context.AllSongs.Remove(allSongs);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdSongs));
        }

        private bool AllSongsExists(int id)
        {
            return _context.AllSongs.Any(e => e.Id == id);
        }
    }
}
