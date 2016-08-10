using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using SimpleWebsite.Models;

namespace SimpleWebsite.Controllers
{
    public class VideoController : Controller
    {
        private ArticleContext _db = new ArticleContext();
        private Cache _cache = HttpRuntime.Cache;

        public ActionResult List()
        {
            try
            {
                var videoList = _cache.Get(StatConf.VideoListCachKey);
                if (videoList == null)
                {
                    videoList = _db.Videos.ToList();
                    _cache.Insert(StatConf.VideoListCachKey, videoList, null, DateTime.Now.AddMinutes(StatConf.CacheExpirationTime),
                        Cache.NoSlidingExpiration);
                }

                return View(videoList);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            if (_cache.Get(StatConf.ArticleListCachKey) == null)
            {
                try
                {
                    _cache.Insert(StatConf.ArticleListCachKey, _db.Articles.ToList(), null, DateTime.Now.AddMinutes(StatConf.CacheExpirationTime),
                        Cache.NoSlidingExpiration);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
            }
            ViewBag.Articles = _cache.Get(StatConf.ArticleListCachKey);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Video video, int[] selectedArticles)
        {
            try
            {
                if (selectedArticles != null && selectedArticles.Any(t => _db.Articles.Find(t).Videos.Count >= 5))
                {
                    ViewBag.Message = "Non Valid";
                    return RedirectToAction("Create", "Video");
                }

                if (selectedArticles != null)
                {
                    foreach (var v in _db.Articles.Where(vi => selectedArticles.Contains(vi.Id)))
                    {
                        video.Articles.Add(v);
                    }
                }
                _db.Entry(video).State = EntityState.Added;
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
                Video video = _db.Videos.Find(id);
                if (video == null)
                {
                    return HttpNotFound();
                }
                return View(video);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(Video video)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Entry(video).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
            }
            return View(video);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Video video = _db.Videos.Find(id);
                if (video == null)
                {
                    return HttpNotFound();
                }
                return View(video);
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
                Video video = _db.Videos.Find(id);
                _db.Videos.Remove(video);
                _db.SaveChanges();
                return RedirectToAction("List");
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