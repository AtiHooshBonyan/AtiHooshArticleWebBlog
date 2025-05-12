using Article.MVC.ApplicationService.Contracts;
using Article.MVC.ApplicationService.Dtos.UserDtos;
using Article.MVC.ApplicationService.Dtos.ArticlePostsDtos;
using Article.MVC.Frameworks.ResponseFrameworks.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Article.MVC.Models.DomainModels;

namespace Article.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IArticlePostsService _articlePostsService;

        #region [- Ctor() -]
        public UserController(IUserService userService, IArticlePostsService articlePostsService)
        {
            _userService = userService;
            _articlePostsService = articlePostsService;
        }
        #endregion

        #region [- GetAll() -]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            Guard_User();

            try
            {
                var getAllResponse = await _userService.GetAll();

                var users = getAllResponse.Value.GetUserServiceDto;

                if (users == null || !users.Any())
                {
                    return NoContent();
                }

                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }

        }
        #endregion

        #region [- Get() -]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(GetUserServiceDto dto)
        {
            Guard_User();

            try
            {
                var getResponse = await _userService.Get(dto);

                var response = getResponse.Value;
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }
        #endregion

        #region [- GetByEmail() -]
        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetByEmail(GetByEmailServiceDto dto)
        {
            Guard_User();

            try
            {
                var getResponse = await _userService.GetByEmail(dto);

                var response = getResponse.Value;
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }
        #endregion

        #region [- Post() -]
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] PostUserServiceDto dto)
        {
            Guard_User();
            var postDto = new GetUserServiceDto { Email = dto.Email };
            var getResponse = await _userService.Get(postDto);

            switch (ModelState.IsValid)
            {
                case true when getResponse.Value is null:
                    {
                        var postResponse = await _userService.Post(dto);
                        return postResponse.IsSuccessful ? Ok() : BadRequest();
                    }
                case true when getResponse.Value is not null:
                    return Conflict(dto);
                default:
                    return BadRequest();
            }
        }
        #endregion

        #region [- Put() -]

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(Guid id, [FromBody] PutUserServiceDto dto)
        {
            Guard_User();

            if (ModelState.IsValid)
            {

                dto.Id = id;
                var putResponse = await _userService.Put(dto);

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
        public async Task<ActionResult<User>> Delete(Guid id)
        {
            Guard_User();

            var deleteDto = new DeleteUserServiceDto { Id = id };

            var deleteResponse = await _userService.Delete(deleteDto);

            if (deleteResponse.IsSuccessful)
            {
                return Ok(deleteResponse.Message);
            }

            return BadRequest(deleteResponse.Message);
        }

        #endregion

        #region [- UserGuard() -]
        private ObjectResult Guard_User()
        {
            if (_userService is null)
            {
                return Problem($" {nameof(_userService)} is null.");
            }
            return null;
        } 
        #endregion

    }
}
