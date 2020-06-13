using System;

namespace Application.Dtos
{
    public class InventionTypeDto : ICrudDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}