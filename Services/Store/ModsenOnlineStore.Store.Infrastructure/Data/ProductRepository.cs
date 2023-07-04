﻿using Microsoft.EntityFrameworkCore;
using ModsenOnlineStore.Store.Application.Interfaces.ProductInterfaces;
using ModsenOnlineStore.Store.Domain.Entities;

namespace ModsenOnlineStore.Store.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var products = await context.Products.AsNoTracking().ToListAsync();

            if (pageNumber < 1)
            {
                return products;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            return products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProductAsync(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task RemoveProductByIdAsync(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is not null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAllProductsByProductTypeIdAsync(int id, int pageNumber, int pageSize)
        {
            var products = await context.Products.AsNoTracking().Where(p => p.ProductTypeId == id).ToListAsync();

            if (pageNumber < 1)
            {
                return products;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            return products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
