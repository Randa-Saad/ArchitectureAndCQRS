using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Products
{
    public interface IProductRepository
    {

        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);

        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task AddAsync(Product product, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);

        void Delete(Product product);
    }
}
