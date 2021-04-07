using BloodSugarTracking.Data;
using BloodSugarTracking.Models;
using BloodSugarTracking.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace BloodSugarTracking.Controllers
{
    public class BloodSugarController : Controller
    {
        private readonly BloodSugarContext _bloodSugarContext;
        private readonly BloodSugarOptions _bloodSugarOptions;

        public BloodSugarController(
            BloodSugarContext bloodSugarContext,
            IOptionsMonitor<BloodSugarOptions> optionsAccessor)
        {
            _bloodSugarContext = bloodSugarContext;
            _bloodSugarOptions = optionsAccessor.CurrentValue;
        }

        public IActionResult Index()
        {
            var bloodSugarTestResults = _bloodSugarContext.BloodSugarTestResults!
                .OrderBy(bloodSugarTestResult => bloodSugarTestResult.TestTime)
                .ToList();

            ViewData["FastingNormal"] = _bloodSugarOptions.FastingNormal;
            ViewData["TwoHoursNormal"] = _bloodSugarOptions.TwoHoursNormal;

            return View(bloodSugarTestResults);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BloodSugarTestResult bloodSugarTestResult)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _bloodSugarContext.BloodSugarTestResults!.AddAsync(bloodSugarTestResult);
            await _bloodSugarContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodSugarTestResult = await _bloodSugarContext.BloodSugarTestResults!.FindAsync(id);
            if (bloodSugarTestResult == null)
            {
                return NotFound();
            }
            return View(bloodSugarTestResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BloodSugarTestResult bloodSugarTestResult)
        {
            if (id != bloodSugarTestResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bloodSugarContext.BloodSugarTestResults!.Update(bloodSugarTestResult);
                    await _bloodSugarContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloodSugarTestResultExists(bloodSugarTestResult.Id))
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
            return View(bloodSugarTestResult);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodSugarTestResult = await _bloodSugarContext.BloodSugarTestResults!
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bloodSugarTestResult == null)
            {
                return NotFound();
            }

            return View(bloodSugarTestResult);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bloodSugarTestResult = await _bloodSugarContext.BloodSugarTestResults!.FindAsync(id);
            if (bloodSugarTestResult == null)
            {
                return NotFound();
            }
            _bloodSugarContext.BloodSugarTestResults!.Remove(bloodSugarTestResult);
            await _bloodSugarContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloodSugarTestResultExists(int id)
        {
            return _bloodSugarContext.BloodSugarTestResults!.Any(e => e.Id == id);
        }
    }
}
