namespace PSher.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IMarksService
    {
        Task<int> Add(string authorId, int imageId, int value);

        Task<int> UpdateMarkValue(int id, int value, string autenticatedUserId);

        Task<int> DeleteMark(int id, string autenticatedUserId);

        Task<string> GetMarkAuthorIdById(int id);
    }
}
