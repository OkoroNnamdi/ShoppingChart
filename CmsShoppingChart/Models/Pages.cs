using System.ComponentModel.DataAnnotations;

namespace CmsShoppingChart.Models
{
    public class Pages
    {
        public int Id { get; set; }
       [Required,MinLength(2,ErrorMessage ="The mininum length is 2")]
        public string Title { get; set; }
        public string Slug { get; set; }
       [Required, MinLength (4,ErrorMessage ="The mininum length is 4")]
        public string Content { get; set; }
        public int Sorting { get; set; }
    }
}
