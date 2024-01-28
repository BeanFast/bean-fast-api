using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

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
    }
}
