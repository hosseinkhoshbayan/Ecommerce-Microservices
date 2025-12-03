using Catalog.Entites.ProductEntities;
using Catalog.Specification;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Catalog.Repositories.ProductReposities
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<ProductBrand> _productBrands;
        private readonly IMongoCollection<ProductType> _productTypes;
        public ProductRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            _products = db.GetCollection<Product>(config["DatabaseSettings:ProductCollectionName"]);
            _productBrands = db.GetCollection<ProductBrand>(config["DatabaseSettings:BrandCollectionName"]);
            _productTypes = db.GetCollection<ProductType>(config["DatabaseSettings:ProductTypeCollectionName"]);
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
           var deleteProduct = await _products.DeleteOneAsync(p => p.Id == productId);
            return deleteProduct.IsAcknowledged && deleteProduct.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
           return await _products.Find(p => true).ToListAsync().ContinueWith(t => (IEnumerable<Product>)t.Result);
        }

        public async Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecParams catalogSpecParmas)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(catalogSpecParmas.Search))
            {
                filter &= builder.Where(p => p.Name.ToLower().Contains(catalogSpecParmas.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalogSpecParmas.BrandId))
            {
                filter &= builder.Eq(p=>p.Brand.Id, catalogSpecParmas.BrandId);
            }

            if(!string.IsNullOrEmpty(catalogSpecParmas.TypeId))
            {
                filter &= builder.Eq(p => p.Type.Id, catalogSpecParmas.TypeId);
            }

            var totalItems = await _products.CountDocumentsAsync(filter);

            var data = await ApplyDataFilters(catalogSpecParmas, filter);

            return new Pagination<Product>
            {
                PageIndex = catalogSpecParmas.PageIndex,
                PageSize = catalogSpecParmas.PageSize,
                Count = (int)totalItems,
                Data = data
            };
        }

      

        public async Task<ProductBrand> GetBrandByIdAsync(string productBrandId)
        {
            return await _productBrands.Find(b => b.Id == productBrandId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrandAsync(string productBrandName)
        {
            return await _products.Find(p =>
            p.Brand.Name.ToLower() == productBrandName.ToLower())
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<Product>)t.Result);
        }

        public async Task<Product> GetProductById(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string productName)
        {
            var filter = Builders<Product>.Filter.Regex(p=>p.Name, new BsonRegularExpression($".*{productName}.*","i"));
            return await _products.Find(filter)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<Product>)t.Result);
        }

        public async Task<ProductType> GetProductTypeByIdAsync(string productTypeId)
        {
           return await _productTypes.Find(t => t.Id == productTypeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updateProduct = await _products.ReplaceOneAsync(p => p.Id == product.Id, product);

            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }

        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams catalogSpecParmas, FilterDefinition<Product> filter)
        {
            var sortDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
            if (!string.IsNullOrEmpty(catalogSpecParmas.Sort))
            {
                switch (catalogSpecParmas.Sort.ToLower())
                {
                    case "priceasc":
                        sortDefinition = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "pricedesc":
                        sortDefinition = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
                        break;
                }
            }
            return await _products.Find(filter)
                .Sort(sortDefinition)
                .Skip((catalogSpecParmas.PageIndex - 1) * catalogSpecParmas.PageSize)
                .Limit(catalogSpecParmas.PageSize)
                .ToListAsync()
                .ContinueWith(t => (IReadOnlyCollection<Product>)t.Result);
        }
    }
}
