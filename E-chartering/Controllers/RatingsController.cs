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
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: api/Ratings
        [HttpGet]
        public IEnumerable<object> GetRatings()
        {


            var rating = (from p in _context.Ratings
                          group p by p.ApplicationUser.Email into c

                          select new
                          {
                              user = c.Key,
                              TotalScore = c.Average(p => p.Score),




                          }).AsQueryable();

            var orderrating = rating.OrderByDescending(p => p.TotalScore).Take(5).AsEnumerable();


            return orderrating;

        }

        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRating([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rating = await _context.Ratings.FindAsync(id);

            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }

        [HttpGet("GetRatingss")]
        public async Task<ActionResult<object>> GetRatingss()
        {

            var userid = HttpContext.User.Identity.Name;
            var userr = await _context.ApplicationUsers.FindAsync(userid);
            var userRoles = await _userManager.GetRolesAsync(userr);
            var role = userRoles[0];

            if (role == "Charterer" || role == "Admin")
            {
                return 0;
            }


            var result = _context.Ratings.Where(p => string.Equals(p.ShipOwnerId, userid, StringComparison.OrdinalIgnoreCase)).Average(e => e.Score);


            return result;


        }
        [HttpGet("GetRatingsss")]
        public async Task<IEnumerable<object>> GetRatingsss()
        {

            var userid = HttpContext.User.Identity.Name;
            var userr = await _context.ApplicationUsers.FindAsync(userid);
            var userRoles = await _userManager.GetRolesAsync(userr);
            var role = userRoles[0];

            if (role == "Charterer")
            {
                return null;
            }

            var userRatings = _context.Ratings.Where(p => string.Equals(p.ShipOwnerId, userid, StringComparison.OrdinalIgnoreCase));


            var result = (from p in userRatings
                         select new
                         {
                             positive = userRatings.Count(l => l.Description == "Positive"),
                             neutral = userRatings.Count(c => c.Description == "Neutral"),
                             negative = userRatings.Count(k => k.Description == "Neagtive")


                         }).AsEnumerable();



            return result;


        }
        [HttpGet("GetCountRatingsss")]
        public IEnumerable<object> GetCountRatingsss()
        {





            var result = from p in _context.Ratings
                         group p by p.ApplicationUser.Email into c


                         select new
                         {
                             User = c.Key,
                             TotalScore = c.Average(p => p.Score),
                             Positive = c.Count(p => p.Description == "Positive"),
                             Neutral = c.Count(p => p.Description == "Neutral"),
                             Negative = c.Count(p => p.Description == "Negative")

                         }
                     ;
            var results = result.OrderByDescending(p => p.TotalScore).Take(5).AsEnumerable();

            return results;


        }
        // PUT: api/Ratings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating([FromRoute] long id, [FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rating.Id)
            {
                return BadRequest();
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
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

        // POST: api/Ratings
        [HttpPost]
        public async Task<IActionResult> PostRating([FromBody] Rating rating)
        {
            if (rating.Score == 5)
            {
                rating.Description = "Positive";
            }
            else if (rating.Score == 3)
            {
                rating.Description = "Neutral";
            }
            else
            {
                rating.Description = "Negative";
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return Ok(rating);
        }

        private bool RatingExists(long id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }
    }
}