using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


            services.AddSingleton(serviceProvider =>
           {

               var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
               var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
               var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
               return mongoClient.GetDatabase(serviceSettings.ServiceName);
           });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
        where T : IEntity
        {

            services.AddSingleton<IRepository<T>>(ServiceProvider =>
                {
                    var dataBase = ServiceProvider.GetService<IMongoDatabase>();
                    return new MongoRepository<T>(dataBase, collectionName);
                });

            return services;
        }
    }
}