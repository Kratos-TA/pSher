namespace PSher.Api.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using Common.Constants;
    using Microsoft.AspNet.Identity;
    using Services.Data.Contracts;
    
    [RoutePrefix("api/comments")]
    public class CommentsController : ApiController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [Authorize]
        public async Task<IHttpActionResult> Post(int authorId, string text, int idImage)
        {
            int commentId =  await this.commentsService.AddComment(authorId, text, idImage);

            if (commentId == GlobalConstants.ItemNotFoundReturnValue)
            {
                return this.BadRequest();
            }

            return this.Ok(commentId);
        }

        [Authorize]
        public async Task<IHttpActionResult> Put(int commentId, bool isLiked)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var imageAuthorId = await this.commentsService.GetMarkAuthorIdById(commentId);

            var isCurrenUserComment = currentUserId == imageAuthorId;

            if (!isCurrenUserComment)
            {
                return this.Unauthorized();
            }

            var isDone =  await this.commentsService.AddLikeDislike(commentId, isLiked);

            if (isDone)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }

        [Authorize]
        public async Task<IHttpActionResult> Delete(int commentId)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var imageAuthorId = await this.commentsService.GetMarkAuthorIdById(commentId);

            var isCurrenUserComment = currentUserId == imageAuthorId;

            if (!isCurrenUserComment)
            {
                return this.Unauthorized();
            }

            var result = await this.commentsService.DeleteComment(commentId);

            return this.Ok(result);
        }
    }
}
