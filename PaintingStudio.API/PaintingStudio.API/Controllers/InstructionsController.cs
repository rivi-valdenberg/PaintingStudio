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
    public class InstructionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly S3Service _s3Service;

        public InstructionsController(ApplicationDbContext context, S3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        // GET: api/instructions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instruction>>> GetInstructions()
        {
            return await _context.Instructions.ToListAsync();
        }

        // GET: api/instructions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Instruction>> GetInstruction(int id)
        {
            var instruction = await _context.Instructions.FindAsync(id);

            if (instruction == null)
            {
                return NotFound();
            }

            return instruction;
        }

        // GET: api/instructions/item/5
        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<Instruction>>> GetInstructionsByItem(int itemId)
        {
            return await _context.Instructions
                .Where(i => i.ItemId == itemId)
                .OrderBy(i => i.StepNumber)
                .ToListAsync();
        }

        // POST: api/instructions
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Instruction>> CreateInstruction([FromForm] InstructionCreateDto dto)
        {
            var item = await _context.Items.FindAsync(dto.ItemId);

            if (item == null)
            {
                return BadRequest("Item not found");
            }

            var instruction = new Instruction
            {
                ItemId = dto.ItemId,
                StepNumber = dto.StepNumber,
                Description = dto.Description
            };

            if (dto.Image != null)
            {
                using var stream = dto.Image.OpenReadStream();
                instruction.ImageUrl = await _s3Service.UploadFileAsync(
                    stream,
                    dto.Image.FileName,
                    dto.Image.ContentType);
            }

            _context.Instructions.Add(instruction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInstruction), new { id = instruction.Id }, instruction);
        }

        // PUT: api/instructions/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInstruction(int id, [FromForm] InstructionUpdateDto dto)
        {
            var instruction = await _context.Instructions.FindAsync(id);

            if (instruction == null)
            {
                return NotFound();
            }

            instruction.StepNumber = dto.StepNumber;
            instruction.Description = dto.Description;

            if (dto.Image != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(instruction.ImageUrl))
                {
                    await _s3Service.DeleteFileAsync(instruction.ImageUrl);
                }

                // Upload new image
                using var stream = dto.Image.OpenReadStream();
                instruction.ImageUrl = await _s3Service.UploadFileAsync(
                    stream,
                    dto.Image.FileName,
                    dto.Image.ContentType);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/instructions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInstruction(int id)
        {
            var instruction = await _context.Instructions.FindAsync(id);

            if (instruction == null)
            {
                return NotFound();
            }

            // Delete image from S3 if exists
            if (!string.IsNullOrEmpty(instruction.ImageUrl))
            {
                await _s3Service.DeleteFileAsync(instruction.ImageUrl);
            }

            _context.Instructions.Remove(instruction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class InstructionCreateDto
    {
        public int ItemId { get; set; }
        public string StepNumber { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }

    public class InstructionUpdateDto
    {
        public string StepNumber { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}