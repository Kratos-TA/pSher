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

        public User()
            : base()
        {
            this.notifications = new HashSet<Notification>();
        }

        public User(string username)
            : base(username)
        {
            this.notifications = new HashSet<Notification>();
        }

        [MaxLength(ValidationConstants.MaxUserRealName)]
        [MinLength(ValidationConstants.MinUserRealName)]
        public string FirstName { get; set; }

        [MaxLength(ValidationConstants.MaxUserRealName)]
        [MinLength(ValidationConstants.MinUserRealName)]
        public string LastName { get; set; }

        public Gallery Gallery { get; set; }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
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
