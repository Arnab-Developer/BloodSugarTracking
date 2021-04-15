using System.Collections.Generic;

namespace BloodSugarTracking.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<BloodSugarTestResult>? BloodSugarTestResults { get; set; }

        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public string Name
        {
            get
            {
                string name = string.Empty;

                if (!string.IsNullOrEmpty(FirstName) &&
                    !string.IsNullOrEmpty(LastName))
                {
                    name = $"{FirstName} {LastName}";
                }
                else if (string.IsNullOrEmpty(FirstName) &&
                    !string.IsNullOrEmpty(LastName))
                {
                    name = LastName;
                }
                else if (!string.IsNullOrEmpty(FirstName) &&
                    string.IsNullOrEmpty(LastName))
                {
                    name = FirstName;
                }

                return name;
            }
        }
    }
}
