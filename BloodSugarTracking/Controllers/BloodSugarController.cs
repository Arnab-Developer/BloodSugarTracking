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
                .OrderBy(bloodSugarTestResult => bloodSugarTestResult.TestTime);

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
            _bloodSugarContext.BloodSugarTestResults!.Add(bloodSugarTestResult);
            await _bloodSugarContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodSugarTestResult = _bloodSugarContext.BloodSugarTestResults!.Find(id);
            if (bloodSugarTestResult == null)
            {
                return NotFound();
            }
            return View(bloodSugarTestResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BloodSugarTestResult bloodSugarTestResult)
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
                    _bloodSugarContext.SaveChanges();
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

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _bloodSugarContext.BloodSugarTestResults!
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _bloodSugarContext.BloodSugarTestResults!.Find(id);
            _bloodSugarContext.BloodSugarTestResults!.Remove(student);
            _bloodSugarContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloodSugarTestResultExists(int id)
        {
            return _bloodSugarContext.BloodSugarTestResults!.Any(e => e.Id == id);
        }
    }
}
