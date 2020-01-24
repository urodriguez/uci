using System;
using Application.ApplicationResults;
using Application.Dtos;

namespace Application.Contracts.Services
{
    public interface ICrudService<TDto> where TDto : IDto
    {
        IApplicationResult GetAll();
        IApplicationResult GetById(Guid id);
        IApplicationResult Create(TDto dto);
        IApplicationResult Update(Guid id, TDto dto);
        IApplicationResult Delete(Guid id);
    }
}