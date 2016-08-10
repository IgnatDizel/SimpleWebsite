using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using SimpleWebsite.Models;

namespace SimpleWebsite.Controllers
{
    public class HomeController : Controller
    {
        private ArticleContext _db = new ArticleContext();
        private Cache _cache = HttpRuntime.Cache;

        public ActionResult Index()
        {
            try
            {
                var articleList = _cache.Get(StatConf.ArticleListCachKey);
                if (articleList == null)
                {
                    articleList = _db.Articles.ToList();
                    _cache.Insert(StatConf.ArticleListCachKey, articleList, null, DateTime.Now.AddMinutes(StatConf.CacheExpirationTime),
                        Cache.NoSlidingExpiration);
                }

                return View(articleList);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
