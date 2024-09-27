using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.DomainServices.IServices
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(LoginRequest loginRequest);
    }
}
