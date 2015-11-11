namespace PSher.Services.Data.Contracts
{
    public interface ICommentsService
    {
        int AddComment(int authorId, string text, int idImage);

        bool AddLikeDislike(int commentId, bool isLiked);

        int DeleteComment(int id);
    }
}
