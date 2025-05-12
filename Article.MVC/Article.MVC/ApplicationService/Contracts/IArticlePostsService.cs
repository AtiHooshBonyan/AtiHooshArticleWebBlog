using Article.MVC.ApplicationService.Dtos.ArticlePostsDtos;
using Article.MVC.Frameworks.ResponseFrameworks.Contracts;

namespace Article.MVC.ApplicationService.Contracts
{
    public interface IArticlePostsService : IService< PostArticleServiceDto, GetArticleServiceDto,
    GetAllArticleServiceDto, PutArticleServiceDto , DeleteArticleServiceDto>
    {
        
    }
}
