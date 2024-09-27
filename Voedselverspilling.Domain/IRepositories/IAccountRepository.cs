using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Domain.IRepositories
{
    public interface IAccountRepository
    {
        Task<AppIdentity> LoginAsync(LoginRequest loginRequest);
    }
}
