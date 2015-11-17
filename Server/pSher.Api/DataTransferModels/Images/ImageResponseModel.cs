namespace PSher.Api.DataTransferModels.Images
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using PSher.Api.Infrastructure.Mapping;
    using PSher.Models;

    public class ImageResponseModel : IMapFrom<Image>, IHaveCustomMappings
    {
        private ICollection<string> tags;

        public ImageResponseModel()
        {
            this.tags = new HashSet<string>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string DropboxUrl { get; set; }

        public DateTime UploadedOn { get; set; }

        public ImageRatingResponseModel Rating { get; set; }

        public virtual ICollection<string> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Image, ImageResponseModel>()
                .ForMember(i => i.AuthorId, opts => opts.MapFrom(i => i.Author.Id))
                .ForMember(i => i.AuthorName, opts => opts.MapFrom(i => i.Author.UserName))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(so => so.Tags.Select(t => t.Name).ToList()));
        }
    }
}