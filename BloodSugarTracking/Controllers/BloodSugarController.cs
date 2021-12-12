using Microsoft.Extensions.Options;

namespace BloodSugarTracking.Controllers;

[Authorize]
public class BloodSugarController : Controller
{
    private readonly BloodSugarContext _bloodSugarContext;
    private readonly IOptionsMonitor<BloodSugarOptions> _optionsAccessor;

    public BloodSugarController(
        BloodSugarContext bloodSugarContext,
        IOptionsMonitor<BloodSugarOptions> optionsAccessor)
    {
        _bloodSugarContext = bloodSugarContext;
        _optionsAccessor = optionsAccessor;
    }

    public IActionResult Index()
    {
        IList<BloodSugarTestResult> bloodSugarTestResults =
            _bloodSugarContext.BloodSugarTestResults!
                .Include(bloodSugarTestResult => bloodSugarTestResult.User)
                .OrderBy(bloodSugarTestResult => bloodSugarTestResult.TestTime)
                .ToList();

        ViewData["FastingNormal"] = _optionsAccessor.CurrentValue.FastingNormal;
        ViewData["TwoHoursNormal"] = _optionsAccessor.CurrentValue.TwoHoursNormal;

        return View(bloodSugarTestResults);
    }

    public IActionResult Create()
    {
        ViewData["Users"] = _bloodSugarContext.Users!
            .OrderBy(u => u.FirstName)
            .ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BloodSugarTestResult bloodSugarTestResult)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        bloodSugarTestResult.User = _bloodSugarContext.Users!
            .FirstOrDefault(u => u.Id == bloodSugarTestResult.UserId);
        if (bloodSugarTestResult.User == null)
        {
            return NotFound();
        }
        bloodSugarTestResult.UserId = null;
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

        ViewData["Users"] = _bloodSugarContext.Users!
            .OrderBy(u => u.FirstName)
            .ToList();

        return View(bloodSugarTestResult);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, BloodSugarTestResult bloodSugarTestResult)
    {
        if (id != bloodSugarTestResult.Id ||
            _bloodSugarContext.Users!
                .FirstOrDefault(u => u.Id == bloodSugarTestResult.UserId) == null)
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

        BloodSugarTestResult? bloodSugarTestResult =
            await _bloodSugarContext.BloodSugarTestResults!
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
        BloodSugarTestResult? bloodSugarTestResult =
            await _bloodSugarContext.BloodSugarTestResults!.FindAsync(id);

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
