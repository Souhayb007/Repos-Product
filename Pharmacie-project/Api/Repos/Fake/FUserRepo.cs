using Api.Enums;
using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Repos.Fake;

public class FUserRepo : IUserRepo
{
    private readonly List<User> _users = new();

    public FUserRepo()
    {
        PasswordHasher<User> _passwordHasher = new();
        
        for (int i = 0; i < 100; i++)
        {
            var role = UserRole.Client;

            if (i == 0) role = UserRole.Admin;
            else if (i <= 10) role = UserRole.Pharmacist;
            else if (i <= 20) role = UserRole.Deliverer;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = $"User {i}",
                Phone = $"Phone {i}",
                Email = $"Email {i}",
                Address = $"Address {i}",
                Username = $"Username {i}",
                Role = role,
                PharmacyId = role == UserRole.Pharmacist ? Guid.NewGuid() : null, // TODO: Get from pharmacy repo
                CostPerKM = role == UserRole.Deliverer ? new Random().Next(1, 10) : null
            };

            user.Password = _passwordHasher.HashPassword(user, $"Password {i}");

            _users.Add(user);
        }
    }

    public Task<IEnumerable<User>> AllAsync()
    {
        return Task.FromResult(_users.AsEnumerable());
    }

    public Task<IEnumerable<User>> AllAsync(UserRole role)
    {
        return Task.FromResult(_users.Where(u => u.Role == role).AsEnumerable());
    }

    public Task<User?> FindByUserNameOrEmailAsync(string usernameOrEmail)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail));
    }

    public Task<bool> SaveAsync(User user)
    {
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
        }
        else
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index == -1) return Task.FromResult(false);

            _users[index] = user;
        }

        return Task.FromResult(true);
    }
}