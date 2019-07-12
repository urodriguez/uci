using System;

namespace Application.Dtos
{
    public class ProductDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}