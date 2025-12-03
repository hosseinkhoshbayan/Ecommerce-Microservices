using Catalog.Entites.ProductEntities;
using MongoDB.Driver;

namespace Catalog.Repositories.ProductReposities
{
    public class BrandRepository : IBrandRepository
    {

        private readonly IMongoCollection<ProductBrand> _brand;

        public BrandRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            _brand = db.GetCollection<ProductBrand>(config["DatabaseSettings:BrandCollectionName"]);
        }


        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _brand.Find(_ => true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string id)
        {
            return await _brand.Find(b=>b.Id == id).FirstOrDefaultAsync();
        }
    }
}
