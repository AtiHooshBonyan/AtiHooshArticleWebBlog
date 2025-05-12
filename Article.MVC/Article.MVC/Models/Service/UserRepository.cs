using Article.MVC.Frameworks;
using Article.MVC.Frameworks.ResponseFrameworks;
using Article.MVC.Frameworks.ResponseFrameworks.Contracts;
using Article.MVC.Models.DomainModels;
using Article.MVC.Models.Service.Contrcats;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Article.MVC.Models.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectDbContext _dbContext;

        #region [- Ctor -]
        public UserRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<User>> Delete(Guid id)
        {
            try
            {
                var DeleteRecord = await _dbContext.users.FindAsync(id);
                if (DeleteRecord == null)
                {
                    return new Response<User>(false, HttpStatusCode.NotFound, "User not found", null);

                }
                if (DeleteRecord is null)
                {
                    return new Response<User>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                _dbContext.users.Remove(DeleteRecord);
                await _dbContext.SaveChangesAsync();
                var response = new Response<User>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, DeleteRecord);
                return response;
            }
            catch (Exception)
            {
                return new Response<User>(false, HttpStatusCode.InternalServerError, "Message", null);
            }
        }
        #endregion

        #region [- GetByEmailAsync() -]
        public async Task<IResponse<User>> GetByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return new Response<User>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }

                var user = await _dbContext.users
                    .Where(u => u.Email == email)
                    .SingleOrDefaultAsync();

                return user is null
                    ? new Response<User>(false, HttpStatusCode.NotFound, "User not found", null)
                    : new Response<User>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, user);
            }
            catch (Exception)
            {
                return new Response<User>(false, HttpStatusCode.InternalServerError, "An error occurred while retrieving the user", null);
            }
        }
        #endregion

        #region [- Insert() -]
        public async Task<IResponse<User>> Insert(User model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<User>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                await _dbContext.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                var response = new Response<User>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Select() -]
        public async Task<IResponse<User>> Select(User model)
        {
            try
            {
                var responseValue = new User();
                if (model.Id.ToString() != "")
                {
                    responseValue = await _dbContext.users.Where(c => c.Id == model.Id).SingleOrDefaultAsync();
                }
                else
                {
                    responseValue = await _dbContext.users.FindAsync(model.Id);
                }
                return responseValue is null ?
                     new Response<User>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                     new Response<User>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, responseValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- SelectAll() -]
        public async Task<IResponse<IEnumerable<User>>> SelectAll()
        {
            try
            {
                var user = await _dbContext.users.AsNoTracking().ToListAsync();
                return user is null ?
                    new Response<IEnumerable<User>>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<IEnumerable<User>>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Update() -]
        public async Task<IResponse<User>> Update(User model)
        {
            try
            {
                if (model == null)
                {
                    return new Response<User>(false, HttpStatusCode.UnprocessableContent, "Input data is null", null);
                }

                var existinguser = await _dbContext.users.FindAsync(model.Id);
                if (existinguser == null)
                {
                    return new Response<User>(false, HttpStatusCode.NotFound, "User not found", null);
                }

                existinguser.FirstName = model.FirstName;
                existinguser.LastName = model.LastName;
                existinguser.Email = existinguser.Email;
                existinguser.PasswordHash = model.PasswordHash;

                await _dbContext.SaveChangesAsync();

                return new Response<User>(true, HttpStatusCode.OK, "User updated successfully", existinguser);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new Response<User>(false, HttpStatusCode.Conflict, "Concurrency conflict occurred", null);
            }
            catch (Exception)
            {
                return new Response<User>(false, HttpStatusCode.InternalServerError, "An error occurred while updating the article", null);
            }
        }

        #endregion
    }
}
