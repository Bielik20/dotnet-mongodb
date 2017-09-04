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

        public async Task<IActionResult> Post()
        {
            MongoDBContext dbContext = new MongoDBContext();

            var post1 = new Post()
            {
                Title = "First One",
                Content = "Try to beat me",
                ReadCount = 10
            };
            var post2 = new Post()
            {
                Title = "Second One",
                Content = "How do you like them apples",
                ReadCount = 10
            };
            var post3 = new Post()
            {
                Title = "Third One",
                Content = "With friends like this",
                ReadCount = 10
            };

            await dbContext.Posts.InsertOneAsync(post1);
            await dbContext.Posts.InsertOneAsync(post2);
            await dbContext.Posts.InsertOneAsync(post3);

            return Redirect("/");
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

            var query = from k in (from a in dbContext.Books.AsQueryable<Book>()
                                    where a.Title == "Test Book 2"
                                    from b in a.Posts
                                    select b)
                         where k.Title == "Second One"
                         select k;
            var result = await query.FirstOrDefaultAsync();
            result.ReadCount = 5;

            dbContext.Books
                .FindOneAndUpdate(x => x.Title == "Test Book 2" && x.Posts.Any(p => p.Title == "Second One"),
                                    Builders<Book>.Update.Set(x => x.Posts[-1], result));

            return Redirect("/");
        }

        public async Task<IActionResult> UpdateWithId()
        {
            MongoDBContext dbContext = new MongoDBContext();

            var query = from k in (from a in dbContext.Books.AsQueryable<Book>()
                                   from b in a.Posts
                                   select b)
                        where k.Id == ObjectId.Parse("59ad72cbd33da61a704237c9")
                        select k;
            var result = await query.FirstOrDefaultAsync();
            result.ReadCount = 7;

            dbContext.Books
                .FindOneAndUpdate(x => x.Title == "Test Book 2" && x.Posts.Any(p => p.Title == "Third One"),
                                    Builders<Book>.Update.Set(x => x.Posts[-1], result));

            return Redirect("/");
        }
    }
}
