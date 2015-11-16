namespace PSher.Api.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using Common.Constants;
    using DataTransferModels.Marks;
    using Microsoft.AspNet.Identity;
    using PSher.Services.Data.Contracts;

    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/marks")]
    public class MarksController : ApiController
    {
        private readonly IMarksService marksService;

        public MarksController(IMarksService marksService)
        {
            this.marksService = marksService;
        }

        [Authorize]
        public async Task<IHttpActionResult> Post(MarkRequestModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.BadRequest(string.Format(ErrorMessages.InvalidRequestModel, "MarkRequestModel"));
            }

            var autenticatedUserId = this.User.Identity.GetUserId();

            var markId = await this.marksService.Add(
                autenticatedUserId,
                model.ImageId,
                model.Value);

            return this.Ok(markId);
        }

        [Authorize]
        public async Task<IHttpActionResult> Put(int id, int value)
        {
            var autenticatedUserId = this.User.Identity.GetUserId();

            var markId = await this.marksService.UpdateMarkValue(id, value, autenticatedUserId);

            return this.Ok(markId);
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var autenticatedUserId = this.User.Identity.GetUserId();

            var markId = await this.marksService.DeleteMark(id, autenticatedUserId);

            return this.Ok(markId);
        }
    }
}
