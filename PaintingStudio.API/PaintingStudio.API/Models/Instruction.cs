namespace PaintingStudio.API.Models
{
    public class instruction
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string StepNumber { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Item Item { get; set; }
    }
}