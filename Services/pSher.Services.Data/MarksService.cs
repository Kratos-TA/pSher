namespace PSher.Services.Data
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;
    using Models;
    using PSher.Common.Constants;
    using PSher.Data.Contracts;
    using PSher.Services.Common.Contracts;

    public class MarksService : IMarksService
    {
        private IRepository<Mark> marks;
        private IRepository<User> users;
        private IRepository<Image> images;
        private INotificationService notifier;

        public MarksService(
            IRepository<Mark> marksRepo,
            IRepository<User> usersRepo,
             IRepository<Image> imagesRepo,
             INotificationService notifier)
        {
            this.marks = marksRepo;
            this.users = usersRepo;
            this.images = imagesRepo;
            this.notifier = notifier;
        }

        public async Task<int> Add(string authorId, int imageId, int value)
        {
            var assessingUser = this.users
                .All()
                .FirstOrDefault(u => u.Id == authorId);

            var imageToAttachMarkTo = this.images
                .All()
                .FirstOrDefault(i => i.Id == imageId);

            if (imageToAttachMarkTo == null || assessingUser == null)
            {
                return GlobalConstants.InvalidDbObjectReturnValue;
            }

            var markToAdd = new Mark()
            {
                GivenBy = assessingUser,
                IsDeleted = false,
                Value = value
            };

            imageToAttachMarkTo.Rating.Marks.Add(markToAdd);

            this.marks.Add(markToAdd);
            this.images.Update(imageToAttachMarkTo);

            await this.marks.SaveChangesAsync();
            await this.images.SaveChangesAsync();

            return markToAdd.Id;
        }

        public async Task<int> UpdateMarkValue(int id, int value, string autenticatedUserId)
        {
            var markToChange = this.marks
                .All()
                .FirstOrDefault(m => m.Id == id && m.IsDeleted == false && m.GivenBy.Id == autenticatedUserId);

            if (markToChange == null)
            {
                return GlobalConstants.InvalidDbObjectReturnValue;
            }

            markToChange.Value = value;
            this.marks.Update(markToChange);
            await this.marks.SaveChangesAsync();

            return markToChange.Id;
        }

        public async Task<int> DeleteMark(int id, string autenticatedUserId)
        {
            var markToChange = this.marks
                .All()
                .FirstOrDefault(m => m.Id == id && m.IsDeleted == false && m.GivenBy.Id == autenticatedUserId);

            if (markToChange == null)
            {
                return GlobalConstants.InvalidDbObjectReturnValue;
            }

            markToChange.IsDeleted = true;
            this.marks.Update(markToChange);
            await this.marks.SaveChangesAsync();

            return markToChange.Id;
        }

        public async Task<string> GetMarkAuthorIdById(int id)
        {
            var imageAuthor = await this.marks
           .All()
           .Where(i => (i.IsDeleted == false) && i.Id == id)
           .Select(i => i.GivenBy.Id)
           .FirstOrDefaultAsync();

            return imageAuthor;
        }
    }
}
