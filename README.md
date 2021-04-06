# Blood sugar tracking app

[![CI CD](https://github.com/Arnab-Developer/BloodSugarTracking/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/Arnab-Developer/BloodSugarTracking/actions/workflows/ci-cd.yml)
![Docker Image Version (latest by date)](https://img.shields.io/docker/v/45862391/bloodsugartracking)

User can enter blood sugar data for fasting and PP and see the already added data in this app.

Add blood sugar data

![image](https://user-images.githubusercontent.com/3396447/113702889-be604200-96f7-11eb-9753-2245c65f8f52.png)

See existing blood sugar data

![image](https://user-images.githubusercontent.com/3396447/113703098-fc5d6600-96f7-11eb-9787-f8101220117f.png)

It is an ASP.NET 5 mvc app with EF to enter and show blood sugar data for fasting and PP. 

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

If the blood sugar data is up from normal then it shows the data as red.

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

## Docker image

This app is in a docker image and stored in dockerhub.

https://hub.docker.com/r/45862391/bloodsugartracking

## Contributing

Read about contributing related things [here](https://github.com/Arnab-Developer/BloodSugarTracking/blob/main/CONTRIBUTING.md).
