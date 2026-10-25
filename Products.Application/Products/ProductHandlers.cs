using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IProductRepository repository,
        ILogger<CreateProductHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating product with name {ProductName}",
            request.Name);

        try
        {
            var product = new Product(
                request.Name,
                request.Price);

            await _repository.AddAsync(
                product, cancellationToken);

            await _repository.SaveChangesAsync(
                cancellationToken);

            _logger.LogInformation(
                "Product created successfully with ID {ProductId}",
                product.Id);

            return product.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create product with name {ProductName}",
                request.Name);

            throw;
        }
    }
}



// Read all

public class GetAllProductsHandler
    : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<GetAllProductsHandler> _logger;

    public GetAllProductsHandler(
        IProductRepository repository,
        ILogger<GetAllProductsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<ProductDto>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all products");

        try
        {
            var products = await _repository.GetAllAsync(
                cancellationToken);

            var result = products
                .Select(p => new ProductDto(
                    p.Id, p.Name, p.Price))
                .ToList();

            _logger.LogInformation(
                "Retrieved {ProductCount} products",
                result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to retrieve all products");

            throw;
        }
    }
}



// Read by ID

public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(
        IProductRepository repository,
        ILogger<GetProductByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting product with ID {ProductId}",
            request.Id);

        try
        {
            var p = await _repository.GetByIdAsync(
                request.Id, cancellationToken);

            if (p is null)
            {
                _logger.LogWarning(
                    "Product with ID {ProductId} was not found",
                    request.Id);

                return null;
            }

            _logger.LogInformation(
                "Product with ID {ProductId} retrieved successfully",
                request.Id);

            return new ProductDto(p.Id, p.Name, p.Price);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to retrieve product with ID {ProductId}",
                request.Id);

            throw;
        }
    }
}



// Update

public class UpdateProductHandler
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(
        IProductRepository repository,
        ILogger<UpdateProductHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Updating product with ID {ProductId}",
            request.Id);

        try
        {
            var product = await _repository.GetByIdAsync(
                request.Id, cancellationToken);

            if (product is null)
            {
                _logger.LogWarning(
                    "Cannot update product. ID {ProductId} was not found",
                    request.Id);

                return false;
            }

            product.Update(request.Name, request.Price);

            await _repository.SaveChangesAsync(
                cancellationToken);

            _logger.LogInformation(
                "Product with ID {ProductId} updated successfully",
                request.Id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update product with ID {ProductId}",
                request.Id);

            throw;
        }
    }
}



// Delete

public class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(
        IProductRepository repository,
        ILogger<DeleteProductHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Deleting product with ID {ProductId}",
            request.Id);

        try
        {
            var product = await _repository.GetByIdAsync(
                request.Id, cancellationToken);

            if (product is null)
            {
                _logger.LogWarning(
                    "Cannot delete product. ID {ProductId} was not found",
                    request.Id);

                return false;
            }

            _repository.Delete(product);

            await _repository.SaveChangesAsync(
                cancellationToken);

            _logger.LogInformation(
                "Product with ID {ProductId} deleted successfully",
                request.Id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to delete product with ID {ProductId}",
                request.Id);

            throw;
        }
    }
}


