using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Category
    {
        [Key]
        // khoa chinh
        public int CategoryId { get; set; }
        public string? TenDanhMuc { get; set; }
        public string? ImageUrl { get; set; }
        public string? Icon { get; set; }

    }
}
