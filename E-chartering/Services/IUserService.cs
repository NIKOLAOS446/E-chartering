using Echartering.Data;
using Echartering.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echartering.Services
{
    public interface IUserService
    {
        ApplicationUser Authenticate(string email, string password);
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser GetById(string id);
        IdentityResult Create(ApplicationUser user, string password);

        void Update(ApplicationUser user, string currentPass = null, string password = null);
       // void Delete(string id);

    }
    public class ApplicationUserService : IUserService 
    {
        private ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

           

            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == email );

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            var signInResult = _signInManager.CheckPasswordSignInAsync(user, password, false).Result;
            if (!signInResult.Succeeded)
                return null;

            // authentication successful
            return user;
        }
        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.Find(id);
        }


        public IdentityResult Create(ApplicationUser user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new Exception("Username \"" + user.UserName + "\" is already taken");
            if (password.Length != 7)
                throw new Exception("Password must be 7 characters and have one digit at least");
            
            

            var result = _userManager.CreateAsync(user, password);
            return result.Result;


        }
        public void Update(ApplicationUser userParam, string currentPass = null, string password = null)
        {

            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            if (userParam.UserName != user.UserName)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.UserName == userParam.UserName))
                    throw new Exception("Username " + userParam.UserName + " is already taken");
            }

            // update user properties
            //user.FirstName = userParam.FirstName;
            //user.LastName = userParam.LastName;
            if (userParam!= null)
            {
                user.UserName = userParam.UserName;
                user.PhoneNumber = userParam.PhoneNumber;
                user.Email = userParam.Email;

            }

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(currentPass))
            {
                _userManager.ChangePasswordAsync(user, currentPass, password);
            }

            //_userManager.UpdateAsync(user);

        }
    }
}
