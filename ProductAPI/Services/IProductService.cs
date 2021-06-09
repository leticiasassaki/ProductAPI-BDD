using ProductAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductAPI.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(long id);
        Task AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task DeleteProduct(Product product);
    }
}
