using BloodSugarTracking.Controllers;
using BloodSugarTracking.Data;
using BloodSugarTracking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using BloodSugarTracking.Options;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodSugarTrackingTest
{
    public class BloodSugarControllerTest
    {
        [Fact]
        public void Can_Index_WorkProperly()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                BloodSugarTestResult bloodSugarTestResult = new()
                {
                    MealTime = DateTime.UtcNow.AddDays(10).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 100.6
                };
                bloodSugarSetupContext.BloodSugarTestResults!.Add(bloodSugarTestResult);
                bloodSugarSetupContext.SaveChanges();
            }

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                Mock<IOptionsMonitor<BloodSugarOptions>> mock = new();
                mock.Setup(s => s.CurrentValue)
                    .Returns(new BloodSugarOptions() { FastingNormal = 100, TwoHoursNormal = 140 });
                BloodSugarController bloodSugarController = new(bloodSugarTestContext, mock.Object);

                ViewResult? viewResult = bloodSugarController.Index() as ViewResult;
                
                Assert.NotNull(viewResult);
                Assert.NotNull(viewResult!.Model);

                IList<BloodSugarTestResult>? bloodSugarTestResults = viewResult.Model as IList<BloodSugarTestResult>;

                Assert.NotNull(bloodSugarTestResults);
                Assert.Equal(DateTime.UtcNow.AddDays(10).Date, bloodSugarTestResults![0].MealTime);
                Assert.Equal(DateTime.UtcNow.Date, bloodSugarTestResults[0].TestTime);
                Assert.Equal(100.6, bloodSugarTestResults[0].Result);

                bloodSugarTestContext.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task Can_Create_WorkProperly()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                Mock<IOptionsMonitor<BloodSugarOptions>> mock = new();
                mock.Setup(s => s.CurrentValue)
                    .Returns(new BloodSugarOptions() { FastingNormal = 100, TwoHoursNormal = 140 });
                BloodSugarController bloodSugarController = new(bloodSugarSetupContext, mock.Object);

                BloodSugarTestResult bloodSugarTestResult = new()
                {
                    MealTime = DateTime.UtcNow.AddDays(10).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 100.6
                };

                await bloodSugarController.Create(bloodSugarTestResult);
            }

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                BloodSugarTestResult? bloodSugarTestResult = bloodSugarTestContext.BloodSugarTestResults!
                    .SingleOrDefault(b => b.MealTime == DateTime.UtcNow.AddDays(10).Date);

                Assert.NotNull(bloodSugarTestResult);
                Assert.Equal(DateTime.UtcNow.AddDays(10).Date, bloodSugarTestResult!.MealTime);
                Assert.Equal(DateTime.UtcNow.Date, bloodSugarTestResult.TestTime);
                Assert.Equal(100.6, bloodSugarTestResult.Result);

                bloodSugarTestContext.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task Can_Edit_WorkProperly()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                BloodSugarTestResult bloodSugarTestResult = new()
                {
                    MealTime = DateTime.UtcNow.AddDays(10).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 100.6
                };
                bloodSugarSetupContext.BloodSugarTestResults!.Add(bloodSugarTestResult);
                bloodSugarSetupContext.SaveChanges();
            }

            using (BloodSugarContext bloodSugarSetupContext1 = new(options))
            { 
                Mock<IOptionsMonitor<BloodSugarOptions>> mock = new();
                mock.Setup(s => s.CurrentValue)
                    .Returns(new BloodSugarOptions() { FastingNormal = 100, TwoHoursNormal = 140 });
                BloodSugarController bloodSugarController = new(bloodSugarSetupContext1, mock.Object);

                BloodSugarTestResult bloodSugarTestResultUpdated = new()
                {
                    Id = 1,
                    MealTime = DateTime.UtcNow.AddDays(15).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 205.4
                };

                await bloodSugarController.Edit(1, bloodSugarTestResultUpdated);
            }

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                BloodSugarTestResult? bloodSugarTestResult = bloodSugarTestContext.BloodSugarTestResults!
                    .SingleOrDefault(b => b.MealTime == DateTime.UtcNow.AddDays(15).Date);

                Assert.NotNull(bloodSugarTestResult);
                Assert.Equal(DateTime.UtcNow.AddDays(15).Date, bloodSugarTestResult!.MealTime);
                Assert.Equal(DateTime.UtcNow.Date, bloodSugarTestResult.TestTime);
                Assert.Equal(205.4, bloodSugarTestResult.Result);

                bloodSugarTestContext.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task Can_Delete_WorkProperly()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                BloodSugarTestResult bloodSugarTestResult = new()
                {
                    MealTime = DateTime.UtcNow.AddDays(10).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 100.6
                };
                bloodSugarSetupContext.BloodSugarTestResults!.Add(bloodSugarTestResult);
                bloodSugarSetupContext.SaveChanges();
            }

            using (BloodSugarContext bloodSugarSetupContext1 = new(options))
            {
                Mock<IOptionsMonitor<BloodSugarOptions>> mock = new();
                mock.Setup(s => s.CurrentValue)
                    .Returns(new BloodSugarOptions() { FastingNormal = 100, TwoHoursNormal = 140 });
                BloodSugarController bloodSugarController = new(bloodSugarSetupContext1, mock.Object);

                await bloodSugarController.DeleteConfirmed(1);
            }

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                Assert.Equal(0, bloodSugarTestContext.BloodSugarTestResults!.Count());

                bloodSugarTestContext.Database.EnsureDeleted();
            }
        }
    }
}
