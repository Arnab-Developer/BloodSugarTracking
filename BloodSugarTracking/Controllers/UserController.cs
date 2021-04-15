using BloodSugarTracking.Data;
using BloodSugarTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodSugarTracking.Controllers
{
    public class UserController : Controller
    {
        private readonly BloodSugarContext _bloodSugarContext;

        public UserController(BloodSugarContext bloodSugarContext)
        {
            _bloodSugarContext = bloodSugarContext;
        }

        public IActionResult Index()
        {
            IList<User> users = _bloodSugarContext.Users!
                .OrderBy(u => u.FirstName)
                .ToList();

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _bloodSugarContext.Users!.AddAsync(user);
            await _bloodSugarContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _bloodSugarContext.Users!.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bloodSugarContext.Users!.Update(user);
                    await _bloodSugarContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _bloodSugarContext.Users!
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User user = await _bloodSugarContext.Users!.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _bloodSugarContext.Users!.Remove(user);
            await _bloodSugarContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _bloodSugarContext.Users!.Any(e => e.Id == id);
        }
    }
}
