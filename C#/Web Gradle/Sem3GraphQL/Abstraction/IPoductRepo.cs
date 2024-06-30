using GBLesson3GraphQL.DTO;

namespace GBLesson3GraphQL.Abstraction
{
    public interface IProductRepo
    {
        public int AddProduct(ProductViewModel productViewModel);
        public IEnumerable<ProductViewModel> GetProduts();
        public int UpdateProduct(int id, float price);
        public int DeleteProduct(int id);
    }
}
