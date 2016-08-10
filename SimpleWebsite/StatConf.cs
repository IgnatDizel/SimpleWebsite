using SimpleWebsite.Models;

namespace SimpleWebsite
{
    public static class StatConf
    {
        public static ArticleContext db = new ArticleContext();
        public const int CacheExpirationTime = 2;
        public const string VideoListCachKey = "ListVideo";
        public const string ArticleListCachKey = "articleList";
    }
}