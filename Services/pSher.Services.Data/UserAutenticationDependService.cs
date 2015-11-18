namespace PSher.Services.Data
{
    using System.Linq;

    using PSher.Data.Contracts;
    using PSher.Models;

    public class UserAutenticationDependService
    {
        protected readonly IRepository<User> Users;

        public UserAutenticationDependService(IRepository<User> userRepo)
        {
            this.Users = userRepo;
        }

        public User GetCurrentOrEmptyUserById(string userId)
        {
            var currentUser = this.Users
                .All()
                .FirstOrDefault(u => u.Id == userId) ??
                    new User()
                    {
                        UserName = string.Empty
                    };

            return currentUser;
        }
    }
}
