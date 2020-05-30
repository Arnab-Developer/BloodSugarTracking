using BloodSugarTracking.Data;
using BloodSugarTracking.Models;
using BloodSugarTracking.Options;
using Microsoft.AspNetCore.Mvc;
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
            var bloodSugarTestResults = _bloodSugarContext.BloodSugarTestResults
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
    }
}
