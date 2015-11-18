namespace PSher.Api.DataTransferModels.Images
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Common.Constants;
    using PSher.Api.Infrastructure.Mapping;
    using PSher.Models;

    public class ImageSimpleResponseModel : IMapFrom<Image>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsPrivate { get; set; }

        [MaxLength(ValidationConstants.MaxImageDropBoxUrlLength)]

        public string Url { get; set; }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Image, ImageSimpleResponseModel>()
                .ForMember(i => i.Url, opt => opt.MapFrom(i => i.DropboxUrl));
        }
    }
}