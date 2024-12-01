using united_movers_api.Services.Interfaces;
using Isopoh.Cryptography.Argon2;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using united_movers_api.Models;

namespace united_movers_api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UnitedMoversDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public AuthService(UnitedMoversDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<User> Login(LoginUser loginUser)
        {
            // Search user in DB and verify password

            User? user = _dbContext.Users.Where(user => user.Username == loginUser.UserName).FirstOrDefault();

            if (user == null || Argon2.Verify(user.PasswordHash, loginUser.Password) == false)
            {
                return null; //returning null intentionally to show that login was unsuccessful
            }

            // Create JWT token handler and get secret key

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

            // Prepare list of user claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.Username)
            };
            foreach (var role in user.UserRoleMappings)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.RoleName));
            }

            // Create token descriptor

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            // Create token and set it to user

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.IsActive = true;

            return user;
        }

        public async Task<User> Register(User registerUser)
        {
            // Add user to DB

            registerUser.PasswordHash = Argon2.Hash(registerUser.PasswordHash);
            _dbContext.Users.Add(registerUser);
            await _dbContext.SaveChangesAsync();

            return registerUser;
        }
    }
}
