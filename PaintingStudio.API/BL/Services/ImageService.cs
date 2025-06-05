using DL.Dtos;
using BL.InterfacesServices;
using DL;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace BL.Services
{
    public class ImageService : IImageService
    {
        private readonly IDataContext _context;
        private readonly IS3Service _s3Service;

        public ImageService(IDataContext context, IS3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        public async Task<List<ImageDto>> GetAllImagesAsync()
        {
            var images = await _context.Images
                .Include(i => i.User)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            //return images.Select(MapToDto).ToList();
            return images.Select(i => MapToDto(i)).ToList();

        }

        public async Task<ImageDto> GetImageByIdAsync(int id)
        {
            var image = await _context.Images
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);

            return image != null ? MapToDto(image) : null;
        }

        public async Task<ImageDto> CreateImageAsync(CreateImageDto createImageDto)
        {
            string imageUrl = null;

            if (createImageDto.ImageFile != null)
            {
                using var stream = createImageDto.ImageFile.OpenReadStream();
                var fileName = $"{Guid.NewGuid()}_{createImageDto.ImageFile.FileName}";
                imageUrl = await _s3Service.UploadImageAsync(stream, fileName, createImageDto.ImageFile.ContentType);
            }

            var image = new Image
            {
                Title = createImageDto.Title,
                Description = createImageDto.Description,
                Instructions = createImageDto.Instructions,
                ImageUrl = imageUrl,
                UserId = createImageDto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return await GetImageByIdAsync(image.Id);
        }

        public async Task<ImageDto> UpdateImageAsync(int id, UpdateImageDto updateImageDto)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return null;

            if (updateImageDto.ImageFile != null)
            {
                // Delete old image from S3
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    await _s3Service.DeleteImageAsync(image.ImageUrl);
                }

                // Upload new image
                using var stream = updateImageDto.ImageFile.OpenReadStream();
                var fileName = $"{Guid.NewGuid()}_{updateImageDto.ImageFile.FileName}";
                image.ImageUrl = await _s3Service.UploadImageAsync(stream, fileName, updateImageDto.ImageFile.ContentType);
            }

            image.Title = updateImageDto.Title ?? image.Title;
            image.Description = updateImageDto.Description ?? image.Description;
            image.Instructions = updateImageDto.Instructions ?? image.Instructions;
            image.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetImageByIdAsync(id);
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return false;

            // Delete image from S3
            if (!string.IsNullOrEmpty(image.ImageUrl))
            {
                await _s3Service.DeleteImageAsync(image.ImageUrl);
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ImageDto>> GetImagesByUserIdAsync(int userId)
        {
            var images = await _context.Images
                .Include(i => i.User)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            //return images.Select(MapToDto).ToList();
            return images.Select(i => MapToDto(i)).ToList();

        }

        private static ImageDto MapToDto(Image image)
        {
            return new ImageDto
            {
                Id = image.Id,
                Title = image.Title,
                Description = image.Description,
                ImageUrl = image.ImageUrl,
                Instructions = image.Instructions,
                CreatedAt = image.CreatedAt,
                UpdatedAt = image.UpdatedAt,
                UserId = image.UserId,
                UserName = image.User?.Name
            };
        }
    }
}