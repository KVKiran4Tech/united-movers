using united_movers_api.Models;

namespace united_movers_api.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<User> Login(LoginUser loginUser);
        public Task<User> Register(User registerUser);
    }
}
