using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.DTOs
{
    public record ProductDto
    (       int Id,
            string Name,
            decimal Price
    );
}
