using BloodSugarTracking.Models;
using Xunit;

namespace BloodSugarTrackingTest;

public class BloodSugarTestResultTest
{
    [Fact]
    public void Can_TimeDurationAfterLastMeal_ReturnProperData()
    {
        BloodSugarTestResult bloodSugarTestResult = new()
        {
            MealTime = DateTime.UtcNow.AddHours(-12),
            TestTime = DateTime.UtcNow,
            Result = 100.6
        };

        Assert.Equal("12 hour 0 minute after last meal", bloodSugarTestResult.TimeDurationAfterLastMeal);
    }

    [Fact]
    public void Can_IsHigh_ReturnProperDataForFastingTrue()
    {
        BloodSugarTestResult bloodSugarTestResult = new()
        {
            MealTime = DateTime.UtcNow.AddHours(-9),
            TestTime = DateTime.UtcNow,
            Result = 110
        };

        bool isHigh = bloodSugarTestResult.IsHigh(100, 140);

        Assert.True(isHigh);
    }

    [Fact]
    public void Can_IsHigh_ReturnProperDataForFastingFalse()
    {
        BloodSugarTestResult bloodSugarTestResult = new()
        {
            MealTime = DateTime.UtcNow.AddHours(-9),
            TestTime = DateTime.UtcNow,
            Result = 98
        };

        bool isHigh = bloodSugarTestResult.IsHigh(100, 140);

        Assert.False(isHigh);
    }

    [Fact]
    public void Can_IsHigh_ReturnProperDataForPPTrue()
    {
        BloodSugarTestResult bloodSugarTestResult = new()
        {
            MealTime = DateTime.UtcNow.AddHours(-2),
            TestTime = DateTime.UtcNow,
            Result = 155
        };

        bool isHigh = bloodSugarTestResult.IsHigh(100, 140);

        Assert.True(isHigh);
    }

    [Fact]
    public void Can_IsHigh_ReturnProperDataForPPFalse()
    {
        BloodSugarTestResult bloodSugarTestResult = new()
        {
            MealTime = DateTime.UtcNow.AddHours(-2),
            TestTime = DateTime.UtcNow,
            Result = 135
        };

        bool isHigh = bloodSugarTestResult.IsHigh(100, 140);

        Assert.False(isHigh);
    }
}
