using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.ViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
