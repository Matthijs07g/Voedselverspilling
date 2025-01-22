using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.DomainServices.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context.Producten.FindAsync(id);
            return product ?? throw new Exception("Product not found");
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Producten.ToListAsync();
            return products ?? throw new Exception("No products found");
        }

        public async Task<Product> AddAsync(Product Product)
        {
            _context.Producten.Add(Product);
            await _context.SaveChangesAsync();
            return Product;
        }

        public async Task<Product> UpdateAsync(Product Product)
        {
            var productOld = await _context.Producten.FindAsync(Product.Id);
            if(productOld == null)
            {
                throw new Exception("Product not found");
            }
            _context.Entry(Product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Product;
        }

        public async Task DeleteAsync(int id)
        {
            var Product = await _context.Producten.FindAsync(id);
            if (Product == null)
            {
                throw new Exception("Product not found");
            }

            _context.Producten.Remove(Product);
            await _context.SaveChangesAsync();
        }
    }
}
