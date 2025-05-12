using Article.MVC.ApplicationService.Dtos.UserDtos;
using Article.MVC.ApplicationService.Dtos.UserDtos.Article.MVC.ApplicationService.Dtos.UserDtos;
using Article.MVC.Frameworks.ResponseFrameworks.Contracts;

namespace Article.MVC.ApplicationService.Contracts
{
    public interface IUserService : IService<PostUserServiceDto, GetUserServiceDto,GetAllUserServiceDto, PutUserServiceDto, DeleteUserServiceDto>
    {
        Task<IResponse<GetUserServiceDto>> GetByEmail(GetByEmailServiceDto dto);

    }
}
