using DL.Dtos;

namespace BL.InterfacesServices
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto> AuthenticateAsync(string email, string password);
        Task<UserDto> AuthenticateByCodeAsync(string accessCode);
    }
}