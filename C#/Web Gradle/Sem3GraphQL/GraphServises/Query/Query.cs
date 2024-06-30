using GBLesson3GraphQL.Abstraction;
using GBLesson3GraphQL.DTO;
using GBLesson3GraphQL.Repo;
using HotChocolate;

namespace GBLesson3GraphQL.GraphServises.Query
{
    public class Query
    {
        public IEnumerable<ProductViewModel> GetProducts([Service] ProductRepo product) => product.GetProduts();
        public IEnumerable<ProductGroupViewModel> GetProductGroups([Service] ProductGroupRepo groups) => groups.GetProdutGroups();
    }
}
