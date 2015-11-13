namespace PSher.Api.DataTransferModels.Album
{
    using Infrastructure.Mapping;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper;
    using PSher.Models;

    public class AlbumDetailsResponseModel : IMapFrom<Album>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<string> Tags { get; set; }

        public virtual ICollection<string> NameOfAllPhotos { get; set; }

        public virtual ICollection<string> UrlsOfPhotos { get; set; }

        public string IdCreator { get; set; }

        public string CreatorName { get; set; }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Album, AlbumDetailsResponseModel>()
                .ForMember(a => a.Tags, opts => opts.MapFrom(a => a.Tags.Select(t => t.Name).ToList()))
                .ForMember(a => a.NameOfAllPhotos, opts => opts.MapFrom(a => a.Images.Select(i => i.Title).ToList()))
                .ForMember(a => a.UrlsOfPhotos, opts => opts.MapFrom(a => a.Images.Select(i => i.DropboxUrl).ToList()))
                .ForMember(a => a.IdCreator, opts => opts.MapFrom(c => c.Creator.Id))
                .ForMember(a => a.CreatorName, opts => opts.MapFrom(c => c.Creator.UserName));
        }
    }
}