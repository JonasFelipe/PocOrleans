using Microsoft.EntityFrameworkCore;
using PocOrleans.Domain.Entities; // Add this line
using PocOrleans.Domain.Interfaces;
using PocOrleans.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PocOrleans.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProfile> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id) ?? new UserProfile { Id = id };
    }

    public async Task SaveAsync(UserProfile user)
    {
        if (_dbContext.Users.Any(u => u.Id == user.Id))
            _dbContext.Users.Update(user);
        else
            _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();
    }
}