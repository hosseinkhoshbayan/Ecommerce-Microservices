using Catalog.Entites.ProductEntities;

namespace Catalog.Repositories.ProductReposities
{
    public interface IProductTypeRepository
    {
        Task<IEnumerable<ProductType>> GetAllTypes();

        Task<ProductType> GetProductTypeByIdAsync(string id);
    }
}
