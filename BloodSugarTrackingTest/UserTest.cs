using BloodSugarTracking.Models;
using Xunit;

namespace BloodSugarTrackingTest
{
    public class UserTest
    {
        [Fact]
        public void Can_Name_ReturnProperData_IfFirstNameAndLastNameIsNotNull()
        {
            User user = new()
            {
                FirstName = "Jon",
                LastName = "Doe"
            };
            Assert.Equal("Jon Doe", user.Name);
        }

        [Fact]
        public void Can_Name_ReturnProperData_IfFirstNameIsNullAndLastNameIsNotNull()
        {
            User user = new()
            {
                LastName = "Doe"
            };
            Assert.Equal("Doe", user.Name);
        }

        [Fact]
        public void Can_Name_ReturnProperData_IfFirstNameIsNotNullAndLastNameIsNull()
        {
            User user = new()
            {
                FirstName = "Jon"
            };
            Assert.Equal("Jon", user.Name);
        }

        [Fact]
        public void Can_Name_ReturnProperData_IfFirstNameAndLastNameIsNull()
        {
            User user = new();
            Assert.Equal(string.Empty, user.Name);
        }
    }
}
