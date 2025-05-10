using Article.MVC.ApplicationService.Contracts;
using Article.MVC.Frameworks.ResponseFrameworks.Contracts;
using Article.MVC.Frameworks;
using System.Net;
using Article.MVC.ApplicationService.Dtos.ArticlePostsDtos;
using Article.MVC.Frameworks.ResponseFrameworks;
using Article.MVC.Models.DomainModels;
using Article.MVC.Models.Service.Contrcats;

namespace Article.MVC.ApplicationService.Services
{
    public class ArticlePostsService : IArticlePostsService
    {
        private readonly IArticlePostsRepository _articlePostsRepository;

        #region [- ctor -]
        public ArticlePostsService(IArticlePostsRepository articlePostsRepository)
        {
            _articlePostsRepository = articlePostsRepository;
        }
        #endregion

        #region [- GetAll() -]
        public async Task<IResponse<GetAllArticleServiceDto>> GetAll()
        {
            var selectAllResponse = await _articlePostsRepository.SelectAll();

            if (selectAllResponse is null)
            {
                return new Response<GetAllArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }

            if (!selectAllResponse.IsSuccessful)
            {
                return new Response<GetAllArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, null);
            }

            var getAllArticlePostsDto = new GetAllArticleServiceDto() { GetArticleServiceDto = new List<GetArticleServiceDto>() };

            foreach (var item in selectAllResponse.Value)
            {
                var articlePostsDto = new GetArticleServiceDto()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Content = item.Content,
                    PublishedAt = item.PublishedAt,
                    ImagePath = item.ImagePath,
                    VideoUrl = item.VideoUrl,
                    UpdatedAt = item.UpdatedAt
                };
                getAllArticlePostsDto.GetArticleServiceDto.Add(articlePostsDto);
            }

            var response = new Response<GetAllArticleServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, getAllArticlePostsDto);
            return response;
        }
        #endregion

        #region [- Get() -]
        public async Task<IResponse<GetArticleServiceDto>> Get(GetArticleServiceDto dto)
        {
            var articlPosts = new ArticlePost()
            {
                Id = dto.Id,
                Title = dto.Title,
                Content = dto.Content,
                PublishedAt = dto.PublishedAt,
                ImagePath = dto.ImagePath,
                VideoUrl = dto.VideoUrl,
                UpdatedAt = dto.UpdatedAt
            };
            var selectResponse = await _articlePostsRepository.Select(articlPosts);

            if (selectResponse is null)
            {
                return new Response<GetArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }

            if (!selectResponse.IsSuccessful)
            {
                return new Response<GetArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, null);
            }
            var getArticlePostsServiceDto = new GetArticleServiceDto()
            {
                Id = selectResponse.Value.Id,
                Title = dto.Title,
                Content = dto.Content,
                PublishedAt = dto.PublishedAt,
                ImagePath = dto.ImagePath,
                VideoUrl = dto.VideoUrl,
                UpdatedAt = dto.UpdatedAt
            };

            var response = new Response<GetArticleServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, getArticlePostsServiceDto);
            return response;
        }
        #endregion

        #region [- Post() -]
        public async Task<IResponse<PostArticleServiceDto>> Post(PostArticleServiceDto dto)
        {
            if (dto is null)
            {
                return new Response<PostArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }
            var postArticle = new ArticlePost()
            {
                Id = new Guid(),
                Title = dto.Title,
                Content = dto.Content,
                PublishedAt = dto.PublishedAt,
                ImagePath = dto.ImagePath,
                VideoUrl = dto.VideoUrl,
                UpdatedAt = dto.UpdatedAt
            };
            var insertResponse = await _articlePostsRepository.Insert(postArticle);

            if (!insertResponse.IsSuccessful)
            {
                return new Response<PostArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, dto);
            }

            var response = new Response<PostArticleServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, dto);
            return response;
        }
        #endregion

        #region [- Put() -]
        public async Task<IResponse<PutArticleServiceDto>> Put(PutArticleServiceDto dto)
        {
            if (dto is null)
            {
                return new Response<PutArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }
            var putArticle = new ArticlePost()
            {
                Id = dto.Id,
                Title = dto.Title,
                Content = dto.Content,
                PublishedAt = dto.PublishedAt,
                ImagePath = dto.ImagePath,
                VideoUrl = dto.VideoUrl,
                UpdatedAt = DateTime.UtcNow
            };
            var updateResponse = await _articlePostsRepository.Update(putArticle);

            if (!updateResponse.IsSuccessful)
            {
                return new Response<PutArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, dto);
            }

            var response = new Response<PutArticleServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, dto);
            return response;
        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<DeleteArticleServiceDto>> Delete(DeleteArticleServiceDto dto)
        {
            if (dto == null)
            {
                return new Response<DeleteArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, "Input DTO cannot be null.", null);
            }

            if (dto.Id == Guid.Empty)
            {
                return new Response<DeleteArticleServiceDto>(false, HttpStatusCode.BadRequest, "Invalid article ID.", dto);
            }
            var deleteResponse = await _articlePostsRepository.Delete(dto.Id);

            if (!deleteResponse.IsSuccessful)
            {
                return new Response<DeleteArticleServiceDto>(false, HttpStatusCode.UnprocessableContent, "Failed to delete the article.", dto);
            }
            return new Response<DeleteArticleServiceDto>(true, HttpStatusCode.OK, "Article deleted successfully.", dto);
        }
 
        #endregion

    }
}
