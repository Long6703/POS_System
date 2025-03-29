using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Shared.DTOs;
using POS.Shared;
using POS_API.Services.IServices;

namespace POS_API.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<UserDTO>>> GetUsers([FromQuery] UserSearchDto searchDto)
        {
            var users = await _userService.GetUsersAsync(searchDto);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.CreateUserAsync(createUserDto);
            if (createdUser == null)
            {
                return BadRequest(new { message = "This email is already registered. Please use another email." });
            }

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Delete Done");
        }

        [HttpPut("roles")]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesDto updateUserRolesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserRolesAsync(updateUserRolesDto);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Update Done");
        }
    }
}