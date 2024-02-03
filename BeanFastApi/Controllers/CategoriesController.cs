using DataTransferObjects.Models.Category.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Constants;

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
            var categories = await _categoryService.GetAll();
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
            return SuccessResult(message: MessageContants.Category.CategoryCreateSucess, statusCode: System.Net.HttpStatusCode.Created);
        }
    }
}
