namespace GBLesson3GraphQL.DTO
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ProductGroupId { get; set; }
        public float Price { get; set; } = 0;
    }
}
