using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Controllers;
using ProductApi.Models;
using ProductApi.Provider;
using ProductApi.Repository;
using ProductApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Runtime.Serialization;

namespace ProductsApi.Test
{
    public class ProductsApiTest
    {
        private readonly Mock<IProductRepository> mockRepo;
        private readonly Mock<IProductDataProvider> mockProvider;
        private readonly ProductsController controller;
        private readonly ProductViewModel product1;
        private readonly ProductViewModel product2;
        private readonly ProductViewModel product3;
        private readonly Product productToInsert;
        private readonly Product invalidProduct;
        private readonly List<ProductViewModel> products;

        public ProductsApiTest()
        {
            mockRepo = new Mock<IProductRepository>();
            mockProvider = new Mock<IProductDataProvider>();
            controller = new ProductsController(mockRepo.Object);
            product1 = new ProductViewModel()
            {
                Id = 1,
                Name = "Tomato Soup",
                Description = "Classic Italian Soup",
                Code = "TS001",
                Price = 10
            };
            product2 = new ProductViewModel()
            {
                Id = 2,
                Name = "Yo-yo",
                Description = "Kid's toy",
                Code = "TY001",
                Price = 3.75m
            };
            product3 = new ProductViewModel()
            {
                Id = 3,
                Name = "Hammer",
                Description = "",
                Code = "TL12",
                Price = 7
            };

            products = new List<ProductViewModel>() { product1, product2, product3};

            productToInsert = new Product()
            {
                Name = "Dairy of kid",
                Description = "New Book",
                Code = "BK12",
                Price = 17
            };

            invalidProduct = new Product()
            {
                Name = "Dairy of kid",
                Description = "New Book",
                Code = "BK128374",
                Price = 17
            };
        }

        [Fact]
        public async Task GetAllProducts()
        {
            //Arrange
            mockRepo.Setup(p => p.GetProducts()).Returns(Task.FromResult(products));

            // Act
            var result = await controller.GetAsync();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var productsResult = okResult.Value.Should().BeAssignableTo<IEnumerable<ProductViewModel>>().Subject;
            productsResult.Count().Should().Be(3);
        }

        [Fact]
        public async Task GetAllProducts_NotFound()
        {
            //Arrange
            
            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetSpecificProduct()
        {
            //Arrange
            mockRepo.Setup(p => p.GetProduct(1)).Returns(Task.FromResult(product1));

            // Act
            var result = await controller.GetAsync(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var productsResult = okResult.Value.Should().BeAssignableTo<ProductViewModel>().Subject;
            productsResult.Name.Should().Be("Tomato Soup");

        }

        [Fact]
        public async Task DeleteProduct()
        {
            //Arrange
            mockRepo.Setup(p => p.DeleteProduct(1)).Returns(Task.FromResult(1));

            // Act
            var result = await controller.DeleteAsync(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_NotFound()
        {
            //Arrange
            mockRepo.Setup(p => p.DeleteProduct(1)).Returns(Task.FromResult(0));

            // Act
            var result = await controller.DeleteAsync(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddProduct()
        {
            //Arrange
            mockRepo.Setup(p => p.AddProduct(productToInsert)).Returns(Task.FromResult(5));

            // Act
            var result = await controller.PostAsync(productToInsert);

            // Assert
            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            okResult.Value.Should().Be(5);
        }

        [Fact]
        public async Task AddProduct_Exception()
        {
            //Arrange
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;

            mockRepo.Setup(p => p.AddProduct(productToInsert)).Throws(exception);

            // Act
            var result = await controller.PostAsync(productToInsert);

            // Assert
            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task AddProduct_BadRequest()
        {
            //Arrange
            mockRepo.Setup(p => p.AddProduct(invalidProduct)).Returns(Task.FromResult(5));

            // Act
            controller.ModelState.AddModelError("ProductCode", "Product Code is longer than 5 characters.");
            var result = await controller.PostAsync(invalidProduct);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;            
            
        }

        [Fact]
        public async Task UpdateProduct()
        {
            // Arrange
            mockRepo.Setup(p => p.GetProduct(17)).Returns(Task.FromResult(product1));
            mockRepo.Setup(p => p.UpdateProduct(productToInsert)).Returns(Task.FromResult(1));

            // Act
            var result = await controller.Put(17, productToInsert);

            // Assert
            result.Should().BeOfType<OkResult>();
           
        }

        [Fact]
        public async Task UpdateProduct_NotFound()
        {
            // Arrange
            mockRepo.Setup(p => p.UpdateProduct(productToInsert)).Returns(Task.FromResult(1));

            // Act
            var result = await controller.Put(17, productToInsert);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

        }
    }
}
