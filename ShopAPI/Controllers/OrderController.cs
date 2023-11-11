using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.DTO;
using ShopAPI.Interfaces;
using ShopAPI.Models;

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

        public OrderController(IOrderRepository orderRepository, IMapper mapper, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders() {
            var orders = _mapper.Map<List<OrderDTO>>(_orderRepository.GetOrders());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(orders);
        }

        [HttpPost("AddOrder")]
        public IActionResult AddOrder([FromBody] int[][] productId, [FromQuery] OrderDTO orderDTO, int userId)
        {
            if (productId == null || orderDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
