using System;
using System.Collections.Generic;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface ICrudService<TDto> where TDto : IDto
    {
        IEnumerable<TDto> GetAll();
        TDto GetById(Guid id);
        Guid Create(TDto dto);
        void Update(Guid id, TDto dto);
        void Delete(Guid id);
    }
}