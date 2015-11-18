namespace PSher.Api.DataTransferModels.Images
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Common.Constants;
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

        [MaxLength(ValidationConstants.MaxImageDropBoxUrlLength)]
        public string Url { get; set; }

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
                .ForMember(i => i.Url, opt => opt.MapFrom(i => i.DropboxUrl))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(so => so.Tags.Select(t => t.Name).ToList()));
        }
    }
}