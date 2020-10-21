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
    public class ProductTypeController : ControllerBase
    {
        private IProductTypeService ProductTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            this.ProductTypeService = productTypeService;
        }

        [HttpPost]
        //[Authorize(Roles = Policies.Admin)]
        [ProducesResponseType(typeof(ProductType), 201)]
        [ProducesResponseType(400)][ProducesResponseType(500)]
        public ActionResult<ProductType> CreateProductType([FromBody] ProductType productType)
        {
            try
            {
                ProductType productTypeToAdd = ProductTypeService.CreateProductType(productType.Name);
                ProductType addedProductType = ProductTypeService.AddProductType(productTypeToAdd);

                if (addedProductType == null)
                {
                    return StatusCode(500, "Error saving product to Database");
                }

                return Created("", addedProductType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(IEnumerable<ProductType>), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<IEnumerable<ProductType>> Get([FromQuery]Filter filter)
        {
            try
            {
                IEnumerable<ProductType> productTypeEnumerable = ProductTypeService.GetProductTypesFilterSearch(filter);
                return Ok(productTypeEnumerable);
            }
            catch (InvalidDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error loading product types. Please try again...");
            }
        }

        [HttpGet("{ID}")]
        //[Authorize]
        [ProducesResponseType(typeof(ProductType), 200)]
        [ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<ProductType> GetByID(int ID)
        {
            try
            {
                ProductType type = ProductTypeService.GetProductTypeByID(ID);
                if (type != null)
                {
                    return Ok(type);
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error loading product type with ID: {ID}\nPlease try again...");
            }
        }

        [HttpPut("{ID}")]
        //[Authorize(Roles = Policies.Admin)]
        [ProducesResponseType(typeof(ProductType), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<ProductType> UpdateByID(int ID, [FromBody] ProductType type)
        {
            try
            {
                if (ProductTypeService.GetProductTypeByID(ID) == null)
                {
                    return NotFound("No product type with such ID found");
                }

                ProductType productTypeToUpdate = ProductTypeService.CreateProductType(type.Name);
                ProductType updatedProductType = ProductTypeService.UpdateProductType(productTypeToUpdate, ID);

                if(updatedProductType == null)
                {
                    return StatusCode(500, "Error updating product type in Database");
                }
                return Accepted(updatedProductType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        //[Authorize(Roles = Policies.Admin)]
        [ProducesResponseType(typeof(ProductType), 202)]
        [ProducesResponseType(400)][ProducesResponseType(404)][ProducesResponseType(500)]
        public ActionResult<ProductType> DeleteByID(int ID)
        {
            if (ProductTypeService.GetProductTypeByID(ID) == null)
            {
                return NotFound("No product type with such ID found");
            }

            try
            {
                ProductType productType = ProductTypeService.DeleteProductType(ID);
                return (productType != null) ? Accepted(productType) : StatusCode(500, $"Server error deleting product type with Id: {ID}");
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error deleting product type with Id: {ID}");
            }
        }
    }
}
