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
    public class RolesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        //Uso de async await para todas los endpoints para evitar caídas y bloqueos.
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Role>>> CreateRole(CreateRoleDto roleDto)
        {
            try
            {

                var role = new Role
                {
                    Name = roleDto.Name,
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Role created", id = role.Id });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at creating user: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            try
            {
                var roles = await _context.Roles
                    .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at getting roles: " + ex.Message });
            }

        }
    
    [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Role>>> UpdateRole(int id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    return NotFound(new { message = "Role not found" });
                }
                role.Name = updateRoleDto.Name;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Role updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at updating role: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Role>>> DeleteRole(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    return NotFound(new { message = "Role not found" });
                }
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Role deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at deleting role: " + ex.Message });
            }
        }
    }
}