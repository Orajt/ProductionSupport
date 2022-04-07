using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ArticleFilePath
    {
        public int Id { get; set; }
        public int ArticleId{get;set;}
        public Article Article{get;set;}
        [Required]
        public string FileType { get; set; }
        [Required]
        public string Path {get;set;}
        public string FileName{get;set;}
    }
}