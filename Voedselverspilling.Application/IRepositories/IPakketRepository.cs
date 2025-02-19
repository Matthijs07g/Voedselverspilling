﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.DomainServices.IRepositories
{
    public interface IPakketRepository
    {
        Task<Pakket> GetByIdAsync(int id);
        Task<IEnumerable<Pakket>> GetAllAsync();
        Task<Pakket> AddAsync(Pakket pakket);
        Task<Pakket> UpdateAsync(Pakket pakket);
        Task DeleteAsync(int id);
        Task<IEnumerable<Pakket>> GetByEmailAsync(string Email);

        Task<Pakket> ReservePakketAsync(int id, Student student);
        Task<IEnumerable<Pakket>> GetByKantine(int id);
    }
}
