using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.Repository;
using ProductApi.ViewModel;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET api/products
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET api/products/5
        [HttpGet("{id}")]
        [ActionName("GetProduct")]
        public async Task<ActionResult> GetAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // POST api/products
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Product product)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    var productId = await _productRepository.AddProduct(product);
                    if (productId > 0)
                    {
                        return CreatedAtAction("GetProduct", new { id = productId }, productId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateException e)
                {
                    if(e.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                    {
                        return StatusCode((int)HttpStatusCode.Conflict);
                    }
                }
  
                catch (Exception e)
                {

                    return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
                }

            }

            return BadRequest(ModelState);
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {


            var dbProduct = await _productRepository.GetProduct(id);

            if (dbProduct == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    product.Id = id;
                    await _productRepository.UpdateProduct(product);

                    return Ok();
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                    {
                        return StatusCode((int)HttpStatusCode.Conflict);
                    }
                }
                catch (Exception e)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
                }
            }

            return BadRequest(ModelState);
        }
    

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            int result = 0;
            try
            {
                result = await _productRepository.DeleteProduct(id);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
