using Article.MVC.ApplicationService.Contracts;
using Article.MVC.ApplicationService.Dtos.ArticlePostsDtos;
using Article.MVC.ApplicationService.Dtos.UserDtos;
using Article.MVC.ApplicationService.Dtos.UserDtos.Article.MVC.ApplicationService.Dtos.UserDtos;
using Article.MVC.Frameworks;
using Article.MVC.Frameworks.ResponseFrameworks;
using Article.MVC.Frameworks.ResponseFrameworks.Contracts;
using Article.MVC.Models.DomainModels;
using Article.MVC.Models.Service;
using Article.MVC.Models.Service.Contrcats;
using System.Net;

namespace Article.MVC.ApplicationService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        #region [- Ctor() -]
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region [- Post() -]
        public async Task<IResponse<PostUserServiceDto>> Post(PostUserServiceDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            var result = await _userRepository.Insert(user);

            if (!result.IsSuccessful)
                return new Response<PostUserServiceDto>(false, result.Status, result.Message, null);

            return new Response<PostUserServiceDto>(true, result.Status, result.Message, new PostUserServiceDto
            {
                Id = result.Value.Id,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                Email = result.Value.Email,
                Password = result.Value.PasswordHash
                
            });
        }
        #endregion

        #region [- Get() -]
        public async Task<IResponse<GetUserServiceDto>> Get(GetUserServiceDto dto)
        {
            var users = new User()
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };
            var selectResponse = await _userRepository.Select(users);

            if (selectResponse is null)
            {
                return new Response<GetUserServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }

            if (!selectResponse.IsSuccessful)
            {
                return new Response<GetUserServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, null);
            }
            var getUsersServiceDto = new GetUserServiceDto()
            {
                Id = selectResponse.Value.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            var response = new Response<GetUserServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, getUsersServiceDto);
            return response;
        }
        #endregion

        #region [- Put() -]
        public async Task<IResponse<PutUserServiceDto>> Put(PutUserServiceDto dto)
        {
            var user = new User
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            var result = await _userRepository.Update(user);

            if (!result.IsSuccessful)
                return new Response<PutUserServiceDto>(false, result.Status, result.Message, null);

            return new Response<PutUserServiceDto>(true, result.Status, result.Message, new PutUserServiceDto
            {
                Id = result.Value.Id,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                Email = result.Value.Email
            });
        }
        #endregion

        #region [- GetAll() -]
        public async Task<IResponse<GetAllUserServiceDto>> GetAll()
        {
            var selectAllResponse = await _userRepository.SelectAll();

            if (selectAllResponse is null)
            {
                return new Response<GetAllUserServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }

            if (!selectAllResponse.IsSuccessful)
            {
                return new Response<GetAllUserServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, null);
            }

            var getUsersDto = new GetAllUserServiceDto() { GetUserServiceDto = new List<GetUserServiceDto>() };

            foreach (var item in selectAllResponse.Value)
            {
                var usersDto = new GetUserServiceDto()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email
                };
                getUsersDto.GetUserServiceDto.Add(usersDto);
            }

            var response = new Response<GetAllUserServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, getUsersDto);
            return response;
        }
        #endregion

        #region [- GetByEmail() -]
        public async Task<IResponse<GetUserServiceDto>> GetByEmail(GetByEmailServiceDto dto)
        {
            var userEntity = await _userRepository.GetByEmailAsync(dto.Email);

            if (userEntity == null)
            {
                return new Response<GetUserServiceDto>(false, HttpStatusCode.NotFound, "User not found", null);
            }

            var result = new GetUserServiceDto
            {
                Id = userEntity.Value.Id,
                FirstName = userEntity.Value.FirstName,
                LastName = userEntity.Value.LastName,
                Email = userEntity.Value.Email
            };

            return new Response<GetUserServiceDto>(true, HttpStatusCode.OK, "User fetched successfully", result);
        }


        #endregion

        #region [- Delete() -]
        public async Task<IResponse<DeleteUserServiceDto>> Delete(DeleteUserServiceDto dto)
        {
            if (dto == null)
            {
                return new Response<DeleteUserServiceDto>(false, HttpStatusCode.UnprocessableContent, "Input DTO cannot be null.", null);
            }

            if (dto.Id == Guid.Empty)
            {
                return new Response<DeleteUserServiceDto>(false, HttpStatusCode.BadRequest, "Invalid user ID.", dto);
            }
            var deleteResponse = await _userRepository.Delete(dto.Id);

            if (!deleteResponse.IsSuccessful)
            {
                return new Response<DeleteUserServiceDto>(false, HttpStatusCode.UnprocessableContent, "Failed to delete the article.", dto);
            }
            return new Response<DeleteUserServiceDto>(true, HttpStatusCode.OK, "Article deleted successfully.", dto);
        } 
        #endregion

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password)); // Replace with real hashing
        }

    }
}
