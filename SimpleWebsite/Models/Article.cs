using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SimpleWebsite.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Display(Name = "Заглавие статьи")]
        public string Title { get; set; }

        [Display(Name = "Текст статьи")]
        public string Content { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public Article()
        {
            Videos = new List<Video>();
        }
    }
}