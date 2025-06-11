using FileImageService.EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImageService.DataAccessLayer.Abstract
{
    public interface IProductDal
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
} 