using Blog.Entities.DTO;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entities.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<ArticleDTO> Articles { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string SearchString { get; set; }
    }
}
