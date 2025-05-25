using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintingStudio.API.Data;
using PaintingStudio.API.Models;
using PaintingStudio.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaintingStudio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly S3Service _s3Service;

        public ItemsController(ApplicationDbContext context, S3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items
                .Include(i => i.Instructions)
                .ToListAsync();
        }

        // GET: api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items
                .Include(i => i.Instructions)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST: api/items
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Item>> CreateItem([FromForm] ItemCreateDto dto)
        {
            var item = new Item
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                Instructions = new List<Instruction>()
            };

            if (dto.Image != null)
            {
                using var stream = dto.Image.OpenReadStream();
                item.ImageUrl = await _s3Service.UploadFileAsync(
                    stream,
                    dto.Image.FileName,
                    dto.Image.ContentType);
            }

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // PUT: api/items/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] ItemUpdateDto dto)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = dto.Name;
            item.Description = dto.Description;

            if (dto.Image != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    await _s3Service.DeleteFileAsync(item.ImageUrl);
                }

                // Upload new image
                using var stream = dto.Image.OpenReadStream();
                item.ImageUrl = await _s3Service.UploadFileAsync(
                    stream,
                    dto.Image.FileName,
                    dto.Image.ContentType);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/items/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items
                .Include(i => i.Instructions)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            // Delete all images from S3
            if (!string.IsNullOrEmpty(item.ImageUrl))
            {
                await _s3Service.DeleteFileAsync(item.ImageUrl);
            }

            foreach (var instruction in item.Instructions)
            {
                if (!string.IsNullOrEmpty(instruction.ImageUrl))
                {
                    await _s3Service.DeleteFileAsync(instruction.ImageUrl);
                }
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class ItemCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }

    public class ItemUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}