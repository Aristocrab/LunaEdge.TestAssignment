using Ardalis.Specification;
using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Users.Specifications;

public sealed class UserByUsernameOrEmail : Specification<User>
{
    public UserByUsernameOrEmail(string usernameOrEmail)
    {
        Query.Where(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
    }
}