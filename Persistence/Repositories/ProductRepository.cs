using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Aggregates;
using Domain.Contracts.Repositories;

namespace Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly UciDbContext _uciDbContext;

        public ProductRepository(UciDbContext uciDbContext)
        {
            _uciDbContext = uciDbContext;
        }

        public IEnumerable<Product> GetAll()
        {
            return _uciDbContext.Products;
        }

        public Product GetById(int id)
        {
            return _uciDbContext.Products.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            product.Id = new Random().Next(0, 1000);
            _uciDbContext.Products.Add(product);
        }

        public void Update(Product product)
        {
            var productFound = _uciDbContext.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productFound == null) return;

            productFound.Id = product.Id;
            productFound.Name = product.Name;
            productFound.Category = product.Category;
            productFound.Price = product.Price;
        }

        public void Delete(int id)
        {
            var productFound = _uciDbContext.Products.FirstOrDefault(p => p.Id == id);
            if (productFound == null) return;

            _uciDbContext.Products.Remove(productFound);
        }

        public void DeleteBulk(IEnumerable<int> idsList)
        {
            var productsToDelete = _uciDbContext.Products.Where(p => idsList.Contains(p.Id));
            _uciDbContext.Products = _uciDbContext.Products.Except(productsToDelete).ToList();
        }
    }
}
