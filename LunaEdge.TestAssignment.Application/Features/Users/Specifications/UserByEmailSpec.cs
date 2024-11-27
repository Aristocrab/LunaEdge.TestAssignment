using Ardalis.Specification;
using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Users.Specifications;

public sealed class UserByEmailSpec : Specification<User>
{
    public UserByEmailSpec(string email)
    {
        Query.Where(u => u.Email == email);
    }
}