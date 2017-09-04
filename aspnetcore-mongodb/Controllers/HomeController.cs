using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnetcore_mongodb.Models;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace aspnetcore_mongodb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            MongoDBContext dbContext = new MongoDBContext();

            List<Post> postList = dbContext.Posts.Find(m => true).ToList();

            return View();
        }

        public IActionResult About()
        {
            MongoDBContext dbContext = new MongoDBContext();
            var entity = new Post
            {
                Content = "Without Id",
                Title = "No ID Title",
                ReadCount = 100,
            };
            dbContext.Posts.InsertOne(entity);


            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Book()
        {
            MongoDBContext dbContext = new MongoDBContext();

            List<Post> postList = dbContext.Posts.Find(m => true).ToList();

            var book = new Book()
            {
                Title = "Test Book 2",
                Posts = postList
            };

            dbContext.Books.InsertOne(book);

            return Redirect("/");
        }

        public async Task<IActionResult> Update()
        {
            MongoDBContext dbContext = new MongoDBContext();

            //var test = dbContext.Books.AsQueryable<Book>()
            //            .Where(x => x.Title == "Test Book 1")
            //            .Select(b => b.Posts.Where(p => p.Title == "No ID Title"))
            //            .ToList();

            var test = dbContext.Books.Find(b => b.Title == "Test Book 1" && b.Posts.Any(x => x.Title == "Updated"))
                        .Project(b => b.Posts)
                        .FirstOrDefault();

            //var filter = Builders<Book>.Filter.Eq("Posts.Title", "Updated");
            //var projection = Builders<Book>.Projection.Include("Posts.$").Exclude("_id");
            //var post = dbContext.Books.Find(filter).Project(projection).SingleOrDefault();

            //var query = from b in dbContext.Books.AsQueryable()
            //            where b.Title == "Test Book 2"
            //            from p in b.Posts.AsQueryable()
            //            where p.Title == "No ID Title"
            //            select p;
            //var test2 = query.FirstOrDefault();

            //var test3 = dbContext.Books.AsQueryable()
            //            .SelectMany(b => b.Posts, (b, p) => new { Book = b, Post = p })
            //            .Where(x => x.Book.Title == "Test Book 2" && x.Post.Title == "Updated")
            //            .Select(x => x.Post)
            //            .FirstOrDefault();

            var result = from k in (from a in dbContext.Books.AsQueryable<Book>()
                                    where a.Title == "Test Book 2"
                                    from b in a.Posts
                                    select b)
                         where k.Title == "No ID Title"
                         select k;
            var test2 = await result.FirstOrDefaultAsync();


            //var newPost = new Post
            //{
            //    Id = new Guid(),
            //    Title = "Updated",
            //    Content = "This is awesome",
            //    ReadCount = 1
            //};


            //dbContext.Books.FindOneAndUpdate(x => x.Title == "Test Book 1" && x.Posts.Any(p => p.Title == "Updated"),
            //                                                    Builders<Book>.Update.Set(x => x.Posts[-1], newPost));



            return Redirect("/");
        }




    }
}
