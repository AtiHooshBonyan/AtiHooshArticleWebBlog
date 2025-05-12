using Article.MVC.Frameworks.ResponseFrameworks.Contracts;
using Article.MVC.Frameworks;
using Article.MVC.Models.Service.Contrcats;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System;
using Article.MVC.Models.DomainModels;
using Article.MVC.Frameworks.ResponseFrameworks;

namespace Article.MVC.Models.Service
{
    public class ArticlePostsRepository : IArticlePostsRepository
    {
        private readonly ProjectDbContext _dbContext;

        #region [- Ctor -]
        public ArticlePostsRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region [- Insert() -]
        public async Task<IResponse<ArticlePost>> Insert(ArticlePost model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<ArticlePost>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                await _dbContext.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                var response = new Response<ArticlePost>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- SelectAll() -]
        public async Task<IResponse<IEnumerable<ArticlePost>>> SelectAll()
        {
            try
            {
                var article = await _dbContext.articlePosts.AsNoTracking().ToListAsync();
                return article is null ?
                    new Response<IEnumerable<ArticlePost>>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<IEnumerable<ArticlePost>>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, article);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Select() -]
        public async Task<IResponse<ArticlePost>> Select(ArticlePost model)
        {
            try
            {
                var responseValue = new ArticlePost();
                if (model.Id.ToString() != "")
                {
                    responseValue = await _dbContext.articlePosts.Where(c => c.Title == model.Title).SingleOrDefaultAsync();
                }
                else
                {
                    responseValue = await _dbContext.articlePosts.FindAsync(model.Id);
                }
                return responseValue is null ?
                     new Response<ArticlePost>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                     new Response<ArticlePost>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, responseValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Update() -]
        public async Task<IResponse<ArticlePost>> Update(ArticlePost model)
        {
            try
            {
                if (model == null)
                {
                    return new Response<ArticlePost>(false, HttpStatusCode.UnprocessableContent, "Input data is null", null);
                }

                var existingArticle = await _dbContext.articlePosts.FindAsync(model.Id);
                if (existingArticle == null)
                {
                    return new Response<ArticlePost>(false, HttpStatusCode.NotFound, "Article not found", null);
                }

                existingArticle.Title = model.Title;
                existingArticle.Content = model.Content;
                existingArticle.PublishedAt = existingArticle.PublishedAt;
                existingArticle.ImagePath = model.ImagePath;
                existingArticle.VideoUrl = model.VideoUrl;
                existingArticle.UpdatedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                return new Response<ArticlePost>(true, HttpStatusCode.OK, "Article updated successfully", existingArticle);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new Response<ArticlePost>(false, HttpStatusCode.Conflict, "Concurrency conflict occurred", null);
            }
            catch (Exception)
            {
                return new Response<ArticlePost>(false, HttpStatusCode.InternalServerError, "An error occurred while updating the article", null);
            }
        }

        #endregion

        #region [- Delete() -]
        public async Task<IResponse<ArticlePost>> Delete(Guid id)
        {
            try
            {
                var DeleteRecord = await _dbContext.articlePosts.FindAsync(id);
                if (DeleteRecord == null)
                {
                    return new Response<ArticlePost>(false, HttpStatusCode.NotFound, "Article not found", null);

                }
                if (DeleteRecord is null)
                {
                    return new Response<ArticlePost>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                _dbContext.articlePosts.Remove(DeleteRecord);
                await _dbContext.SaveChangesAsync();
                var response = new Response<ArticlePost>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, DeleteRecord);
                return response;
            }
            catch (Exception)
            {
                return new Response<ArticlePost>(false, HttpStatusCode.InternalServerError, "Message", null);
            }
        }
        #endregion
    }
}

