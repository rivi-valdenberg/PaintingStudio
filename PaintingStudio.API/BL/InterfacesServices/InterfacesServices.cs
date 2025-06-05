using DL.Dtos;

namespace BL.InterfacesServices
{
    public interface IImageService
    {
        Task<List<ImageDto>> GetAllImagesAsync();
        Task<ImageDto> GetImageByIdAsync(int id);
        Task<ImageDto> CreateImageAsync(CreateImageDto createImageDto);
        Task<ImageDto> UpdateImageAsync(int id, UpdateImageDto updateImageDto);
        Task<bool> DeleteImageAsync(int id);
        Task<List<ImageDto>> GetImagesByUserIdAsync(int userId);
    }
}