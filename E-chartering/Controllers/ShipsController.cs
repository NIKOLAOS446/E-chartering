using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Echartering.Data;
using Echartering.Models;
using Microsoft.AspNetCore.Authorization;


namespace Echartering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShipsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ships
        [HttpGet]
        public IEnumerable<Ship> GetUserShips()
        {
            var userid = HttpContext.User.Identity.Name;

            var result = _context.Ships.Where(p => string.Equals(p.UserId, userid, StringComparison.OrdinalIgnoreCase));

            return result;


        }
        // GET: api/Ships
        [HttpGet("All")]       
        public IEnumerable<Ship> GetShips()
        {


            var result = _context.Ships.Include(e => e.ApplicationUser); 

            return result;


        }

        // GET: api/Ships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ship>> GetShips(int id)
        {
            var ship = await _context.Ships.FindAsync(id);

            ship.Date.ToString();

            if (ship == null)
            {
                return NotFound();
            }

            return ship;
        }

        // PUT: api/Ships/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShip([FromRoute] long id, [FromBody] Ship ship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ship.Id)
            {
                return BadRequest();
            }

            _context.Entry(ship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipExists(id))
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

        // POST: api/Ships
        [HttpPost]
        public async Task<IActionResult> PostShip([FromBody] Ship ship)
        {
            var userid = HttpContext.User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (ship != null && userid!=null)
            {
                ship.UserId = userid;
                _context.Ships.Add(ship);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetShips", new { id = ship.Id }, ship);
        }

        // DELETE: api/Ships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShip([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ship = await _context.Ships.FindAsync(id);
            if (ship == null)
            {
                return NotFound();
            }

            _context.Ships.Remove(ship);
            await _context.SaveChangesAsync();

            return Ok(ship);
        }
     

        private bool ShipExists(long id)
        {
            return _context.Ships.Any(e => e.Id == id);
        }
    }
}