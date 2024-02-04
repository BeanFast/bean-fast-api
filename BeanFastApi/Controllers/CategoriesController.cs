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

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            string? userRole = GetUserRole();
            var categories = await _categoryService.GetAll(userRole);
            return SuccessResult(categories);
        }

        [HttpGet(ApiEndpointConstants.Category.GetCategorybyId)]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetById(id);
            return SuccessResult(category);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest category)
        {
            await _categoryService.CreateCategory(category);
            return SuccessResult<object>(message: MessageConstants.Category.CategoryCreateSucess, statusCode: System.Net.HttpStatusCode.Created);
        }
    }
}
