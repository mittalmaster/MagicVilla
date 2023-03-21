using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly string SecretKey;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            SecretKey = configuration.GetValue<String>("AppSetting:SecretKey");

        }
        public async Task<bool> IsUniqueUser(string username)
        {
            var z = await _db.LocalUsers.FirstOrDefaultAsync(x => x.UserName == username);
            if (z == null)
                return true;
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var loginResponseDTO = new LoginResponseDTO();
            var user = await _db.LocalUsers.FirstOrDefaultAsync(u=>u.UserName== loginRequestDTO.UserName && u.Password==loginRequestDTO.Password);
            if(user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };

            }



            //Generate JWT Token here 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            loginResponseDTO.Token = tokenHandler.WriteToken(token);
            loginResponseDTO.User = user;
            return loginResponseDTO;

        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            LocalUser local = new()
            {
                UserName = registrationRequestDTO.UserName,             
                Name= registrationRequestDTO.Name,
                Role= registrationRequestDTO.Role,
                Password= registrationRequestDTO.Password                
            };
            await _db.LocalUsers.AddAsync(local);
            await _db.SaveChangesAsync();
            local.Password = string.Empty;
            return local;

        }
    }
}
