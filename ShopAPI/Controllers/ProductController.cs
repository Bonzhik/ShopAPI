﻿using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using ShopAPI.Repositories;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts() {
            var products = _mapper.Map<List<ProductDTO>>(_productRepository.GetProducts());
            return Ok(products);
        }
        [HttpGet("GetProducts/{id}")]
        public IActionResult GetProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var product = _mapper.Map<ProductDTO>(_productRepository.GetProduct(id));
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);

        }
        [HttpGet("GetProductsByCategory/{id}")]
        public IActionResult GetProductsByCategory(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var category = _categoryRepository.GetCategory(id);
            if (category == null) {
                return NotFound();
            }
            var products = _mapper.Map<List<ProductDTO>>(_productRepository.GetProductsByCategory(category));
            return Ok(products);
        }
        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromQuery] int[] catId, [FromBody] ProductDTO productDTO)
        {
            if (catId == null || productDTO == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _mapper.Map<Product>(productDTO);
            var checkProduct = _productRepository.GetProducts().Any(p=>p.Name == productDTO.Name);
            if (checkProduct)
            {
                ModelState.AddModelError("", "Product is already exists");
                return StatusCode(422, ModelState);
            }
            if (!_productRepository.AddProduct(catId, product))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct([FromQuery] int[] catId, [FromBody] ProductDTO productDTO)
        {
            if (catId == null || productDTO == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _mapper.Map<Product>(productDTO);
            if (!_productRepository.GetProducts().Any(p => p.Id == product.Id))
            {
                return NotFound();
            }
            if (_productRepository.GetProducts().Any(p => p.Name == product.Name && p.Id != product.Id))
            {
                ModelState.AddModelError(" ", "This product already exists");
                return StatusCode(422, ModelState);
            }
            if (!_productRepository.UpdateProduct(catId, product))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            if (!_productRepository.DeleteProduct(product))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");

        }
    }
}
