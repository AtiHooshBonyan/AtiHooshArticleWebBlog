namespace Article.MVC.ApplicationService.Dtos.ArticlePostsDtos
{
    public class PutArticleServiceDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string VideoUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
