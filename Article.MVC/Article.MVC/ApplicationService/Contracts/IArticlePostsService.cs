using Article.MVC.ApplicationService.Dtos.ArticlePostsDtos;

namespace Article.MVC.ApplicationService.Contracts
{
    public interface IArticlePostsService : IService< PostArticleServiceDto, GetArticleServiceDto, GetAllArticleServiceDto, PutArticleServiceDto , DeleteArticleServiceDto>
    {
    }
}
