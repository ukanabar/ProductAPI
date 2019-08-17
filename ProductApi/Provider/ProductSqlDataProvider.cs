using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Provider
{
    public class ProductSqlDataProvider: IProductDataProvider
    {
        ProductContext _db;
        public ProductSqlDataProvider(ProductContext db)
        {
            _db = db;
        }

        public async Task<int> AddProduct(Product product)
        {
            if (_db != null)
            {
                await _db.Product.AddAsync(product);
                await _db.SaveChangesAsync();

                return product.Id;
            }

            return 0;
        }

        public async Task<int> DeleteProduct(int productId)
        {
            int result = 0;

            if (_db != null)
            {
                //Find the post for specific post id
                var product = await _db.Product.FirstOrDefaultAsync(x => x.Id == productId);

                if (product != null)
                {
                    //Delete that post
                    _db.Product.Remove(product);

                    //Commit the transaction
                    result = await _db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<ProductViewModel> GetProduct(int productId)
        {
            return await(from p in _db.Product
                         where p.Id == productId
                         select new ProductViewModel
                         {
                             Id = p.Id,
                             Name = p.Name,
                             Code = p.Code,
                             Description = p.Description,
                             Price = p.Price,
                             CreatedOn = p.CreatedOn
                         }).FirstOrDefaultAsync();
        }

        public async Task<List<ProductViewModel>> GetProducts()
        {
            if (_db != null)
            {
                return await(from p in _db.Product
                             select new ProductViewModel
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Code = p.Code,
                                 Description = p.Description,
                                 Price = p.Price,
                                 CreatedOn = p.CreatedOn
                             }).ToListAsync();
            }

            return null;
        }

        public async Task UpdateProduct(Product product)
        {
            if (_db != null)
            {
                //Delete that post
                _db.Product.Update(product);

                //Commit the transaction
                await _db.SaveChangesAsync();
            }
        }
    }
}
