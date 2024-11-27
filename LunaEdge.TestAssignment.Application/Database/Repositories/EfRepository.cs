using Ardalis.Specification.EntityFrameworkCore;

namespace LunaEdge.TestAssignment.Application.Database.Repositories;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}