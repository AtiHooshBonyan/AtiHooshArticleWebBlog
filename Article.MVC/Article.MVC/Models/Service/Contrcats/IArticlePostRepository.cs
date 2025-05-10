using Article.MVC.Models.DomainModels;
using System;

namespace Article.MVC.Models.Service.Contrcats
{
    public interface IArticlePostsRepository : IRepository<ArticlePost, IEnumerable<ArticlePost>>
    {
    }
}
