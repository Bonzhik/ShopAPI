using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Interfaces;
using ShopAPI.Models;
using ShopAPI.Repositories;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository,IUserRepository userRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            var categories = _mapper.Map<List<RoleDTO>>(_roleRepository.GetRoles());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }
        [HttpGet("GetRoleByUserId")]
        public IActionResult GetRoleByUserId([FromQuery]int userId) {
            if (userId == 0) { 
                return BadRequest();
            }
            var user = _userRepository.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            var role = user.Role.Name;
            return Ok(role);
        }
        [HttpPost("AddRole")]
        public IActionResult AddRole([FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = _mapper.Map<Role>(roleDTO);
            if (!_roleRepository.IsExists(role))
            {
                ModelState.AddModelError("", "Role is already exists");
                return StatusCode(422, ModelState);
            }
            if (!_roleRepository.AddRole(role))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
        [HttpPut("UpdateRole")]
        public IActionResult UpdateRole([FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = _mapper.Map<Role>(roleDTO);
            if (_roleRepository.GetRole(role.Id) == null)
            {
                ModelState.AddModelError("", "Role is not exists");
                return StatusCode(422, ModelState);
            }
            if (_roleRepository.GetRoles().Where(c => c.Id != roleDTO.Id).Where(c => c.Name == roleDTO.Name).Any())
            {
                ModelState.AddModelError("", "Role is already exists");
                return StatusCode(422, ModelState);
            }
            if (!_roleRepository.UpdateRole(role))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");

        }
        [HttpDelete("DeleteRole/{roleId}")]
        public IActionResult DeleteRole(int roleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = _roleRepository.GetRole(roleId);
            if (role == null)
            {
                return NotFound();
            }
            if (!_roleRepository.DeleteRole(role))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Success");
        }
    }
}
