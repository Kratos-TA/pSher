namespace PSher.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using PSher.Models;

    public interface IUsersServices : IService
    {
        IQueryable<User> GetById(string userId);

        Task<string> Delete(string userId);

        Task<string> Update(string userId, string newEmail = null, string newFirstName = null, string newLastName = null);
    }
}