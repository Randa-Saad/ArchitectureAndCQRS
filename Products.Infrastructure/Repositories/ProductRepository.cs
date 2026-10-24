using Microsoft.EntityFrameworkCore;
using Products.Application.Products;
using Products.Domain.Entities;
using Products.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public Task<List<Product>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return _context.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<Product?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return _context.Products
                .FirstOrDefaultAsync(
                    p => p.Id == id, cancellationToken);
        }

        public async Task AddAsync(
            Product product,
            CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(
                product, cancellationToken);
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}
