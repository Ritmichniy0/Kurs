namespace GBLesson3GraphQL.Models
{
    public class ProductStorage
    {
		public int ProductId { get; set; }
		public int StorageId { get; set; }
		public int Count { get; set; }

		public virtual Product? Product { get; set; }
        public virtual Storage? Storage { get; set; }
        
    }
}
