using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleWebsite.Models
{
    public class Video
    {
        public int Id { get; set; }

//        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "url видео")]
        [RegularExpression(@"@^(https?|ftp)://[^\s/$.?#].[^\s]*$@iS", ErrorMessage = "Некорректный адрес")]
        public string VideoUrl { get; set; }

//        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Название видео")]
        public string VideoTitle { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public Video()
        {
            Articles = new HashSet<Article>();
        }
    }
}