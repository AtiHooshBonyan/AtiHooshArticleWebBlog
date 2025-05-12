using System.ComponentModel.DataAnnotations;

namespace Article.MVC.Models.DomainModels
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Store a hashed password

        // Navigation property for articles
        public ICollection<ArticlePost> Articles { get; set; } = new List<ArticlePost>();
    }

}
