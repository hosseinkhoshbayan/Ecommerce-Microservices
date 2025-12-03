using Catalog.Entites.ProductEntities;
using MongoDB.Driver;

namespace Catalog.Repositories.ProductReposities
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly IMongoCollection<ProductType> _productTypes;

        public ProductTypeRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            _productTypes = db.GetCollection<ProductType>(config["DatabaseSettings:ProductTypeCollectionName"]);
        }
        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _productTypes.Find(_ => true).ToListAsync();
        }

        public async Task<ProductType> GetProductTypeByIdAsync(string id)
        {
            return await _productTypes.Find(t=>t.Id == id).FirstOrDefaultAsync();
        }
    }
}
