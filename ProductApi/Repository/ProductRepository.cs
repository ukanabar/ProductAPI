using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.Provider;
using ProductApi.ViewModel;

namespace ProductApi.Repository
{
    public class ProductRepository : IProductRepository
    {

        IProductDataProvider _dataProvider;
        public ProductRepository(IProductDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }


        public async Task<int> AddProduct(Product product)
        {
            return await _dataProvider.AddProduct(product);
        }

        public async Task<int> DeleteProduct(int productId)
        {
            return await _dataProvider.DeleteProduct(productId);
        }

        public async Task<ProductViewModel> GetProduct(int productId)
        {
            return await _dataProvider.GetProduct(productId);
        }

        public async Task<List<ProductViewModel>> GetProducts()
        {
            return await _dataProvider.GetProducts();
        }

        public async Task UpdateProduct(Product product)
        {
            await _dataProvider.UpdateProduct(product);
        }
    }
}
