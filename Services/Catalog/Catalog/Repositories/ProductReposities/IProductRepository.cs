using Catalog.Entites.ProductEntities;
using Catalog.Specification;

namespace Catalog.Repositories.ProductReposities
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductAsync();

        Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecParams catalogSpecParmas);

        Task<IEnumerable<Product>> GetProductByNameAsync(string productName);

        Task<IEnumerable<Product>> GetProductByBrandAsync(string productBrandName);

        Task<Product> GetProductById(string productId);

        Task<Product> CreateProduct(Product product);

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(string productId);

        Task<ProductBrand> GetBrandByIdAsync(string productBrandId);

        Task<ProductType> GetProductTypeByIdAsync(string productTypeId);


    }
}
