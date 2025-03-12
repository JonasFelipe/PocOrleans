using Orleans;
using PocOrleans.Domain.Entities;
using PocOrleans.Domain.Interfaces;
using System.Threading.Tasks;

namespace PocOrleans.Infrastructure.Grains;

public class UserProfileGrain : Grain, IUserProfileGrain
{
    private UserProfile _profile = new();

    public Task<UserProfile> GetProfile() => Task.FromResult(_profile);

    public Task SetProfile(string name, string email)
    {
        _profile.Name = name;
        _profile.Email = email;
        return Task.CompletedTask;
    }
}
