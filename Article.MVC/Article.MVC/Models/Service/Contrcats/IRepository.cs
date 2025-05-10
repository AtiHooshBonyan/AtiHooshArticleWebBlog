using Article.MVC.Frameworks.ResponseFrameworks.Contracts;

namespace Article.MVC.Models.Service.Contrcats
{
    public interface IRepository<T, TCollection>
    {
        Task<IResponse<TCollection>> SelectAll();
        Task<IResponse<T>> Select(T obj);
        Task<IResponse<T>> Insert(T obj);
        Task<IResponse<T>> Update(T obj);
        Task<IResponse<T>> Delete(Guid id);

    }
}
