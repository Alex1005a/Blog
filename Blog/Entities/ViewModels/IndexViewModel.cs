using Blog.Entities.DTO;
using System.Collections.Generic;

namespace Blog.Entities.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<ArticleDTO> Articles { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string SearchString { get; set; }
    }
}
