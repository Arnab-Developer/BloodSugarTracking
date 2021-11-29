using System.ComponentModel.DataAnnotations;

namespace BloodSugarTracking.Models;

public class BloodSugarTestResult
{
    public int Id { get; set; }

    [Display(Name = "Meal time")]
    public DateTime MealTime { get; set; }

    [Display(Name = "Test time")]
    public DateTime TestTime { get; set; }

    [Display(Name = "Blood sugar test result")]
    public double Result { get; set; }

    public int? UserId { get; set; }

    public User? User { get; set; }

    public string TimeDurationAfterLastMeal
    {
        get
        {
            TimeSpan timeDurationAfterLastMeal = TestTime.Subtract(MealTime);
            return $"{timeDurationAfterLastMeal.Hours} hour {timeDurationAfterLastMeal.Minutes} minute after last meal";
        }
    }

    public bool IsHigh(int fastingNormal, int twoHoursNormal)
    {
        TimeSpan timeDurationAfterLastMeal = TestTime.Subtract(MealTime);
        if (timeDurationAfterLastMeal.Hours >= 8 && Result >= fastingNormal)
        {
            return true;
        }
        if (timeDurationAfterLastMeal.Hours < 8 && Result >= twoHoursNormal)
        {
            return true;
        }
        return false;
    }
}
