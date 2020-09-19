using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;

        private readonly IManageProductService _manageProductService;
        public ProductsController(IPublicProductService publicProductService,
            IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        //https://localhost:port/products?pageIndex=1&pageSize=10&categoryId=
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var product = await _publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(product);
        }

        //https://localhost:port/products/1
        [HttpGet("{produtctId}/{languageId}")]
        public async Task<IActionResult> GetById(int produtctId, string languageId)
        {
            var product = await _manageProductService.GetById(produtctId, languageId);
            if (product == null)
            {
                return BadRequest("Cannot find prodcut");
            }
            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(request);
            if (productId == 0)
            {
                return BadRequest();
            }
            var product = await _manageProductService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{produtctId}")]
        public async Task<IActionResult> Delete(int produtctId)
        {
            var affectedResult = await _manageProductService.Delete(produtctId);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }


        [HttpPatch("{produtctId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int produtctId, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(produtctId, newPrice);
            if (isSuccessful)
            {
                return Ok();
            }

            return BadRequest();
        }

        #region Images
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _manageProductService.AddImage(productId, request);
            if (imageId == 0)
            {
                return BadRequest();
            }
            var image = await _manageProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpGet("{produtctId}/images/{languageId}")]
        public async Task<IActionResult> GetImageById(int produtctId, int imageId)
        {
            var image = await _manageProductService.GetImageById(imageId);
            if (image == null)
            {
                return BadRequest("Cannot find prodcut");
            }
            return Ok(image);
        }


        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _manageProductService.UpdateImage(imageId, request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{produtctId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var affectedResult = await _manageProductService.RemoveImage(imageId);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }


        #endregion
    }
}
