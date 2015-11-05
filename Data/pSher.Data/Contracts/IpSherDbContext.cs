namespace PSher.Data.Contracts
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using PSher.Models;

    public interface IPSherDbContext
    {
        IDbSet<Gallery> Galleries { get; set; }

        IDbSet<Album> Albums { get; set; }

        IDbSet<Image> Images { get; set; }

        IDbSet<ImageInfo> ImageInfos { get; set; }

        IDbSet<Discussion> Discussions { get; set; }

        IDbSet<Comment> Comments { get; set; }

        IDbSet<Rating> Ratings { get; set; }

        IDbSet<Mark> Marks { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void Dispose();

        int SaveChanges();
    }
}
