using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entities;

namespace PetShop.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductService ProductService;

        public ProductController(IProductService productService)
        {
            this.ProductService = productService;
        }

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            try
            {
                Product productToAdd = ProductService.CreateProduct(product.Name, product.Type, product.Price, product.CreatedDate);
                Product addedProduct;

                if (string.IsNullOrEmpty(product.Type))
                {
                    return BadRequest("No product type selected");
                }

                addedProduct = ProductService.AddProduct(productToAdd);

                if (addedProduct == null)
                {
                    return StatusCode(500, "Error saving product to Database");
                }

                return Created("", addedProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<IEnumerable<Product>> Get([FromQuery]Filter filter)
        {
            try
            {
                IEnumerable<Product> productEnumerable = ProductService.GetProductsFilterSearch(filter);
                return Ok(productEnumerable);
            }
            catch (InvalidDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Error loading products. Please try again...");
            }
        }

        [HttpGet("{ID}")]
        //[Authorize]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Product> GetByID(int ID)
        {
            try
            {
                Product product = ProductService.GetProductByID(ID);
                if (product != null)
                {
                    return Ok(product);
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error loading product with ID: {ID}\nPlease try again...");
            }
        }

        [HttpPut("{ID}")]
        //[Authorize]
        [ProducesResponseType(typeof(Product), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Product> UpdateByID(int ID, [FromBody] Product product)
        {
            try
            {
                Product existingProduct = ProductService.GetProductByID(ID);

                if (existingProduct == null)
                {
                    return NotFound("No product with such ID found");
                }

                Product productToUpdate = ProductService.CreateProduct(product.Name, product.Type, product.Price, product.CreatedDate);

                if (string.IsNullOrEmpty(product.Type))
                {
                    return BadRequest("No product type selected");
                }

                Product updatedProduct = ProductService.UpdateProduct(productToUpdate, ID);

                if(updatedProduct == null)
                {
                    return StatusCode(500, "Error updating product in Database");
                }
                return Accepted(updatedProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        //[Authorize]
        [ProducesResponseType(typeof(Product), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<Product> DeleteByID(int ID)
        {
            if (ProductService.GetProductByID(ID) == null)
            {
                return NotFound("No product with such ID found");
            }

            try
            {
                Product product = ProductService.DeleteProduct(ID);
                return (product != null) ? Accepted(product) : StatusCode(500, $"Server error deleting product with Id: {ID}");
            }
            catch(ArgumentException ex)
            {
               return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error deleting product with Id: {ID}");
            }
        }
    }
}
