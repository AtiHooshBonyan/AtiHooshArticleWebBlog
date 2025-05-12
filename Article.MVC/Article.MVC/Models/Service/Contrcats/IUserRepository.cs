using Article.MVC.Frameworks.ResponseFrameworks.Contracts;
using Article.MVC.Models.DomainModels;
using System.Collections.ObjectModel;

namespace Article.MVC.Models.Service.Contrcats
{
    public interface IUserRepository : IRepository<User, IEnumerable<User>>
    {
        Task<IResponse<User>> GetByEmailAsync(string email);
    }

}
