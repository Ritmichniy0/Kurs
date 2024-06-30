using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using GBLesson3GraphQL.Abstraction;
using GBLesson3GraphQL.DB;
using GBLesson3GraphQL.DTO;
using GBLesson3GraphQL.Models;

namespace GBLesson3GraphQL.Repo
{
    public class ProductGroupRepo : IProductGroupRepo
    {
        private readonly IMapper _mapper;
        private IMemoryCache _memoryCache;

        public ProductGroupRepo(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public int AddProductGroup(ProductGroupViewModel productGroupViewModel)
        {
            using (var productContext = new ProductContext())
            {
                var entityGroup = productContext.ProductGroup.FirstOrDefault(x => x.Name!.ToLower().Equals(productGroupViewModel.Name!.ToLower()));
                if (entityGroup == null)
                {
                    var entity = _mapper.Map<ProductGroup>(productGroupViewModel);
                    productContext.ProductGroup.Add(entity);
                    productContext.SaveChanges();
                    _memoryCache.Remove("groups");
                    productGroupViewModel.Id = entity.Id;
                }
                else
                {
                    throw new Exception("ProductGroup already exist");
                }
            }
            return productGroupViewModel.Id ?? 0;
        }


        public IEnumerable<ProductGroupViewModel> GetProdutGroups()
        {
            if (_memoryCache.TryGetValue("groups", out List<ProductGroupViewModel> productGroupsCache))
            {
                return productGroupsCache!;
            }
            using (var productContext = new ProductContext())
            {
                var groups = productContext.ProductGroup.Select(_mapper.Map<ProductGroupViewModel>).ToList();
                _memoryCache.Set("groups", groups, TimeSpan.FromMinutes(30));
                return groups;
            }
        }


        public int DeleteProductGroup(int id)
        {
            using (var productContext = new ProductContext())
            {
                if (productContext.ProductGroup.Count(x => x.Id == id) > 0)
                {
                    var entityProductGroup = productContext.ProductGroup.FirstOrDefault(x => x.Id == id);
                    if (productContext.Products.FirstOrDefault(x => x.ProductGroupId == entityProductGroup!.Id) == null)
                    {
                        productContext.ProductGroup.Remove(entityProductGroup!);
                        productContext.SaveChanges();
                        _memoryCache.Remove("groups");
                    }
                    else
                    {
                        throw new Exception("Group is not empty");
                    }
                }
                else
                {
                    throw new Exception("Group does not exist");
                }
            }

            return id;
        }

        
    }
}
