using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SimpleWebsite.Models
{
    public class ArticleContext: DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Video> Videos { get; set; }

        public ArticleContext() : base("DefaultConnection")
        {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Video>().HasMany(c => c.Articles)
                .WithMany(s => s.Videos)
                .Map(t => t.MapLeftKey("VideoId")
                .MapRightKey("ArticleId")
                .ToTable("VideoArticle"));
        } 
    }
}