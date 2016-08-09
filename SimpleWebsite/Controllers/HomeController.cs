using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using SimpleWebsite.Models;

namespace SimpleWebsite.Controllers
{
    public class HomeController : Controller
    {
        ArticleContext db = new ArticleContext();
        private Cache cache = HttpRuntime.Cache;

        public ActionResult Index()
        {

            if (cache.Get("List") == null)
            {
                try
                {
                    cache.Insert("List", db.Articles.ToList(), null, DateTime.Now.AddMinutes(2), Cache.NoSlidingExpiration);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
            }
            
            return View(cache.Get("List"));
        }

    }
}
