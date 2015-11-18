namespace PSher.Api.DataTransferModels.Album
{
    using System;
    using System.Linq;
    using AutoMapper;
    using PSher.Api.Infrastructure.Mapping;
    using PSher.Models;

    public class AlbumResponseModel : IMapFrom<Album>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UrlOfPicture { get; set; }

        public string Name { get; set; }

        public string CreatorName { get; set; }

        public bool IsPrivate { get; set; }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Album, AlbumResponseModel>()
                .ForMember(a => a.UrlOfPicture, opts => opts.MapFrom(a => a.Images.FirstOrDefault().Url))
                .ForMember(a => a.CreatorName, opts => opts.MapFrom(a => a.Creator.UserName));
        }
    }
}