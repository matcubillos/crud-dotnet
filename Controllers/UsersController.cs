using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Persistense;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController(AppDbContext context) : ControllerBase
    {
        // GET: UserController
        private readonly AppDbContext _context = context;

        //Uso de async await para todas los endpoints para evitar caídas y bloqueos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Role)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Role = new RoleDto
                        {
                            Id = u.Role.Id,
                            Name = u.Role.Name
                        }
                    }).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                //http codigo 500 para excepciones no controladas
                return StatusCode(500, new { message = "Unexpected error occurred while fetching users: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> CreateUser(CreateUserDto userDto)
        {
            try
            {
                var roleExists = await _context.Roles.AnyAsync(r => r.Id == userDto.RoleId);
                if (!roleExists)
                {
                    return BadRequest(new { message = "That role doesn't exist." });
                }

                var user = new User
                {
                    Name = userDto.Name,
                    Email = userDto.Email,
                    RoleId = userDto.RoleId,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "User was created successfully", id = user.Id });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at creating user: ", ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> UpdateUser(int id, CreateUserDto userDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                var roleExists = await _context.Roles.AnyAsync(r => r.Id == userDto.RoleId);
                if (!roleExists)
                {
                    return BadRequest(new { message = "Role specified doesnt exist!" });
                }
                user.Name = userDto.Name;
                user.Email = userDto.Email;
                user.RoleId = userDto.RoleId;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "User was updated successully", id = user.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at updating user: ", ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "User was deleted successfully", id = user.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at deleting user: ", ex.Message });
            }
        }
    }
}

