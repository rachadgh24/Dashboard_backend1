namespace task1.Application.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public required string Model { get; set; }
        public required int maxSpeed { get; set; }
        public int? CustomerId { get; set; }
    }
}