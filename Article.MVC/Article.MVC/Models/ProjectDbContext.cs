using Article.MVC.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Article.MVC.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }
        public DbSet<ArticlePost> articlePosts { get; set; }
    }
}
