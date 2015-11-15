namespace PSher.Api.DataTransferModels.Marks
{
    using AutoMapper;
    using PSher.Api.Infrastructure.Mapping;
    using PSher.Models;

    public class MarkResponseModel : IMapFrom<Mark>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public string GivenBy { get; set; }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Mark, MarkResponseModel>()
                .ForMember(m => m.GivenBy, opts => opts.MapFrom(m => m.GivenBy.UserName));
        }
    }
}