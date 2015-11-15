namespace PSher.Api.DataTransferModels.Images
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using PSher.Api.DataTransferModels.Marks;
    using PSher.Api.Infrastructure.Mapping;
    using PSher.Models;

    public class ImageRatingResponseModel : IMapFrom<Rating>, IHaveCustomMappings
    {
        private ICollection<MarkResponseModel> marks;

        public ImageRatingResponseModel()
        {
            this.marks = new HashSet<MarkResponseModel>();
        }

        public int Id { get; set; }

        public virtual ICollection<MarkResponseModel> Marks
        {
            get { return this.marks; }
            set { this.marks = value; }
        }

        public double Average { get { return this.Marks.Select(m => m.Value).Average(); } }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<Rating, ImageRatingResponseModel>()
                .ForMember(r => r.Marks, opts => opts.MapFrom(r => r.Marks));
        }
    }
}