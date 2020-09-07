using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Echartering.Data;
using Echartering.Helpers;
using Echartering.Models;
using Echartering.Models.ApplicationUserModels;
using Echartering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Echartering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _appSettings;

        public ApplicationUserController(IUserService userService, UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _context = context;

        }

        // GET: api/ApplicationUser


        [HttpPost]
        [AllowAnonymous]
        [HttpPost("Register")]
        //POST : /api/ApplicationUser/Register
        public async Task<ActionResult<RegisterModel>> Register(RegisterModel registerModel)
        {
            // registerModel.Role = "Shiponwer";
            if (registerModel != null && registerModel.Email != null && registerModel.Username != null && registerModel.Role != null &&registerModel.Password!=null)
            {

                var applicationUser = new ApplicationUser
                {

                    UserName = registerModel.Username,
                    Email = registerModel.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = registerModel.phoneNumber


                };
                if (applicationUser != null)
                {


                    try
                    {
                        var result = _userService.Create(applicationUser, registerModel.Password);

                        await _userManager.AddToRoleAsync(applicationUser, registerModel.Role);

                        return Ok(result);
                    }
                    catch (Exception ex)
                    {

                        
                        return BadRequest(ex.Message );



                    }
                }
                else
                {
                    return BadRequest("Wrong Credentials");
                }
            }
            else
            {
                return BadRequest();
            }
            
        }
        // POST: api/ApplicationUser
        [HttpPost]
        [Route("PostUser")]
        public async Task<IActionResult> PostUser([FromBody] ApplicationUser user)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user != null )
            {
               
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("Getuser", new { id = user.Id }, user);
        }
        // PUT:api/ApplicationUser
        [HttpPut("{id}")]   
        public async Task<IActionResult> PutUser([FromRoute] string id, [FromBody] ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpPut]
        [Route("UpdateUser{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateModel updateModel)
        {
            var user = new ApplicationUser
            {
                Id = updateModel.id,
                Email = updateModel.email,
                UserName = updateModel.userName,
                PhoneNumber = updateModel.phoneNumber
            };

            try
            {
                // save 
                _userService.Update(user, updateModel.CurrentPass, updateModel.password);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            return NoContent();
        }

        [HttpPost]
        [Route("Authenticate")]
        //POST : /api/ApplicationUser/Authenticate
        public async Task<ActionResult> Authenticate(LoginModel loginModel)
        {
            var user = _userService.Authenticate(loginModel.Email, loginModel.Password);

            if (user == null)
                return BadRequest( "Username or password is incorrect" );

            if (user.IsApproved==false )
                return BadRequest("Wait to be Approved By Admin");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles[0];

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                EMail = user.Email,
                Token = tokenString,
                role =role 

            });
        }
        [Authorize]
        [HttpGet("GetUser")]
        public async Task<ActionResult> GetUser()
        {
            var userid = HttpContext.User.Identity.Name;
            var userr = await _context.ApplicationUsers.FindAsync(userid);
            var userRoles = await _userManager.GetRolesAsync(userr);
            var role = userRoles[0];

            return Ok(new {
              
                Email=userr.Email,
                UserName= userr.UserName,
                role
            } );

        
        }
        // GET: api/ApplicationUser
        [HttpGet("GetApprovedUsers")]
        public IQueryable<object> GetApprovedUsers()
        {


            var result = _context.Users.Where(p => p.IsApproved == true);
            
            var user = (from u in _context.Users
                        join p in _context.UserRoles on u.Id equals p.UserId
                        join c in _context.Roles on p.RoleId equals c.Id
                        where (u.IsApproved==true)

                        select  new {
                            Id = u.Id,
                        Email =  u.Email,
                        PhoneNumber = u.PhoneNumber,
                        UserName = u.UserName,
                        PasswordHash = u.PasswordHash,
                        RoleId = c.Id,
                        Role = c.Name                        
                         }
                        ).AsQueryable();


            return user;


        }

        // GET: api/ApplicationUser
        [HttpGet("GetNotApprovedUsers")]
        public IEnumerable<ApplicationUser> GetNotApprovedUsers()  
        {


            var result = _context.Users.Where(p => p.IsApproved == false);

            return result;


        }
        // DELETE: api/ApplicationUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(user);
        }

        [Authorize]
        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
            return Ok();
        }

        //// PUT: api/ApplicationUser/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
