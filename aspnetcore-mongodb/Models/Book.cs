using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_mongodb.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<Post> Posts { get; set; }
    }
}
