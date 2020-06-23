using eShop.Business.Interfaces;
using eShop.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using eShopService.Services;
using Microsoft.AspNetCore.Authorization;

namespace eShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly ILogger<ProductsController> _logger;
        //private readonly IUserService _userService;

        public ProductsController(IProductManager productManager
            , ILogger<ProductsController> logger
            )//, IUserService userService)
        {
            _productManager = productManager;
            _logger = logger;
           // _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productManager.GetProducts();
                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var product = await _productManager.GetProduct(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] Product newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productId = await _productManager.AddProduct(newProduct);
                    if (productId > 0)
                    {
                        return Ok(productId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest();
                }

            }

            return BadRequest();

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product productChanges)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var changedProduct = await _productManager.UpdateProduct(productChanges);
                    if (changedProduct != null)
                    {
                        return Ok();
                    }

                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, Product product)
        {
            try
            {
                var isSuccess = await _productManager.UpdateProductPrice(id, product);
                if (isSuccess)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return NotFound();
                }

                return BadRequest();
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            bool isDeleted;
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                isDeleted = await _productManager.DeleteProduct(id);
                if (!isDeleted)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
