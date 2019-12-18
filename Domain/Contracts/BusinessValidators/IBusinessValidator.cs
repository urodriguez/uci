using Domain.Contracts.Aggregates;

namespace Domain.Contracts.BusinessValidators
{
    public interface IBusinessValidator<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        void Validate(TAggregateRoot aggregate);
    }
}