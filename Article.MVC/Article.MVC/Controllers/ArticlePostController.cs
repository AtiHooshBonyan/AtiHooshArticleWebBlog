using Article.MVC.ApplicationService.Contracts;
using Article.MVC.ApplicationService.Dtos.ArticlePostsDtos;
using Article.MVC.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Article.MVC.Controllers
{
    [Route("api/articles")]
    public class ArticlePostController : Controller
    {
        private readonly IArticlePostsService _articlePostsService;

        #region [- Ctor() -]
        public ArticlePostController(IArticlePostsService articlePostsService)
        {
            _articlePostsService = articlePostsService;
        }
        #endregion

        #region [- GetAll() -]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticlePost>>> GetAll()
        {
            Guard_Blog();

            try
            {
                var getAllResponse = await _articlePostsService.GetAll();

                var articles = getAllResponse.Value.GetArticleServiceDto;

                if (articles == null || !articles.Any())
                {
                    return NoContent();
                }

                return Ok(articles); // ✅ Return the list directly
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving articles.");
            }

        }
        #endregion

        #region [- Get() -]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticlePost>> Get(GetArticleServiceDto dto)
        {
            Guard_Blog();

            try
            {
                var getResponse = await _articlePostsService.Get(dto);

                var response = getResponse.Value;
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);

            }
            catch (Exception)
            {
                // Log exception here
                return StatusCode(500, "An error occurred while retrieving the article.");
            }
        }
        #endregion

        #region [- Post() -]
        [HttpPost]
        public async Task<ActionResult<ArticlePost>> Post([FromBody] PostArticleServiceDto dto)
        {
            Guard_Blog();
            var postDto = new GetArticleServiceDto { Title = dto.Title };
            var getResponse = await _articlePostsService.Get(postDto);

            switch (ModelState.IsValid)
            {
                case true when getResponse.Value is null:
                    {
                        var postResponse = await _articlePostsService.Post(dto);
                        return postResponse.IsSuccessful ? Ok() : BadRequest();
                    }
                case true when getResponse.Value is not null:
                    return Conflict(dto);
                default:
                    return BadRequest();
            }
        }
        #endregion

        #region [- Upload() -]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] IFormFile file)
        {
            if (file.Length > 0)
            {
                var filePath = Path.Combine("uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Ok(new { url = $"/uploads/{file.FileName}" });
            }
            return BadRequest();
        }
        #endregion

        #region [- Put() -]

        [HttpPut("{id}")]
        public async Task<ActionResult<ArticlePost>> Put(Guid id, [FromBody] PutArticleServiceDto dto)
        {
            Guard_Blog(); 

            if (ModelState.IsValid)
            {
                
                dto.Id = id;  
                var putResponse = await _articlePostsService.Put(dto);

                if (putResponse.IsSuccessful)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(putResponse.Message);  
                }
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }

        #endregion

        #region [- Delete() -]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ArticlePost>> Delete(Guid id)
        {
            Guard_Blog(); 

            var deleteDto = new DeleteArticleServiceDto { Id = id };

            var deleteResponse = await _articlePostsService.Delete(deleteDto);

            if (deleteResponse.IsSuccessful)
            {
                return Ok(deleteResponse.Message); 
            }

            return BadRequest(deleteResponse.Message); 
        }

        #endregion

        #region [- BlogGuard() -]
        private ObjectResult Guard_Blog()
        {
            if (_articlePostsService is null)
            {
                return Problem($" {nameof(_articlePostsService)} is null.");
            }
            return null;
        }
        #endregion
    }

}

