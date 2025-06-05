using Microsoft.AspNetCore.Http;

namespace DL.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Instructions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class CreateImageDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int UserId { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class UpdateImageDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}