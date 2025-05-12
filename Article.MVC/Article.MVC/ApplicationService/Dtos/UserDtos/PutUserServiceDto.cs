using System.ComponentModel.DataAnnotations;

namespace Article.MVC.ApplicationService.Dtos.UserDtos
{
    public class PutUserServiceDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }

}
