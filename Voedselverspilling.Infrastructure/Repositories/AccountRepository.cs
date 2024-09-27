using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppIdentity> _userManager;
        private readonly SignInManager<AppIdentity> _signInManager;

        public AccountRepository(UserManager<AppIdentity> userManager, SignInManager<AppIdentity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginRequest loginRequest)
        {
            // Step 1: Check if the user exists
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null)
            {
                Console.WriteLine("User not found");
                throw new UnauthorizedAccessException("Invalid login attempt");  // User not found
            }

            // Step 2: Check if the password is correct
            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid credentials");  // Incorrect password
            }

            return result; // Successful login
        }

    }
}
