using System;

namespace Application.Dtos
{
    public class InventionDto : ICrudDto
    {
        public Guid Id { get; set; }
        public string InventorName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public bool Enable { get; set; }
    }
}