using FileImageService.EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImageService.BusinessLayer.Abstract
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }
} 