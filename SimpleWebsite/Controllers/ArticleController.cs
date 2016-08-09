using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using SimpleWebsite.Models;

namespace SimpleWebsite.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleContext db = new ArticleContext();
        private Cache cache = HttpRuntime.Cache;

        public ActionResult Details(int id = 0)
        {
            string key = "DeteilsById:" + id;


            if (cache.Get(key) == null)
            {
                try
                {
                    cache.Insert(key, db.Articles.Find(id), null, DateTime.Now.AddMinutes(2), Cache.NoSlidingExpiration);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
            }

            if (cache.Get(key) == null)
            {
                return HttpNotFound();
            }
            return View(cache.Get(key));
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.Videos = db.Videos.ToList();
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

            if (ModelState.IsValid)
            {
                if (selectedVideos != null)
                {
                    if (selectedVideos.Length > 5)
                    {
                        ViewBag.Message = "Non Valid"; //TODO: Вывести ошибку
                        return RedirectToAction("Edit", "Article", new { id = article.Id });
                    }
                    try
                    {
                        foreach (var v in db.Videos.Where(vi => selectedVideos.Contains(vi.Id)))   //TODO: Херня! Исправить
                        {
                            article.Videos.Add(v);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.errorMessage = ex.Message;
                        return View("Error");
                    }
                    foreach (var v in db.Videos.Where(vi => selectedVideos.Contains(vi.Id)))
                    {
                        article.Videos.Add(v);
                    }
                }
                try
                {
                    db.Entry(article).State = EntityState.Added;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
                
                return RedirectToAction("Index", "Home");

            }

            ViewBag.Message = "Non Valid";
            return RedirectToAction("Create", "Article");
        }

        public ActionResult Edit(int id = 0)
        {
            Article article;
            try
            {
                article = db.Articles.Find(id);
                ViewBag.Videos = db.Videos.ToList();
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
            
            if (article == null)
            {
                return HttpNotFound();
            }
            
            return View(article);
        }

        [HttpPost]
        public ActionResult Edit(Article article, int?[] selectedVideos)
        {
            Article newArticle;
            if (ModelState.IsValid)
            {
                try
                {
                    newArticle = db.Articles.Find(article.Id);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");                    
                }
                newArticle.Title = article.Title;
                newArticle.Content = article.Content;

                newArticle.Videos.Clear();

                if (selectedVideos != null)
                {
                    if (selectedVideos.Length > 5)
                    {
                        ViewBag.Message = "Non Valid"; //TODO: Вывести ошибку
                        return RedirectToAction("Edit", "Article", new { id = article.Id });
                    }
                    try
                    {
                        foreach (var v in db.Videos.Where(vi => selectedVideos.Contains(vi.Id)))
                        {
                            newArticle.Videos.Add(v);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.errorMessage = ex.Message;
                        return View("Error");
                    }
                }
                try
                {
                    db.Entry(newArticle).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    return View("Error");
                }
                
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Non Valid";              //TODO: Вывести ошибку
            return RedirectToAction("Edit", "Article", new { id = article.Id });
        }

        public ActionResult Delete(int id = 0)
        {
            Article article;
            try
            {
                article = db.Articles.Find(id);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");  
            }
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article;
            try
            {
                article = db.Articles.Find(id);
                db.Articles.Remove(article);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
            
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}