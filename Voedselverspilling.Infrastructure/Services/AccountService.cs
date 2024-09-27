using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.IServices;

namespace Voedselverspilling.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<SignInResult> LoginAsync(LoginRequest loginRequest)
        {
            return await _accountRepository.LoginAsync(loginRequest);
        }
    }
}
