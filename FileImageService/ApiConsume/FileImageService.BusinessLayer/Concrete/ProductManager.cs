using FileImageService.BusinessLayer.Abstract;
using FileImageService.DataAccessLayer.Abstract;
using FileImageService.EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImageService.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productDal.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productDal.GetByIdAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            return await _productDal.AddAsync(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _productDal.UpdateAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productDal.DeleteAsync(id);
        }
    }
} 