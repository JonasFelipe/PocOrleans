using PocOrleans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocOrleans.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserProfile> GetByIdAsync(Guid id);
        Task SaveAsync(UserProfile user);
    }
}
