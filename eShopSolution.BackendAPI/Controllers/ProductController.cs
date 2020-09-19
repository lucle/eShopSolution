using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;

        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService publicProductService, 
            IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId)
        {
            var product = await _publicProductService.GetAll(languageId);
            return Ok(product);
        }


        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> Get([FromQuery] GetPublicProductPagingRequest request)
        {
            var product = await _publicProductService.GetAllByCategoryId(request);
            return Ok(product);
        }

        [HttpGet("{Id}/{languageId}")]
        public async Task<IActionResult> GetById(int Id, string languageId)
        {
            var product = await _manageProductService.GetById(Id, languageId);
            if (product == null)
            {
                return BadRequest("Cannot find prodcut");
            }
            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var productId = await _manageProductService.Create(request);
            if(productId == 0){
                return BadRequest();
            }
            var product = await _manageProductService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new {  id = productId}, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _manageProductService.Delete(id);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }


        [HttpPut("price/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(id, newPrice);
            if (isSuccessful)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
