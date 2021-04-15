namespace BloodSugarTracking.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public User()
        {
            Id = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public string Name
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
