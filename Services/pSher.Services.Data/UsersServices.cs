﻿namespace PSher.Services.Data
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using PSher.Common.Constants;
    using PSher.Common.Extensions;
    using PSher.Data.Contracts;
    using PSher.Models;
    using PSher.Services.Data.Contracts;

    public class UsersServices : IUsersServices
    {
        private readonly IRepository<User> users;

        public UsersServices(IRepository<User> usersRepo)
        {
            this.users = usersRepo;
        }

        public IQueryable<User> GetByUserName(string username)
        {
            var resultUser = this.users
                .All()
                .Where(user => user.UserName == username && user.IsDeleted == false);

            return resultUser;
        }

        public async Task<string> Delete(string userId)
        {
            var userToDelete = this.users
                .GetById(userId);

            if (userToDelete == null)
            {
                return ErrorMessages.InvalidUser;
            }

            userToDelete.IsDeleted = true;
            userToDelete.Albums.ForEach(a => a.IsDeleted = true);
            userToDelete.Images.ForEach(i =>
            {
                i.IsDeleted = true;
                i.Tags.Clear();
            });

            this.users.Update(userToDelete);

            var result = await this.users.SaveChangesAsync();

            return string.Format(Messages.DeleteEntityWithNameConformation, "User", userToDelete.UserName);
        }

        public async Task<string> Update(string userId, string newEmail = null, string newFirstName = null, string newLastName = null)
        {
            var userToUpdate = await this.users
                .All()
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false);

            if (userToUpdate == null)
            {
                return ErrorMessages.InvalidUser;
            }

            if (!string.IsNullOrEmpty(newEmail))
            {
                userToUpdate.Email = newEmail;
            }

            if (!string.IsNullOrEmpty(newFirstName))
            {
                userToUpdate.FirstName = newFirstName;
            }

            if (!string.IsNullOrEmpty(newLastName))
            {
                userToUpdate.LastName = newLastName;
            }

            this.users.Update(userToUpdate);

            var result = await this.users.SaveChangesAsync();

            return string.Format(Messages.UpdateEntityWithNameConformation, "User", userToUpdate.UserName);
        }
    }
}
