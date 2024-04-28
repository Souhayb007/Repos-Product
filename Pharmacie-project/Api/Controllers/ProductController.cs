using Api.Dtos;
using Api.Models;
using Api.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly  connectionString;
        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SqlServerDB"];
        }
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }


        [HttpPost]
        public IActionResult CreatProduct(ProductDto productDto) 
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO products " +
                        "(name,Description,Image,CategoryId) VALUES" +
                        "(@name,@Description,@Image,@CategoryId)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@Description", productDto.Description);
                        command.Parameters.AddWithValue("@Image", productDto.Image);
                        command.Parameters.AddWithValue("@CategoryId", productDto.CategoryId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "sorry but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetProduct() 
        {
            List<Product> products=new List<Product>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Product ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();

                                product.Id = reader.GetGuid(0);
                                product.Name = reader.GetString(1);
                                product.Description = reader.GetString(2);
                                product.Image = reader.GetString(3);
                                product.CategoryId = reader.GetGuid(4);

                                products.Add(product);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "sorry but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(products);
        }


        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) 
        {
            Product product = new Product();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Product  WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                product.Id = reader.GetGuid(0);
                                product.Name = reader.GetString(1);
                                product.Description = reader.GetString(2);
                                product.Image = reader.GetString(3);
                                product.CategoryId = reader.GetGuid(4);

                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            {
                ModelState.AddModelError("Product", "sorry but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(product);
        }

        [HttpPut]
        public IActionResult UpdateProdcut(int id, ProductDto productDto) 
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "Update  Product Set name=@name, Description=@Description, Image=@Image," +
                        " CategoryId=@CategoryId WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@Description", productDto.Description);
                        command.Parameters.AddWithValue("@Image", productDto.Image);
                        command.Parameters.AddWithValue("@CategoryId", productDto.CategoryId);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex) { }
            {
                ModelState.AddModelError("Product", "sorry but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE  FROM Product WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id",id);
                       
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "sorry but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }




        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepo.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var result = await _productRepo.SaveAsync(product);
            if (!result)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Product product)
        {
            var existingProduct = await _productRepo.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Mettre à jour les champs nécessaires
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Image = product.Image;
            existingProduct.Barcode = product.Barcode;
            existingProduct.CategoryId = product.CategoryId;

            var result = await _productRepo.UpdateAsync(existingProduct);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var existingProduct = await _productRepo.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var result = await _productRepo.DeleteAsync(existingProduct);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}