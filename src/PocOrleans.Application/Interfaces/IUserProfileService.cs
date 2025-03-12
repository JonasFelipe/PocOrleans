using PocOrleans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocOrleans.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetProfile(Guid id);
        Task SetProfile(Guid id, string name, string email);    
    }
}
