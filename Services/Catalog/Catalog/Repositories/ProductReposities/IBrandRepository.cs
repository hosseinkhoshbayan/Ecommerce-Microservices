using System.Collections;
using Catalog.Entites.ProductEntities;

namespace Catalog.Repositories.ProductReposities
{
    public interface IBrandRepository
    {
        Task<IEnumerable<ProductBrand>> GetAllBrands();
        
        Task<ProductBrand> GetBrandByIdAsync(string id);


    }
}
