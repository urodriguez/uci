using System;

namespace Application.Dtos
{
    public interface ICrudDto : IDto
    {
        Guid Id { get; set; }
    }
}