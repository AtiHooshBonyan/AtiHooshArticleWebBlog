using System.ComponentModel.DataAnnotations;

namespace Article.MVC.Models.DomainModels
{
    public class ArticlePost
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string VideoUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
