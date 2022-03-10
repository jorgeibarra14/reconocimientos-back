namespace ITGovApi.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar = "assets/images/avatars/brian-hughes.jpg";
        public string JobTitle { get; set; }
        public string JobLevel { get; set; }

    }
}
