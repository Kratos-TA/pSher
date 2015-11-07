namespace PSher.Api.Models.Images
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PSher.Common.Constants;

    public class SaveImageRequestModel
    {
        // string title, string authorUserName, int albumId, string description, bool isPrivate, ICollection<string> imageTags

        private ICollection<string> tags;

        public SaveImageRequestModel()
        {
            this.tags = new HashSet<string>();
        }

        [Required]
        [MinLength(ValidationConstants.MinImageTitle)]
        [MaxLength(ValidationConstants.MaxImageTitle)]
        public string Title { get; set; }

        [MaxLength(ValidationConstants.MaxImageDescription)]
        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        [Required]
        public string AuthorUserName { get; set; }

        // TODO: Set the ImageInfo !!!
        //public virtual ImageInfo ImageInfo { get; set; }

        public virtual ICollection<string> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

    }
}