using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Blog.Models
{
    public class Posts
    {
        [Key]
        public int PostId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string NamePost { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(500)")]
        public string? Avatar { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [AllowHtml]
        public string? ContentSort { get; set; }
        [AllowHtml]
        public string? Description { get; set; }
        public string? SearchString { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
