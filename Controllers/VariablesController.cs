using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VariablesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        // GET: VariablesController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VariableDto>>> GetVariables()
        {
            try
            {
                var variables = await _context.Variables
                    .Select( u => new VariableDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Value = u.Value,
                        Type = u.Type.ToString()
                    }).ToListAsync();
                return Ok(variables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at getting variables: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Variables>>> CreateVariable(CreateVariableDto createVariableDto)
        {
            try
            {
                var variable = new Variables
                {
                    Name = createVariableDto.Name,
                    Value = createVariableDto.Value,
                    Type = createVariableDto.Type
                };
                _context.Variables.Add(variable);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Variable created successfully", id = variable.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at creating variable: " + ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Variables>>> UpdateVariable(int id, UpdateVariableDto updateVariableDto)
        {
            try
            {
                var variable = await _context.Variables.FindAsync(id);
                if (variable == null)
                {
                    return NotFound(new { message = "Variable not found" });
                }
                variable.Name = updateVariableDto.Name;
                variable.Value = updateVariableDto.Value;
                variable.Type = updateVariableDto.Type;
                _context.Variables.Update(variable);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Variable updated successfully", id = variable.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at updating variable: " + ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Variables>>> DeleteVariable(int id)
        {
            try
            {
                var variable = await _context.Variables.FindAsync(id);
                if (variable == null)
                {
                    return NotFound(new { message = "Variable not found" });
                }
                _context.Variables.Remove(variable);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Variable deleted successfully", id = variable.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error at deleting variable: " + ex.Message });
            }
        }

    }
}
