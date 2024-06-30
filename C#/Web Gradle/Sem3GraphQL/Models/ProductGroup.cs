namespace GBLesson3GraphQL.Models
{
    public class ProductGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
