namespace PSher.Services.Data
{
    using PSher.Data.Contracts;
    using Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;

    public class MarksService : IMarksService
    {
        private IRepository<Mark> marks;
        private IRepository<User> users;
        private IRepository<Image> images;

        public MarksService(IRepository<Mark> marksRepo, IRepository<User> usersRepo, IRepository<Image> imagesRepo)
        {
            this.marks = marksRepo;
            this.users = usersRepo;
            this.images = imagesRepo;
        }

        public async Task<int> Add(string authorUserName, int imageId, int value)
        {
            var assessingUser = this.users
                .All()
                .FirstOrDefault(u => u.UserName == authorUserName);

            var markToAdd = new Mark()
            {
                GivenBy = assessingUser,
                IsDeleted = false,
                Value = value
            };

            var imageToAttachMarkTo = this.images
                .All()
                .FirstOrDefault(i => i.Id == imageId);

            imageToAttachMarkTo.Marks.Add(markToAdd);

            this.marks.Add(markToAdd);
            this.images.Update(imageToAttachMarkTo);

            await this.marks.SaveChangesAsync();
            await this.images.SaveChangesAsync();

            return markToAdd.Id;
        }

        public async Task<int> UpdateMarkValue(int id, int value)
        {
            var markToChange = this.marks
                .All()
                .FirstOrDefault(m => m.Id == id);

            markToChange.Value = value;

            this.marks.Update(markToChange);

            await this.marks.SaveChangesAsync();

            return markToChange.Id;
        }

        public async Task<int> DeleteMark(int id)
        {
            var markToChange = this.marks
                .All()
                .FirstOrDefault(m => m.Id == id);

            markToChange.IsDeleted = true;

            this.marks.Update(markToChange);

            await this.marks.SaveChangesAsync();

            return markToChange.Id;
        }
    }
}
