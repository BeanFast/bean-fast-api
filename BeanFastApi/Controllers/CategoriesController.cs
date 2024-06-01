using BeanFastApi.Validators;
using BusinessObjects.Models;
using DataTransferObjects.Models.Category.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using System.Security.Claims;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Settings;

namespace BeanFastApi.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService, IUserService userService) : base(userService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            string? userRole = GetUserRole();
            var categories = await _categoryService.GetAll(userRole);
            return SuccessResult(categories);
        }

        [HttpGet(ApiEndpointConstants.Category.GetCategorybyId)]
        public async Task<IActionResult> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryService.GetById(id);
            return SuccessResult(category);
        }
        [HttpGet("topSellers")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> GetTopSellerCategories([FromQuery] int topCount)
        {
            var result = await _categoryService.GetTopSellerCategory(topCount, await GetManagerAsync());
            return SuccessResult(result);
        }


        [HttpPost]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> CreateCategoryAsync(CreateCategoryRequest category)
        {
            await _categoryService.CreateCategory(category, await GetUserAsync());
            return SuccessResult<object>(message: MessageConstants.CategoryMessageConstrant.CategoryCreateSucess, statusCode: System.Net.HttpStatusCode.Created);
        }
        [HttpPut("{id}")]
        [Authorize(RoleName.MANAGER)]

        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] Guid id, [FromBody] UpdateCategoryRequest category)
        {
            await _categoryService.UpdateCategoryAsync(id, category, await GetUserAsync());
            return SuccessResult<object>();
        }
        [HttpDelete("{id}")]
        [Authorize(RoleName.MANAGER)]

        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id, await GetUserAsync());
            return SuccessResult<object>();
        }
    }
}
