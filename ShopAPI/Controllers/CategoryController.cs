using AutoMapper;
using ShopAPI.DTO;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository,IProductRepository productRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }
        [HttpPost("AddCategory")]
        public IActionResult AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(categoryDTO);
            if (_categoryRepository.IsExists(category))
            {
                ModelState.AddModelError("", "Category is already exists");
                return StatusCode(422, ModelState);
            }
            if (!_categoryRepository.AddCategory(category))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpPut("UpdateCategory")]
        public IActionResult UpdateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(categoryDTO);
            if (_categoryRepository.GetCategory(category.Id) == null)
            {
                ModelState.AddModelError("", "Category is not exists");
                return StatusCode(422, ModelState);
            }
            if (_categoryRepository.CategoryNameExists(categoryDTO.Id , categoryDTO.Name))
            {
                ModelState.AddModelError("", "Category is already exists");
                return StatusCode(422, ModelState);
            }
            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");

        }
        [HttpDelete("DeleteCategory/{catId}")]
        public IActionResult DeleteCategory(int catId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.GetCategory(catId);
            if (category == null)
            {
                return NotFound();
            }
            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpGet("GetCategoriesByProduct/{prId}")]

        public IActionResult GetCategoriesByProduct(int prId)
        {
            if (prId == null)
            {
                return BadRequest();
            }
            var product = _productRepository.GetProduct(prId);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategoriesByProduct(prId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }
    }
}
