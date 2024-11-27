using Ardalis.Specification;

namespace LunaEdge.TestAssignment.Application.Database.Repositories;

public interface IRepository<T> : IRepositoryBase<T>
  where T : class
{
}