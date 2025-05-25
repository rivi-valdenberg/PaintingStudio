// Models/Item.cs
namespace PaintingStudio.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Instruction> Instructions { get; set; }
    }
}

// Models/Instruction.cs
namespace PaintingStudio.API.Models
{
    public class Instruction
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string StepNumber { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Item Item { get; set; }
    }
}

// Models/User.cs
namespace PaintingStudio.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
    }
}