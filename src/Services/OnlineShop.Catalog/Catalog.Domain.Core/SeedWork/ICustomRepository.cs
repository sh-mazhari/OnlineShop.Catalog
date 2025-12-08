namespace Catalog.Domain.Core.SeedWork;

public interface ICustomRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
