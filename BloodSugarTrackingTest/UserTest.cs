using BloodSugarTracking.Models;
using Xunit;

namespace BloodSugarTrackingTest
{
    public class UserTest
    {
        [Fact]
        public void Can_Name_ReturnProperData()
        {
            User user = new()
            {
                FirstName = "Jon",
                LastName = "Doe"
            };
            Assert.Equal("Jon Doe", user.Name);
        }
    }
}
