using GBLesson3GraphQL.Abstraction;
using GBLesson3GraphQL.DTO;
using GBLesson3GraphQL.Repo;

namespace GBLesson3GraphQL.GraphServises.Mutation
{
    public class Mutation
    {
        private IProductRepo _poductRepo;
        private IProductGroupRepo _productGroupRepo;

        public Mutation(IProductRepo poductRepo, IProductGroupRepo productGroupRepo)
        {
            _poductRepo = poductRepo;
            _productGroupRepo = productGroupRepo;
        }

        public int AddProduct(ProductViewModel productViewModel) => _poductRepo.AddProduct(productViewModel);
        public int UpdateProduct(int id, float price) => _poductRepo.UpdateProduct(id, price);
        public int DeleteProduct(int id) => _poductRepo.DeleteProduct(id);


        public int AddProductGroup(ProductGroupViewModel productGroupViewModel) => _productGroupRepo.AddProductGroup(productGroupViewModel);
        public int DeleteProductGroup(int id) => _productGroupRepo.DeleteProductGroup(id);
        
    } 
}
