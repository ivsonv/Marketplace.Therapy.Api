using Marketplace.Domain.Models.Request;
using Marketplace.Domain.Models.Request.Categories;
using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.Categories;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : DefaultController
    {
        private readonly CategoryService _categoryService;
        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<BaseRs<List<categoryRs>>> Show([FromQuery] BaseRq<categoryRq> _request)
            => await _categoryService.show(_request);

        [HttpGet("{id:int}")]
        public async Task<BaseRs<categoryRs>> FindById([FromRoute] int id)
            => await _categoryService.FindById(id);

        [HttpPost]
        public async Task<BaseRs<categoryRs>> Store([FromBody] BaseRq<categoryRq> _request)
            => await _categoryService.Store(_request);

        [HttpPut]
        public async Task<BaseRs<categoryRs>> Update([FromBody] BaseRq<categoryRq> _request)
            => await _categoryService.Update(_request);

        [HttpDelete("{id:int}")]
        public async Task<BaseRs<bool>> Delete([FromRoute] int id)
            => await _categoryService.Delete(id);
    }
}