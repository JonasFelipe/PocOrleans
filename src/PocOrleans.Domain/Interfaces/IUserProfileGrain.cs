using Orleans;
using PocOrleans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocOrleans.Domain.Interfaces
{
    public interface IUserProfileGrain : IGrainWithGuidKey
    {
        Task<UserProfile> GetProfile();
        Task SetProfile(string name, string email);
    }
}
