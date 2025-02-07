using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SupportAI.Domain.Entities.Identity;
using SupportAI.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SupportAI.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth");

            group.MapPost("/register", async (UserRegistrationDto userDto, UserManager<User> userManager) =>
            {
                var user = new User
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    FullName = userDto.FullName,
                    TenantId = userDto.TenantId
                };

                var result = await userManager.CreateAsync(user, userDto.Password);
                return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
            });

            group.MapPost("/login", async (UserLoginDto loginDto, UserManager<User> userManager) =>
            {
                var user = await userManager.FindByEmailAsync(loginDto.Email);
                if (user is null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
                    return Results.Unauthorized();

                var token = GenerateJwtToken(user);
                return Results.Ok(new { Token = token });
            });
        }

        private static string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes("YourSuperSecretKeyHere");
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("TenantId", user.TenantId.ToString()) // Multi-tenancy support
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
