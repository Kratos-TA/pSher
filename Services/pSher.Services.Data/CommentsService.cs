namespace PSher.Services.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;
    using Models;
    using PSher.Common.Constants;
    using PSher.Data.Contracts;
    using PSher.Services.Common.Contracts;

    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> comments;
        private readonly IRepository<Image> images;
        private readonly IRepository<User> users;
        private INotificationService notifier;

        public CommentsService(
            IRepository<Comment> commentsRepo,
            IRepository<Image> imagesRepo,
            IRepository<User> usersRepo,
            INotificationService notifier)
        {
            this.comments = commentsRepo;
            this.images = imagesRepo;
            this.users = usersRepo;
            this.notifier = notifier;
        }

        public async Task<int> AddComment(int authorId, string text, int idImage)
        {
            var imageToAddCommentTo = this.images.GetById(idImage);
            var author = this.users.GetById(authorId);

            if (imageToAddCommentTo == null || author == null)
            {
                return GlobalConstants.ItemNotFoundReturnValue;
            }

            var newComment = new Comment()
            {
                Author = author,
                Dislikes = 0,
                Likes = 0,
                PostedOn = DateTime.Now,
                Text = text
            };
            
            imageToAddCommentTo.Comments.Add(newComment);

            await this.images.SaveChangesAsync();

            return newComment.Id;
        }

        public async Task<bool> AddLikeDislike(int commentId, bool isLiked)
        {
            var commentToRate = this.comments.GetById(commentId);

            if (commentToRate == null)
            {
                return false;
            }

            if (isLiked)
            {
                ++commentToRate.Likes;
            }
            else
            {
                ++commentToRate.Dislikes;
            }

            await this.comments.SaveChangesAsync();

            return true;
        }

        public async Task<int> DeleteComment(int id)
        {
            var commentToDelete = this.comments.GetById(id);

            if (commentToDelete == null)
            {
                return GlobalConstants.ItemNotFoundReturnValue;
            }

            commentToDelete.IsDeleted = true;

            await this.comments.SaveChangesAsync();

            return commentToDelete.Id;
        }

        public async Task<string> GetMarkAuthorIdById(int id)
        {
            var imageAuthor = await this.comments
                .All()
                .Where(i => (i.IsDeleted == false) && i.Id == id)
                .Select(i => i.Author)
                .Where(u => u.IsDeleted == false)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            return imageAuthor;
        }
    }
}
