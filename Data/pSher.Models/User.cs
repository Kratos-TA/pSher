﻿namespace PSher.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PSher.Common.Constants;

    public class User : IdentityUser
    {
        private ICollection<Notification> notifications;
        private ICollection<Image> images;
        private ICollection<Album> albums;

        public User()
            : base()
        {
            this.notifications = new HashSet<Notification>();
            this.images = new HashSet<Image>();
            this.albums = new HashSet<Album>();
        }

        public User(string username)
            : base(username)
        {
            this.notifications = new HashSet<Notification>();
            this.images = new HashSet<Image>();
            this.albums = new HashSet<Album>();
        }

        public bool IsDeleted { get; set; }

        [MaxLength(ValidationConstants.MaxUserRealName)]
        [MinLength(ValidationConstants.MinUserRealName)]
        public string FirstName { get; set; }

        [MaxLength(ValidationConstants.MaxUserRealName)]
        [MinLength(ValidationConstants.MinUserRealName)]
        public string LastName { get; set; }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
        }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<Album> Albums
        {
            get { return this.albums; }
            set { this.albums = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }
    }
}
