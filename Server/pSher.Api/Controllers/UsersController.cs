namespace PSher.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using PSher.Api.DataTransferModels.Users;
    using PSher.Common.Constants;
    using PSher.Services.Data.Contracts;

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUsersServices usersServices;

        public UsersController(IUsersServices usersServices)
        {
            this.usersServices = usersServices;
        }

        public async Task<IHttpActionResult> Get(string id)
        {
            var result = this.usersServices
                .GetById(id)
                .ProjectTo<UserResponseModel>()
                .ToList();

            return this.Ok(result);
        }

        [Authorize]
        [EnableCors("*", "*", "*")]
        public async Task<IHttpActionResult> Put(UpdateUserRequestModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.BadRequest(string.Format(ErrorMessages.InvalidRequestModel, "UpdateUserRequestModel"));
            }

            var authenticatedUser = this.User.Identity.GetUserId();

            var updateUserResult = await this.usersServices.Update(
                authenticatedUser,
                model.Email,
                model.FirstName,
                model.LastName);

            return this.Ok(updateUserResult);
        }

        public async Task<IHttpActionResult> Delete()
        {
            var authenticatedUser = this.User.Identity.GetUserId();

            var deletedAlbumId = await this.usersServices.Delete(authenticatedUser);

            return this.Ok(deletedAlbumId);
        }
    }
}