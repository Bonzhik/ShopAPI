using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.DTO;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using ShopAPI.Repositories;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public OrderController(IOrderRepository orderRepository, IMapper mapper, IUserRepository userRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders() {
            var orders = _mapper.Map<List<OrderDTO>>(_orderRepository.GetOrders());
            return Ok(orders);
        }

        [HttpPost("AddOrder")]
        public IActionResult AddOrder([FromBody] ProductHelper productHelper, [FromQuery] OrderDTO orderDTO, int userId)
        {
            var productId = productHelper.ProductId;
            if (productId == null || orderDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            for (int i = 0; i < productId.Length; i++)
            {
                var currentproduct = _productRepository.GetProduct(productId[i][0]);
                if (productId[i][1] > currentproduct.Quantity)
                {
                    ModelState.AddModelError("", "Not enough items");
                    return StatusCode(454, ModelState);
                }

            }
            var order = _mapper.Map<Order>(orderDTO);
            order.User = _userRepository.GetUser(userId);
            if (!_orderRepository.AddOrder(productId, order))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");

        }
        [HttpPut("UpdateOrder")]

        public IActionResult UpdateOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = _mapper.Map<Order>(orderDTO);
            if (!_orderRepository.UpdateOrder(order))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }

        [HttpDelete("DeleteOrder")]

        public IActionResult DeleteOrder([FromQuery] int orderId)
        {
            if (orderId == 0) {
                return BadRequest(ModelState);
            }
            var order = _orderRepository.GetOrders().FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }
            if (!_orderRepository.DeleteOrder(order))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpGet("GetOrdersByUser")]
        public IActionResult GetOrdersByUser([FromQuery]int userId) {
            if (userId == 0)
            {
                return BadRequest();
            }
            var orders = _mapper.Map<List<OrderDTO>>(_orderRepository.GetOrders(userId));
            return Ok(orders);

        }
        
    }
}
