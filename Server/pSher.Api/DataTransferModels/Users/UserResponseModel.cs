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
        private ICollection<ImageSimpleResponseModel> images;

        public UserResponseModel()
        {
            this.images = new HashSet<ImageSimpleResponseModel>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<ImageSimpleResponseModel> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<User, UserResponseModel>()
             .ForMember(urm => urm.Images, opts => opts.MapFrom(u => u.Images.OrderBy(i => i.UploadedOn)));
        }
    }
}