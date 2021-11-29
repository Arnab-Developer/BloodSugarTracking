using BloodSugarTracking.Controllers;
using BloodSugarTracking.Data;
using BloodSugarTracking.Models;
using BloodSugarTracking.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BloodSugarTrackingTest
{
    public class BloodSugarControllerTest : IDisposable
    {
        private bool _disposedValue;

        [Fact]
        public void Is_Index_WorkProperly()
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
            }
        }

        [Fact]
        public void Is_Index_WorkProperlyWithUserData()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                User user = new()
                {
                    FirstName = "Jon",
                    LastName = "Doe"
                };
                bloodSugarSetupContext.Users!.Add(user);
                bloodSugarSetupContext.SaveChanges();
            }

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                User user = bloodSugarSetupContext.Users!.First(u => u.FirstName == "Jon");
                BloodSugarTestResult bloodSugarTestResult = new()
                {
                    MealTime = DateTime.UtcNow.AddDays(10).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 100.6,
                    User = user
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
                Assert.Equal(1, bloodSugarTestResults[0].UserId);
                Assert.Equal(1, bloodSugarTestResults[0].User!.Id);
                Assert.Equal("Jon", bloodSugarTestResults[0].User!.FirstName);
                Assert.Equal("Doe", bloodSugarTestResults[0].User!.LastName);
                Assert.Equal("Jon Doe", bloodSugarTestResults[0].User!.Name);
            }
        }

        [Fact]
        public async Task Is_Create_WorkProperlyWithUserData()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                User user = new()
                {
                    FirstName = "Jon",
                    LastName = "Doe"
                };
                bloodSugarSetupContext.Users!.Add(user);
                bloodSugarSetupContext.SaveChanges();
            }

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
                    Result = 100.6,
                    UserId = 1
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
            }
        }

        [Fact]
        public async Task Is_Create_WorkProperlyWithInvalidUserData()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                User user = new()
                {
                    FirstName = "Jon",
                    LastName = "Doe"
                };
                bloodSugarSetupContext.Users!.Add(user);
                bloodSugarSetupContext.SaveChanges();
            }

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
                    Result = 100.6,
                    UserId = 2
                };

                NotFoundResult? notFoundResult =
                    await bloodSugarController.Create(bloodSugarTestResult) as NotFoundResult;

                Assert.NotNull(notFoundResult);
            }
        }

        [Fact]
        public async Task Is_Edit_WorkProperlyWithUserData()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                User[] users = new[]
                {
                    new User() { FirstName = "Jon", LastName = "Doe" },
                    new User() { FirstName = "Jon1", LastName = "Doe1" },
                };
                bloodSugarSetupContext.Users!.AddRange(users);
                bloodSugarSetupContext.SaveChanges();
            }

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                BloodSugarTestResult bloodSugarTestResult = new()
                {
                    MealTime = DateTime.UtcNow.AddDays(10).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 100.6,
                    User = bloodSugarSetupContext.Users!.First(u => u.Id == 1)
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
                    Result = 205.4,
                    UserId = 2
                };

                await bloodSugarController.Edit(1, bloodSugarTestResultUpdated);
            }

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                BloodSugarTestResult? bloodSugarTestResult = bloodSugarTestContext.BloodSugarTestResults!
                    .Include(b => b.User)
                    .SingleOrDefault(b => b.MealTime == DateTime.UtcNow.AddDays(15).Date);

                Assert.NotNull(bloodSugarTestResult);
                Assert.Equal(DateTime.UtcNow.AddDays(15).Date, bloodSugarTestResult!.MealTime);
                Assert.Equal(DateTime.UtcNow.Date, bloodSugarTestResult.TestTime);
                Assert.Equal(205.4, bloodSugarTestResult.Result);
                Assert.Equal("Jon1", bloodSugarTestResult.User!.FirstName);
                Assert.Equal("Doe1", bloodSugarTestResult.User!.LastName);
                Assert.Equal("Jon1 Doe1", bloodSugarTestResult.User!.Name);
            }
        }

        [Fact]
        public async Task Is_Edit_DontUpdateIfDataNotFound()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                Mock<IOptionsMonitor<BloodSugarOptions>> mock = new();
                mock.Setup(s => s.CurrentValue)
                    .Returns(new BloodSugarOptions() { FastingNormal = 100, TwoHoursNormal = 140 });
                BloodSugarController bloodSugarController = new(bloodSugarTestContext, mock.Object);

                BloodSugarTestResult bloodSugarTestResultUpdated = new()
                {
                    Id = 1,
                    MealTime = DateTime.UtcNow.AddDays(15).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 205.4
                };

                NotFoundResult? notFoundResult =
                    await bloodSugarController.Edit(1, bloodSugarTestResultUpdated) as NotFoundResult;

                Assert.NotNull(notFoundResult);
            }
        }

        [Fact]
        public async Task Is_Edit_DontUpdateIfUserDataNotFound()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                User[] users = new[]
                {
                    new User() { FirstName = "Jon", LastName = "Doe" },
                    new User() { FirstName = "Jon1", LastName = "Doe1" },
                };
                bloodSugarSetupContext.Users!.AddRange(users);
                bloodSugarSetupContext.SaveChanges();
            }

            using (BloodSugarContext bloodSugarSetupContext = new(options))
            {
                BloodSugarTestResult bloodSugarTestResult = new()
                {
                    MealTime = DateTime.UtcNow.AddDays(10).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 100.6,
                    User = bloodSugarSetupContext.Users!.First(u => u.Id == 1)
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
                    Result = 205.4,
                    UserId = 5
                };

                NotFoundResult? notFoundResult =
                    await bloodSugarController.Edit(1, bloodSugarTestResultUpdated) as NotFoundResult;

                Assert.NotNull(notFoundResult);
            }
        }

        [Fact]
        public async Task Is_Edit_DontUpdateInvalidModel()
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
                    Id = 2,
                    MealTime = DateTime.UtcNow.AddDays(15).Date,
                    TestTime = DateTime.UtcNow.Date,
                    Result = 205.4
                };

                await bloodSugarController.Edit(1, bloodSugarTestResultUpdated);
            }

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                BloodSugarTestResult? bloodSugarTestResult = bloodSugarTestContext.BloodSugarTestResults!
                    .SingleOrDefault(b => b.MealTime == DateTime.UtcNow.AddDays(10).Date);

                Assert.NotNull(bloodSugarTestResult);
                Assert.Equal(DateTime.UtcNow.AddDays(10).Date, bloodSugarTestResult!.MealTime);
                Assert.Equal(DateTime.UtcNow.Date, bloodSugarTestResult.TestTime);
                Assert.Equal(100.6, bloodSugarTestResult.Result);
            }
        }

        [Fact]
        public async Task Is_Delete_WorkProperly()
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
            }
        }

        [Fact]
        public async Task Is_Delete_DontUpdateIfDataNotFound()
        {
            var options = new DbContextOptionsBuilder<BloodSugarContext>()
                .UseInMemoryDatabase("BloodSugarTestDb")
                .Options;

            using (BloodSugarContext bloodSugarTestContext = new(options))
            {
                Mock<IOptionsMonitor<BloodSugarOptions>> mock = new();
                mock.Setup(s => s.CurrentValue)
                    .Returns(new BloodSugarOptions() { FastingNormal = 100, TwoHoursNormal = 140 });
                BloodSugarController bloodSugarController = new(bloodSugarTestContext, mock.Object);

                NotFoundResult? notFoundResult =
                    await bloodSugarController.DeleteConfirmed(1) as NotFoundResult;

                Assert.NotNull(notFoundResult);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    var options = new DbContextOptionsBuilder<BloodSugarContext>()
                        .UseInMemoryDatabase("BloodSugarTestDb")
                        .Options;
                    using (BloodSugarContext bloodSugarTestContext = new(options))
                    {
                        bloodSugarTestContext.Database.EnsureDeleted();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BloodSugarControllerTest()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
