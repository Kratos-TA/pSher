namespace PSher.Api.DataTransferModels.Album
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Infrastructure.Mapping;
    using PSher.Api.DataTransferModels.Images;
    using PSher.Models;

    public class AlbumDetailsResponseModel : IMapFrom<Album>, IHaveCustomMappings
    {
        private ICollection<ImageSimpleResponseModel> images;

        public AlbumDetailsResponseModel()
        {
            this.images = new HashSet<ImageSimpleResponseModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<string> Tags { get; set; }

        public string CreatorId { get; set; }

        public string CreatorName { get; set; }

        public bool IsPrivate { get; set; }

        public virtual ICollection<ImageSimpleResponseModel> Images
        {
            get { return this.images; }
            set { this.images = value; }
        } 

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Album, AlbumDetailsResponseModel>()
                .ForMember(a => a.Tags, opts => opts.MapFrom(a => a.Tags.Select(t => t.Name).ToList()))
                .ForMember(a => a.Images, opts => opts.MapFrom(a => a.Images.OrderBy(i => i.UploadedOn)))
                .ForMember(a => a.CreatorId, opts => opts.MapFrom(a => a.Creator.Id))
                .ForMember(a => a.CreatorName, opts => opts.MapFrom(a => a.Creator.UserName));
        }
    }
}