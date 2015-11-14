namespace PSher.Api.DataTransferModels.Users
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using PSher.Api.DataTransferModels.Images;
    using PSher.Api.Infrastructure.Mapping;
    using PSher.Models;

    public class UserResponseModel : IMapFrom<User>, IHaveCustomMappings
    {
        private ICollection<Image> images;

        public UserResponseModel()
        {
            this.images = new HashSet<Image>();
        }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<User, UserResponseModel>()
             .ForMember(urm => urm.Images, opts => opts.MapFrom(u => u.Photostream.Images.OrderBy(i => i.UploadedOn)));
        }
    }
}