using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Interfaces;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers() {
            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetUsers());
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            return Ok(users);
        }
        [HttpGet("GetUser/{userId}")]
        public IActionResult GetUser(int userId)
        {
            var user = _mapper.Map<UserDTO>(_userRepository.GetUser(userId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<User>(userDTO);

            var userCheck = _userRepository.GetUsers().FirstOrDefault(u => u.Email == userDTO.Email);

            if (userCheck != null)
            {
                ModelState.AddModelError("", "User with this email already exists");
                return StatusCode(432, ModelState);
            }
            if (!_userRepository.AddUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromQuery] int[] roleId, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<User>(userDTO);
            var userCheck = _userRepository.GetUsers().FirstOrDefault(u => u.Email == userDTO.Email && u.Id != userDTO.Id);
            if (userCheck != null)
            {
                ModelState.AddModelError("", "User with this email already exists");
                return StatusCode(432, ModelState);
            }
            if (!_userRepository.UpdateUser(roleId, user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userCheck = _userRepository.GetUsers().FirstOrDefault(u => u.Id == userId);
            if (userCheck == null) { 
                return NotFound();
            }
            if (!_userRepository.DeleteUser(userCheck))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
