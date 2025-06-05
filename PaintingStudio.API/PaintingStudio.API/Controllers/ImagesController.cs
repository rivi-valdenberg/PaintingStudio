using DL.Dtos;
using BL.InterfacesServices;
using Microsoft.AspNetCore.Mvc;

namespace PaintingStudio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ImageDto>>> GetAllImages()
        {
            var images = await _imageService.GetAllImagesAsync();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDto>> GetImage(int id)
        {
            var image = await _imageService.GetImageByIdAsync(id);
            if (image == null)
                return NotFound();

            return Ok(image);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ImageDto>>> GetImagesByUser(int userId)
        {
            var images = await _imageService.GetImagesByUserIdAsync(userId);
            return Ok(images);
        }

        [HttpPost]
        public async Task<ActionResult<ImageDto>> CreateImage([FromForm] CreateImageDto createImageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = await _imageService.CreateImageAsync(createImageDto);
            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ImageDto>> UpdateImage(int id, [FromForm] UpdateImageDto updateImageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = await _imageService.UpdateImageAsync(id, updateImageDto);
            if (image == null)
                return NotFound();

            return Ok(image);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var result = await _imageService.DeleteImageAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}