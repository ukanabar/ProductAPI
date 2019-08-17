using ProductApi.Models;
using ProductApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductViewModel>> GetProducts();

        Task<ProductViewModel> GetProduct(int productId);

        Task<int> AddProduct(Product product);

        Task<int> DeleteProduct(int productId);

        Task UpdateProduct(Product product);
    }
}
