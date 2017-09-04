using aspnetcore_mongodb.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_mongodb
{
    public class MongoDBContext
    {
        private MongoDBOptions _options;
        private IMongoDatabase _database { get; }

        public MongoDBContext(IOptions<MongoDBOptions> options)
        {
            _options = options.Value;
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_options.ConnectionString));
                if (_options.IsSSL)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                }
                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(_options.DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
        }

        public IMongoCollection<Post> Posts
        {
            get
            {
                return _database.GetCollection<Post>("Posts");
            }
        }

        public IMongoCollection<Book> Books
        {
            get
            {
                return _database.GetCollection<Book>("Books");
            }
        }
    }
}
