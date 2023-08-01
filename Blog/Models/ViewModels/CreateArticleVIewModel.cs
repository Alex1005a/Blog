using System.ComponentModel.DataAnnotations;

namespace Blog.Models.ViewModels
{
    public class CreateArticleViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Текст")]
        public string Body { get; set; }
    }
}
