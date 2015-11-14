namespace PSher.Api.Controllers
{
    using System.Web.Http;
    using System.Web.Http.Cors;

    using Common.Constants;
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
        [EnableCors("*", "*", "*")]
        public IHttpActionResult Post(int authorId, string text, int idImage)
        {
            int commentId = this.commentsService.AddComment(authorId, text, idImage);

            if (commentId == GlobalConstants.ErrorCodeComment)
            {
                return this.BadRequest();
            }

            return this.Ok("Comment with id " + commentId + " added.");
        }

        [Authorize]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult Put(int commentId, bool isLiked)
        {
            bool isDone = this.commentsService.AddLikeDislike(commentId, isLiked);

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
        public IHttpActionResult Delete(int id)
        {
            this.commentsService.DeleteComment(id);

            return this.Ok();
        }
    }
}
