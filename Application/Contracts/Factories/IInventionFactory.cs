using Application.Dtos;
using Domain.Aggregates;

namespace Application.Contracts.Factories
{
  public interface IInventionFactory : IFactory<InventionDto, Invention>
  {
  }
}
