# Blood sugar tracking app

ASP.NET 5 mvc app with EF to enter and show blood sugar data for 
fasting and PP.

```c#
// Show existing blood sugar data
public IActionResult Index()
{
    var bloodSugarTestResults = _bloodSugarContext.BloodSugarTestResults
        .OrderBy(bloodSugarTestResult => bloodSugarTestResult.TestTime);

    ViewData["FastingNormal"] = _bloodSugarOptions.FastingNormal;
    ViewData["TwoHoursNormal"] = _bloodSugarOptions.TwoHoursNormal;

    return View(bloodSugarTestResults);
}

// Create new blood sugar data
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
```

If the sugar data is up from normal then it shows the data as red.

```c#
@if (bloodSugarTestResult.IsHigh((int)ViewData["FastingNormal"], (int)ViewData["TwoHoursNormal"]))
{
    <span style="color: red">@bloodSugarTestResult.Result</span>
}
else
{
    @bloodSugarTestResult.Result
}
```

The normal range of blood sugar is mentioned in the appsettings.json file.

```json
{
  "FastingNormal": 100,
  "TwoHoursNormal": 140,
}
```
## Screenshot

![image](https://user-images.githubusercontent.com/3396447/113700018-f1a0d200-96f3-11eb-85e2-6f6a7eb72523.png)

## Limitation

Currently no update or delete functionality is present.
