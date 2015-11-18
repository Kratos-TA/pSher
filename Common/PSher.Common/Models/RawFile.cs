namespace PSher.Common.Models
{
    public class RawFile
    {
        public string OriginalFileName { get; set; }
        
        public string FileExtension { get; set; }
      
        public byte[] Content { get; set; }

        public byte[] PreviewContent { get; set; }
    }
}
