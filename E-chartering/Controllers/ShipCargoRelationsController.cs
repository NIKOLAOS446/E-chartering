using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Echartering.Data;
using Echartering.Models;
using Microsoft.AspNetCore.Identity;

namespace Echartering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipCargoRelationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager <ApplicationUser> _userManager;

        public ShipCargoRelationsController(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
      
        // GET: api/ShipCargoRelations
        [HttpGet]
        [Route("GetShipCargoRelationsByShipOwner")]
        public IEnumerable<ShipCargoRelation> GetShipCargoRelationsByShipOwner()
        {
            var userid = HttpContext.User.Identity.Name;
            var result= _context.ShipCargoRelations.Where(p => string.Equals(p.ShipUserId, userid, StringComparison.OrdinalIgnoreCase)
            && p.IsApproved==false && p.AcceptorRole== "ShipOwner").Include(e=>e.Cargo.ApplicationUser).Include(p=>p.Cargo).Include(l=>l.Ship);
            
            return result;
        }

        [HttpGet]
        [Route("GetApprovedShipCargoRelationsByShipOwner")]
        public IEnumerable<ShipCargoRelation> GetApprovedShipCargoRelationsByShipOwner()
        {
            var userid = HttpContext.User.Identity.Name;
           
          
            var result = _context.ShipCargoRelations.Where(p => string.Equals(p.ShipUserId, userid, StringComparison.OrdinalIgnoreCase)
             && p.IsApproved == true ).Include(e => e.Cargo.ApplicationUser).Include(p => p.Cargo).Include(l => l.Ship);
             
            

            return result;
        }
        // GET: api/ShipCargoRelations
        [HttpGet]
        [Route("GetShipCargoRelationsByCharterer")]
        public IEnumerable<ShipCargoRelation> GetShipCargoRelationsByCharterer()
        {
            var userid = HttpContext.User.Identity.Name;
            var result = _context.ShipCargoRelations.Where(p => string.Equals(p.CargoUserId, userid, StringComparison.OrdinalIgnoreCase)
             && p.IsApproved == false && p.AcceptorRole == "Charterer").Include(e => e.Ship.ApplicationUser).Include(p => p.Cargo).Include(l => l.Ship);

            return result;
        }

        [HttpGet]
        [Route("GetApprovedShipCargoRelationsByCharterer")]
        public IEnumerable<ShipCargoRelation> GetApprovedShipCargoRelationsByCharterer()
        {
            var userid = HttpContext.User.Identity.Name;


            var result = _context.ShipCargoRelations.Where(p => string.Equals(p.CargoUserId, userid, StringComparison.OrdinalIgnoreCase)
             && p.IsApproved == true ).Include(e => e.Ship.ApplicationUser).Include(p => p.Cargo).Include(l => l.Ship);



            return result;
        }
        // GET: api/ShipCargoRelations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipCargoRelation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shipCargoRelation = await _context.ShipCargoRelations.FindAsync(id);

            if (shipCargoRelation == null)
            {
                return NotFound();
            }

            return Ok(shipCargoRelation);
        }

        // PUT: api/ShipCargoRelations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipCargoRelation([FromRoute] long id, [FromBody] ShipCargoRelation shipCargoRelation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shipCargoRelation.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipCargoRelation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipCargoRelationExists(id))
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

        // POST: api/ShipCargoRelations
        [HttpPost]
        [Route("ChartererPostShipCargoRelation")]
        public async Task<IActionResult> ChartererPostShipCargoRelation([FromBody] ShipCargoRelation shipCargoRelation)
        {
            var userid = HttpContext.User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (shipCargoRelation != null && userid != null && shipCargoRelation.FixedFreight != 0 && shipCargoRelation.CargoId != 0 && shipCargoRelation.Commission !=0)
            {
                shipCargoRelation.DateCreated = System.DateTime.Now;
                shipCargoRelation.CargoUserId = userid;
                shipCargoRelation.IsApproved = false;
                shipCargoRelation.AcceptorRole = "ShipOwner";
                _context.ShipCargoRelations.Add(shipCargoRelation);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Please Fill all required Fields");
            }
           

            return CreatedAtAction("GetShipCargoRelation", new { id = shipCargoRelation.Id }, shipCargoRelation);
        }

        // POST: api/ShipCargoRelations
        [HttpPost]
        [Route("ShipownerPostShipCargoRelation")]
        public async Task<IActionResult> ShipownerPostShipCargoRelation([FromBody] ShipCargoRelation shipCargoRelation)
        {
            var userid = HttpContext.User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (shipCargoRelation != null && userid != null && shipCargoRelation.FixedFreight != 0 && shipCargoRelation.ShipId != 0 && shipCargoRelation.Commission != 0)
            {
                shipCargoRelation.DateCreated = System.DateTime.Now;
                shipCargoRelation.ShipUserId = userid;
                shipCargoRelation.AcceptorRole = "Charterer";
                shipCargoRelation.IsApproved1= false;
                _context.ShipCargoRelations.Add(shipCargoRelation);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Please Fill all required Fields");
            }


            return CreatedAtAction("GetShipCargoRelation", new { id = shipCargoRelation.Id }, shipCargoRelation);
        }

        // DELETE: api/ShipCargoRelations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipCargoRelation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shipCargoRelation = await _context.ShipCargoRelations.FindAsync(id);
            if (shipCargoRelation == null)
            {
                return NotFound();
            }

            _context.ShipCargoRelations.Remove(shipCargoRelation);
            await _context.SaveChangesAsync();

            return Ok(shipCargoRelation);
        }

        private bool ShipCargoRelationExists(long id)
        {
            return _context.ShipCargoRelations.Any(e => e.Id == id);
        }
    }
}