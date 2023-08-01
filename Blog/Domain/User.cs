using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain
{
    public class User : IdentityUser
    {
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; private set; }

        public User() { }

        public void AddImgUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
        }
    }
}
