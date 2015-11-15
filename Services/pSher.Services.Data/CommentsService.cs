namespace PSher.Services.Data
{
    using System;

    using PSher.Common.Constants;
    using Contracts;
    using Models;
    using PSher.Data.Contracts;

    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> comments;
        private readonly IRepository<Image> images;
        private readonly IRepository<User> users;

        public CommentsService(IRepository<Comment> commentsRepo, IRepository<Image> imagesRepo, IRepository<User> usersRepo)
        {
            this.comments = commentsRepo;
            this.images = imagesRepo;
            this.users = usersRepo;
        }

        public int AddComment(int authorId, string text, int idImage)
        {
            var imageToAddCommentTo = this.images.GetById(idImage);
            var author = this.users.GetById(authorId);

            if (imageToAddCommentTo == null || author == null)
            {
                return GlobalConstants.ErrorCodeComment;
            }

            var currentComment = new Comment()
            {
                Author = author,
                Dislikes = 0,
                Likes = 0,
                PostedOn = DateTime.Now,
                Text = text
            };

            this.comments.Add(currentComment);
            imageToAddCommentTo.Comments.Add(currentComment);

            this.comments.SaveChanges();
            this.images.SaveChanges();

            return currentComment.Id;
        }

        public bool AddLikeDislike(int commentId, bool isLiked)
        {
            var commentToRate = this.comments.GetById(commentId);

            if (commentToRate == null)
            {
                return false;
            }

            if (isLiked)
            {
                commentToRate.Likes++;
            }
            else
            {
                commentToRate.Dislikes++;
            }

            this.comments.Update(commentToRate);
            this.comments.SaveChanges();

            return true;
        }

        public int DeleteComment(int id)
        {
            var commentToDelete = this.comments.GetById(id);

            commentToDelete.IsDeleted = true;

            this.comments.Update(commentToDelete);
            this.comments.SaveChanges();

            return commentToDelete.Id;
        }
    }
}
