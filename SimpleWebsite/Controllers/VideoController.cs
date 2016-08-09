using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using SimpleWebsite.Models;

namespace SimpleWebsite.Controllers
{
    public class VideoController : Controller
    {
        private ArticleContext db = new ArticleContext();
        private Cache cache = HttpRuntime.Cache;

        public ActionResult List()
        {
            if (cache.Get("ListVideo") == null)
            {
                try
                {
                    cache.Insert("ListVideo", db.Videos.ToList(), null, DateTime.Now.AddMinutes(2),
                        Cache.NoSlidingExpiration);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
            }
            return View(cache.Get("ListVideo"));
        }

        public ActionResult Create()
        {
            if (cache.Get("allVideo") == null)
            {
                try
                {
                    cache.Insert("allVideo", db.Articles.ToList(), null, DateTime.Now.AddMinutes(2),
                        Cache.NoSlidingExpiration);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
            }
            ViewBag.Articles = cache.Get("allVideo");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Video video, int[] selectedArticles)
        {
            for (int i = 0; i < selectedArticles.Length; i++)
            {
                if (db.Articles.Find(selectedArticles[i]).Videos.Count >= 5)
                {
                    ViewBag.Message = "Non Valid";
                    return RedirectToAction("Create", "Video");
                }
            }


            if (ModelState.IsValid)
            {
                if (selectedArticles != null)
                {
                    foreach (var v in db.Articles.Where(vi => selectedArticles.Contains(vi.Id)))
                    {
                        video.Articles.Add(v);
                    }
                    try
                    {
                        db.Entry(video).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.errorMessage = ex.Message;
                        return View("Error");
                    }
                    return RedirectToAction("Index", "Home");

                }
            }

            ViewBag.Message = "Non Valid";
            return RedirectToAction("Create", "Video");
        }

        public ActionResult Edit(int id = 0)
        {
            Video video;
            try
            {
                video = db.Videos.Find(id);

            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);

        }

        [HttpPost]
        public ActionResult Edit(Video video)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(video).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
                return RedirectToAction("List");

            }
            return View(video);
        }

        public ActionResult Delete(int id = 0)
        {
            Video video;
            try
            {
                video = db.Videos.Find(id);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video;
            try
            {
                video = db.Videos.Find(id);
                db.Videos.Remove(video);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}