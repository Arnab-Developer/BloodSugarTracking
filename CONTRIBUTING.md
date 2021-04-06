Contributions are welcome. Please create a new issue or pick an existing 
issue to work on. Let me know on which issue you are going to work before 
raising a pull request.

You need Visual Studio 2019, ASP.NET Core 3.1 and SQL Server to work on this app.
Create database `BloodSugarDb` and table `BloodSugarTestResults` locally to start 
working as per the below model.

```c#
public class BloodSugarTestResult
{
    public int Id { get; set; }

    [Display(Name = "Meal time")]
    public DateTime MealTime { get; set; }

    [Display(Name = "Test time")]
    public DateTime TestTime { get; set; }

    [Display(Name = "Blood sugar test result")]
    public double Result { get; set; }
 
    // more code...
}    
```
