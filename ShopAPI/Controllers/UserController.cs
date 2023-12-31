﻿using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Interfaces;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository,IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers() {
            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetUsers());
            return Ok(users);
        }
        [HttpGet("GetUser/{userId}")]
        public IActionResult GetUser(int userId)
        {
            var user = _mapper.Map<UserDTO>(_userRepository.GetUser(userId));
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
            user.Role = _roleRepository.GetRoles().First(r => r.Name == "User");

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
        public IActionResult UpdateUser([FromQuery] int roleId, [FromBody] UserDTO userDTO)
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
            user.Role = _roleRepository.GetRoles().First(r => r.Id == roleId);
            var userCheck = _userRepository.GetUsers().FirstOrDefault(u => u.Email == userDTO.Email && u.Id != userDTO.Id);
            if (userCheck != null)
            {
                ModelState.AddModelError("", "User with this email already exists");
                return StatusCode(432, ModelState);
            }
            if (!_userRepository.UpdateUser(user))
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
        [HttpPost("Login")]
        public IActionResult Login([FromBody]UserLogin userLogin) { 
            if (userLogin == null)
            {
                return BadRequest();
            }
            var user = _userRepository.GetUserByEmail(userLogin.Email);
            var userDto = _mapper.Map<UserDTO>(user);
            if (user == null) {
                return NotFound();
            }
            if (!(user.Email == userLogin.Email && user.Password == userLogin.Password))
            {
                ModelState.AddModelError("", "Invalid Email or Password");
                return StatusCode(411, ModelState);
            }
            return Ok(userDto);
        }
    }
}
