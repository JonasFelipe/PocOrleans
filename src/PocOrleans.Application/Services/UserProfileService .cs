using Orleans;
using Orleans.Runtime;
using PocOrleans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocOrleans.Domain.Interfaces;

namespace PocOrleans.Application.Services
{
    public class UserProfileService
    {
        private readonly IClusterClient _orleansClient;
        private readonly IUserRepository _userRepository;

        public UserProfileService(IClusterClient orleansClient, IUserRepository userRepository)
        {
            _orleansClient = orleansClient;
            _userRepository = userRepository;
        }

        public async Task<UserProfile> GetProfileAsync(Guid id)
        {
            var grain = _orleansClient.GetGrain<IUserProfileGrain>(id);
            return await grain.GetProfile();
        }

        public async Task SetProfileAsync(Guid id, string name, string email)
        {
            var grain = _orleansClient.GetGrain<IUserProfileGrain>(id);
            await grain.SetProfile(name, email);

            var user = new UserProfile { Id = id, Name = name, Email = email };
            await _userRepository.SaveAsync(user);
        }
    }
}
