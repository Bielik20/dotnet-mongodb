using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnetcore_mongodb.Models;
using MongoDB.Driver;

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
                Posts =postList
            };

            dbContext.Books.InsertOne(book);

            return Redirect("/");
        }

        public IActionResult Update()
        {
            MongoDBContext dbContext = new MongoDBContext();

            var test = dbContext.Books.AsQueryable<Book>().Where(x => x.Title == "Test Book 1" && x.Posts.Any(p => p.Title == "Updated")).Select(b => b.Posts).FirstOrDefault();
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
