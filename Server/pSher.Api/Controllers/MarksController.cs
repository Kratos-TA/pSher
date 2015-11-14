namespace PSher.Api.Controllers
{
    using System.Web.Http;

    using Common.Constants;
    using DataTransferModels.Marks;
    using PSher.Services.Data.Contracts;

    [RoutePrefix("api/marks")]
    public class MarksController : ApiController
    {
        private readonly IMarksService marksService;

        public MarksController(IMarksService marksService)
        {
            this.marksService = marksService;
        }

        public IHttpActionResult Post(MarkRequestModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return this.BadRequest(string.Format(ErrorMessages.InvalidRequestModel, "MarkRequestModel"));
            }

            var markId = this.marksService.Add(
                model.AuthorUserName,
                model.ImageId,
                model.Value);

            return this.Ok(markId);
        }

        public IHttpActionResult Put(int id, int value)
        {
            var markId = this.marksService.UpdateMarkValue(id, value);

            return this.Ok(markId);
        }

        public IHttpActionResult Delete(int id)
        {
            var markId = this.marksService.DeleteMark(id);

            return this.Ok(markId);
        }
    }
}
