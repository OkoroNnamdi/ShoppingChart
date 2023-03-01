using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CmsShoppingChart.Extensions;
using Microsoft.AspNetCore.Http;

namespace CmsShoppingChart.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "The mininum length is 2")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "The mininum length is 4")]
        public string Description { get; set; }
        public string Slug { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal ? Price { get; set; }

        public string Image { get; set; }
        [DisplayName("Category")]
        [Range (1, int.MaxValue, ErrorMessage ="You must choose a Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category  Category { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
