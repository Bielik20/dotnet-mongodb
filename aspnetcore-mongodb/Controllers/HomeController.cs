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
        private readonly MongoDBContext _context;

        public HomeController(MongoDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Post> postList = _context.Posts.Find(m => true).ToList();

            return View();
        }

        public IActionResult About()
        {
            var entity = new Post
            {
                Content = "Without Id",
                Title = "No ID Title",
                ReadCount = 100,
            };
            _context.Posts.InsertOne(entity);


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

            await _context.Posts.InsertOneAsync(post1);
            await _context.Posts.InsertOneAsync(post2);
            await _context.Posts.InsertOneAsync(post3);

            return Redirect("/");
        }

        public IActionResult Book()
        {
            List<Post> postList = _context.Posts.Find(m => true).ToList();

            var book = new Book()
            {
                Title = "Test Book 2",
                Posts = postList
            };

            _context.Books.InsertOne(book);

            return Redirect("/");
        }

        public async Task<IActionResult> Update()
        {
            var query = from k in (from a in _context.Books.AsQueryable()
                                    where a.Title == "Test Book 2"
                                    from b in a.Posts
                                    select b)
                         where k.Title == "Second One"
                         select k;
            var result = await query.FirstOrDefaultAsync();
            result.ReadCount = 5;

            _context.Books
                .FindOneAndUpdate(x => x.Title == "Test Book 2" && x.Posts.Any(p => p.Title == "Second One"),
                                    Builders<Book>.Update.Set(x => x.Posts[-1], result));

            return Redirect("/");
        }

        public async Task<IActionResult> UpdateWithId()
        {
            var query = from k in (from a in _context.Books.AsQueryable<Book>()
                                   from b in a.Posts
                                   select b)
                        where k.Id == ObjectId.Parse("59ad72cbd33da61a704237c9")
                        select k;
            var result = await query.FirstOrDefaultAsync();
            result.ReadCount = 7;

            _context.Books
                .FindOneAndUpdate(x => x.Title == "Test Book 2" && x.Posts.Any(p => p.Title == "Third One"),
                                    Builders<Book>.Update.Set(x => x.Posts[-1], result));

            return Redirect("/");
        }
    }
}
