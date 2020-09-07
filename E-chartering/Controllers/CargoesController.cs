using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Echartering.Data;
using Echartering.Models;

namespace Echartering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CargoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cargoes
        [HttpGet]
        public IEnumerable<Cargo> GetCargos()
        {
            var userid = HttpContext.User.Identity.Name;

            var result = _context.Cargos.Where(p => string.Equals(p.UserId, userid, StringComparison.OrdinalIgnoreCase));

            return result;
        }

        // GET: api/Cargoes
        [HttpGet("All")]
        public IEnumerable<Cargo> GetCargoes()
        {


            var result = _context.Cargos.Include(e=>e.ApplicationUser);

            return result;
        }

        // GET: api/Cargoes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCargo([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cargo = await _context.Cargos.FindAsync(id);

            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        // PUT: api/Cargoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCargo([FromRoute] long id, [FromBody] Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cargo.Id)
            {
                return BadRequest();
            }

            _context.Entry(cargo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CargoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cargoes
        [HttpPost]
        public async Task<IActionResult> PostCargo([FromBody] Cargo cargo)
        {
            var userid = HttpContext.User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (cargo != null && userid != null)
            {
                cargo.UserId = userid;
                _context.Cargos.Add(cargo);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCargo", new { id = cargo.Id }, cargo);
        }

        // DELETE: api/Cargoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargo([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }

            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();

            return Ok(cargo);
        }

        private bool CargoExists(long id)
        {
            return _context.Cargos.Any(e => e.Id == id);
        }
    }
}