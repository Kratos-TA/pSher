namespace PSher.Api.Controllers
{
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http.Cors;
    using System.Web.Http;

    using Common.Constants;
    using DataTransferModels.Marks;
    using Microsoft.AspNet.Identity;
    using PSher.Api.Validation;
    using PSher.Services.Data.Contracts;
    
    [RoutePrefix("api/marks")]
    [EnableCors("*", "*", "*")]
    public class MarksController : ApiController
    {
        public const string EntityName = "Mark";

        private readonly IMarksService marksService;

        public MarksController(IMarksService marksService)
        {
            this.marksService = marksService;
        }

        [HttpPost]
        [Authorize]
        [ValidateModel]
        public async Task<IHttpActionResult> Post(MarkRequestModel model)
        {
            var autenticatedUserId = this.User.Identity.GetUserId();

            var markId = await this.marksService.Add(
                autenticatedUserId,
                model.ImageId,
                model.Value);

            return this.Ok(markId);
        }

        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> Put(int id, int value)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var imageAuthorId = await this.marksService.GetMarkAuthorIdById(id);

            var isCurrenUserMark = currentUserId == imageAuthorId;

            if (!isCurrenUserMark)
            {
                return this.Unauthorized();
            }

            var changesMade = await this.marksService.UpdateMarkValue(id, value);

            if (changesMade == GlobalConstants.ItemNotFoundReturnValue)
            {
                return this.NotFound();
            }


            return this.Ok(changesMade);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var imageAuthorId = await this.marksService.GetMarkAuthorIdById(id);

            var isCurrenUserImage = currentUserId == imageAuthorId;

            if (!isCurrenUserImage)
            {
                return this.Unauthorized(AuthenticationHeaderValue.
                   Parse(string.Format(ErrorMessages.UnoutorizedAccess, EntityName)));
            }

            var changesMade = await this.marksService.DeleteMark(id);

            if (changesMade == GlobalConstants.ItemNotFoundReturnValue)
            {
                return this.NotFound();
            }

            return this.Ok(changesMade);
        }
    }
}
