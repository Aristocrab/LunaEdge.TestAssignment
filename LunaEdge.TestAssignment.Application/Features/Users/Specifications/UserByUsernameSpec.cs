using Ardalis.Specification;
using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Users.Specifications;

public sealed class UserByUsernameSpec : Specification<User>
{
    public UserByUsernameSpec(string username)
    {
        Query.Where(u => u.Username == username);
    }
}