using System.Security.Cryptography.X509Certificates;
using ThirdParty.BouncyCastle.Utilities.IO.Pem;

namespace Play.Catalog.Service.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; init; }
        public int Port {get; init;}
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}