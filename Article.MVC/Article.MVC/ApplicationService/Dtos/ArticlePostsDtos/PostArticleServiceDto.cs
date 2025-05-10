namespace Article.MVC.ApplicationService.Dtos.ArticlePostsDtos
{
    public class PostArticleServiceDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string VideoUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
