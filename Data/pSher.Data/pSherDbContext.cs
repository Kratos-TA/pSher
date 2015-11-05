namespace PSher.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using PSher.Data.Contracts;
    using PSher.Models;

    public class PSherDbContext : IdentityDbContext<User>, IPSherDbContext
    {
        public PSherDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Gallery> Galleries { get; set; }

        public virtual IDbSet<Album> Albums { get; set; }

        public virtual IDbSet<Image> Images { get; set; }

        public virtual IDbSet<ImageInfo> ImageInfos { get; set; }

        public virtual IDbSet<Discussion> Discussions { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Rating> Ratings { get; set; }

        public virtual IDbSet<Mark> Marks { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        public static PSherDbContext Create()
        {
            return new PSherDbContext();
        }
    }
}
