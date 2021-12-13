namespace BloodSugarTrackingTest;

public class UserControllerTest : IDisposable
{
    private bool _disposedValue;

    [Fact]
    public void Is_Index_WorkProperly()
    {
        var options = new DbContextOptionsBuilder<BloodSugarContext>()
            .UseInMemoryDatabase("UserTestDb")
            .Options;

        Mock<ITenantProvider> iTenantProviderMock = new();
        iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

        using (BloodSugarContext bloodSugarSetupContext = new(options, iTenantProviderMock.Object))
        {
            User user = new()
            {
                FirstName = "Jon",
                LastName = "Doe"
            };
            bloodSugarSetupContext.Users!.Add(user);
            bloodSugarSetupContext.SaveChanges();
        }

        using (BloodSugarContext bloodSugarTestContext = new(options, iTenantProviderMock.Object))
        {
            UserController userController = new(bloodSugarTestContext, iTenantProviderMock.Object);

            ViewResult? viewResult = userController.Index() as ViewResult;

            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult!.Model);

            IList<User>? users = viewResult.Model as IList<User>;

            Assert.NotNull(users);
            Assert.Equal("Jon", users![0].FirstName);
            Assert.Equal("Doe", users[0].LastName);
            Assert.Equal("Jon Doe", users[0].Name);
            Assert.Equal("1", users[0].TenantId);
        }
    }

    [Fact]
    public async Task Is_Create_WorkProperly()
    {
        var options = new DbContextOptionsBuilder<BloodSugarContext>()
            .UseInMemoryDatabase("UserTestDb")
            .Options;

        Mock<ITenantProvider> iTenantProviderMock = new();
        iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

        using (BloodSugarContext bloodSugarSetupContext = new(options, iTenantProviderMock.Object))
        {
            UserController userController = new(bloodSugarSetupContext, iTenantProviderMock.Object);
            User user = new()
            {
                FirstName = "Jon",
                LastName = "Doe"
            };
            await userController.Create(user);
        }

        using (BloodSugarContext bloodSugarTestContext = new(options, iTenantProviderMock.Object))
        {
            User? user = bloodSugarTestContext.Users!
                .SingleOrDefault(u => u.FirstName == "Jon");

            Assert.NotNull(user);
            Assert.Equal("Jon", user!.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("Jon Doe", user.Name);
            Assert.Equal("1", user.TenantId);
        }
    }

    [Fact]
    public async Task Is_Edit_WorkProperly()
    {
        var options = new DbContextOptionsBuilder<BloodSugarContext>()
            .UseInMemoryDatabase("UserTestDb")
            .Options;

        Mock<ITenantProvider> iTenantProviderMock = new();
        iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

        using (BloodSugarContext bloodSugarSetupContext = new(options, iTenantProviderMock.Object))
        {
            User user = new()
            {
                FirstName = "Jon",
                LastName = "Doe"
            };
            bloodSugarSetupContext.Users!.Add(user);
            bloodSugarSetupContext.SaveChanges();
        }

        using (BloodSugarContext bloodSugarSetupContext1 = new(options, iTenantProviderMock.Object))
        {
            UserController userController = new(bloodSugarSetupContext1, iTenantProviderMock.Object);
            User user = new()
            {
                Id = 1,
                FirstName = "Jon1",
                LastName = "Doe1"
            };
            await userController.Edit(1, user);
        }

        using (BloodSugarContext bloodSugarTestContext = new(options, iTenantProviderMock.Object))
        {
            User? user = bloodSugarTestContext.Users!
                .SingleOrDefault(u => u.FirstName == "Jon1");

            Assert.NotNull(user);
            Assert.Equal("Jon1", user!.FirstName);
            Assert.Equal("Doe1", user.LastName);
            Assert.Equal("Jon1 Doe1", user.Name);
            Assert.Equal("1", user.TenantId);
        }
    }

    [Fact]
    public async Task Is_Edit_DontUpdateIfDataNotFound()
    {
        var options = new DbContextOptionsBuilder<BloodSugarContext>()
            .UseInMemoryDatabase("UserTestDb")
            .Options;

        Mock<ITenantProvider> iTenantProviderMock = new();
        iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

        using BloodSugarContext bloodSugarTestContext = new(options, iTenantProviderMock.Object);
        UserController userController = new(bloodSugarTestContext, iTenantProviderMock.Object);
        User userUpdated = new()
        {
            Id = 1,
            FirstName = "Jon1",
            LastName = "Doe1"
        };
        NotFoundResult? notFoundResult =
            await userController.Edit(1, userUpdated) as NotFoundResult;
        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async Task Is_Edit_DontUpdateInvalidModel()
    {
        var options = new DbContextOptionsBuilder<BloodSugarContext>()
            .UseInMemoryDatabase("UserTestDb")
            .Options;

        Mock<ITenantProvider> iTenantProviderMock = new();
        iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

        using (BloodSugarContext bloodSugarSetupContext = new(options, iTenantProviderMock.Object))
        {
            User user = new()
            {
                FirstName = "Jon",
                LastName = "Doe"
            };
            bloodSugarSetupContext.Users!.Add(user);
            bloodSugarSetupContext.SaveChanges();
        }

        using (BloodSugarContext bloodSugarSetupContext1 = new(options, iTenantProviderMock.Object))
        {
            UserController userController = new(bloodSugarSetupContext1, iTenantProviderMock.Object);
            User user = new()
            {
                Id = 2,
                FirstName = "Jon1",
                LastName = "Doe1"
            };
            await userController.Edit(1, user);
        }

        using (BloodSugarContext bloodSugarTestContext = new(options, iTenantProviderMock.Object))
        {
            User? user = bloodSugarTestContext.Users!
                .SingleOrDefault(u => u.FirstName == "Jon");

            Assert.NotNull(user);
            Assert.Equal("Jon", user!.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("Jon Doe", user.Name);
        }
    }

    [Fact]
    public async Task Is_Delete_WorkProperly()
    {
        var options = new DbContextOptionsBuilder<BloodSugarContext>()
            .UseInMemoryDatabase("UserTestDb")
            .Options;

        Mock<ITenantProvider> iTenantProviderMock = new();
        iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

        using (BloodSugarContext bloodSugarSetupContext = new(options, iTenantProviderMock.Object))
        {
            User user = new()
            {
                FirstName = "Jon1",
                LastName = "Doe1"
            };
            bloodSugarSetupContext.Users!.Add(user);
            bloodSugarSetupContext.SaveChanges();
        }

        using (BloodSugarContext bloodSugarSetupContext1 = new(options, iTenantProviderMock.Object))
        {
            UserController userController = new(bloodSugarSetupContext1, iTenantProviderMock.Object);
            await userController.DeleteConfirmed(1);
        }

        using BloodSugarContext bloodSugarTestContext = new(options, iTenantProviderMock.Object);
        Assert.Equal(0, bloodSugarTestContext.Users!.Count());
    }

    [Fact]
    public async Task Is_Delete_DontUpdateIfDataNotFound()
    {
        var options = new DbContextOptionsBuilder<BloodSugarContext>()
            .UseInMemoryDatabase("UserTestDb")
            .Options;

        Mock<ITenantProvider> iTenantProviderMock = new();
        iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

        using (BloodSugarContext bloodSugarTestContext = new(options, iTenantProviderMock.Object))
        {
            UserController userController = new(bloodSugarTestContext, iTenantProviderMock.Object);
            NotFoundResult? notFoundResult =
                await userController.DeleteConfirmed(1) as NotFoundResult;
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
                    .UseInMemoryDatabase("UserTestDb")
                    .Options;

                Mock<ITenantProvider> iTenantProviderMock = new();
                iTenantProviderMock.Setup(s => s.GetTenantId()).Returns(Task.FromResult("1"));

                using BloodSugarContext bloodSugarSetupContext = new(options, iTenantProviderMock.Object);
                bloodSugarSetupContext.Database.EnsureDeleted();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~UserControllerTest()
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
