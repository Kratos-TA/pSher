namespace PSher.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task<int> AddComment(int authorId, string text, int idImage);

        Task<bool> AddLikeDislike(int commentId, bool isLiked);

        Task<int> DeleteComment(int id);

        Task<string> GetMarkAuthorIdById(int id);
    }
}
