using MediatR;
using Products.Application.DTOs;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Products;

    // Commands
    public record CreateProductCommand(
        string Name,
        decimal Price) : IRequest<int>;

    public record UpdateProductCommand(
        int Id,
        string Name,
        decimal Price) : IRequest<bool>;

    public record DeleteProductCommand(
        int Id) : IRequest<bool>;

    // Queries
    public record GetAllProductsQuery()
        : IRequest<List<ProductDto>>;

    public record GetProductByIdQuery(int Id)
        : IRequest<ProductDto?>;


    // Create
    public class CreateProductHandler
        : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _repository;

        public CreateProductHandler(
            IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product(
                request.Name,
                request.Price);

            await _repository.AddAsync(
                product, cancellationToken);

            await _repository.SaveChangesAsync(
                cancellationToken);

            return product.Id;
        }
    }


    // Read all
    public class GetAllProductsHandler
        : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsHandler(
            IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDto>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllAsync(
                cancellationToken);

            return products.Select(p => new ProductDto(
                p.Id, p.Name, p.Price)).ToList();
        }
    }


    // Read by ID
    public class GetProductByIdHandler
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdHandler(
            IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var p = await _repository.GetByIdAsync(
                request.Id, cancellationToken);

            return p is null
                ? null
                : new ProductDto(p.Id, p.Name, p.Price);
        }
    }


    // Update
    public class UpdateProductHandler
        : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;

        public UpdateProductHandler(
            IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(
                request.Id, cancellationToken);

            if (product is null)
                return false;

            product.Update(request.Name, request.Price);

            await _repository.SaveChangesAsync(
                cancellationToken);

            return true;
        }
    }


    // Delete
    public class DeleteProductHandler
        : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;

        public DeleteProductHandler(
            IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(
                request.Id, cancellationToken);

            if (product is null)
                return false;

            _repository.Delete(product);

            await _repository.SaveChangesAsync(
                cancellationToken);

            return true;
        }
    }

