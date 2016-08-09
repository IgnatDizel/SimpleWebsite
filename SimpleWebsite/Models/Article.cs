using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleWebsite.Models
{
    public class Article
    {
        public int Id { get; set; }

//        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Заглавие статьи")]
//        [StringLength(150, ErrorMessage = "Длина заглавия не должна быть более 150 символов")]
        public string Title { get; set; }

//        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Текст статьи")]
        public string Content { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public Article()
        {
            Videos = new List<Video>();
        }
    }
}