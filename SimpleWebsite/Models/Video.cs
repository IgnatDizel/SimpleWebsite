using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SimpleWebsite.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Display(Name = "src видео")]
        public string VideoUrl { get; set; }

        [Display(Name = "Название видео")]
        public string VideoTitle { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public Video()
        {
            Articles = new HashSet<Article>();
        }
    }
}