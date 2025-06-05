using DL.Dtos;
using BL.InterfacesServices;
using DL;
using DL.Dtos;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext _context;

        public UserService(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var hashedPassword = HashPassword(createUserDto.Password);

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                PasswordHash = hashedPassword,
                Role = createUserDto.Role,
                AccessCode = createUserDto.AccessCode ?? GenerateAccessCode(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Name = updateUserDto.Name ?? user.Name;
            user.Email = updateUserDto.Email ?? user.Email;
            user.IsActive = updateUserDto.IsActive;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.PasswordHash = HashPassword(updateUserDto.Password);
            }

            await _context.SaveChangesAsync();
            return MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return MapToDto(user);
        }

        public async Task<UserDto> AuthenticateByCodeAsync(string accessCode)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.AccessCode == accessCode && u.IsActive);

            return user != null ? MapToDto(user) : null;
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                AccessCode = user.AccessCode,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }

        private static string GenerateAccessCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}