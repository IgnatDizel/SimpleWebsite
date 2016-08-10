using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using SimpleWebsite.Models;

namespace SimpleWebsite.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleContext _db = new ArticleContext();
        private Cache _cache = HttpRuntime.Cache;

        public ActionResult Details(int id = 0)
        {
            string articleDeteilsCachKey = "DeteilsById:" + id;


            if (_cache.Get(articleDeteilsCachKey) == null)
            {
                try
                {
                    _cache.Insert(articleDeteilsCachKey, _db.Articles.Find(id), null, DateTime.Now.AddMinutes(StatConf.CacheExpirationTime), Cache.NoSlidingExpiration);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
            }

            if (_cache.Get(articleDeteilsCachKey) == null)
            {
                return HttpNotFound();
            }
            return View(_cache.Get(articleDeteilsCachKey));
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.Videos = _db.Videos.ToList();
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Article article, int[] selectedVideos)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Non Valid";
                return RedirectToAction("Create", "Article");
            }

            try
            {
                if (selectedVideos != null)
                {
                    if (selectedVideos.Length > 5)
                    {
                        ViewBag.Message = "Non Valid"; //TODO: Вывести ошибку
                        return RedirectToAction("Edit", "Article", new {id = article.Id});
                    }

                    foreach (var v in _db.Videos.Where(vi => selectedVideos.Contains(vi.Id)))
                    {
                        article.Videos.Add(v);
                    }
                }

                _db.Entry(article).State = EntityState.Added;
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var article = _db.Articles.Find(id);

                if (article == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Videos = _db.Videos.ToList();

                return View(article);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(Article article, int[] selectedVideos)
        {
            if (!ModelState.IsValid || (selectedVideos != null && selectedVideos.Length > 5))
            {
                ViewBag.Message = "Non Valid"; //TODO: Вывести ошибку
                return RedirectToAction("Edit", "Article", new {id = article.Id});
            }

            try
            {
                var newArticle = _db.Articles.Find(article.Id);

                if (newArticle == null)
                {
                    ViewBag.Message = "Non Valid"; //TODO: Вывести ошибку
                    return RedirectToAction("Edit", "Article", new { id = article.Id });
                }

                newArticle.Title = article.Title;
                newArticle.Content = article.Content;

                newArticle.Videos.Clear();

                if (selectedVideos != null)
                {
                    foreach (var v in _db.Videos.Where(vi => selectedVideos.Contains(vi.Id)))
                    {
                        newArticle.Videos.Add(v);
                    }
                }

                _db.Entry(newArticle).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Article article = _db.Articles.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                return View(article);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Article article = _db.Articles.Find(id);
                _db.Articles.Remove(article);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}