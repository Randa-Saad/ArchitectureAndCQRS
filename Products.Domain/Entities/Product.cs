using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        private Product() { } // For EF Core

        public Product(string name, decimal price)
        {
            SetName(name);
            SetPrice(price);
        }

        public void Update(string name, decimal price)
        {
            SetName(name);
            SetPrice(price);
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            Name = name.Trim();
        }

        private void SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException(
                    "Price cannot be negative.");

            Price = price;
        }
    }
}
