using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using GBLesson3GraphQL.Abstraction;
using GBLesson3GraphQL.DB;
using GBLesson3GraphQL.DTO;
using GBLesson3GraphQL.Models;

namespace GBLesson3GraphQL.Repo
{
    public class ProductRepo : IProductRepo
    {
        private readonly IMapper _mapper;
        private IMemoryCache _memoryCache;

        public ProductRepo(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public int AddProduct(ProductViewModel productViewModel)
        {
            using (var productContext = new ProductContext())
            {
                var entityProduct = productContext.Products.FirstOrDefault(x => x.Name!.ToLower().Equals(productViewModel.Name!.ToLower()));
                if (entityProduct == null)
                {
                    var entity = _mapper.Map<Product>(productViewModel);
                    productContext.Products.Add(entity);
                    productContext.SaveChanges();
                    _memoryCache.Remove("products");
                    productViewModel.Id = entity.Id;
                }
                else
                {
                    throw new Exception("Product already exist");
                }
            }
            return productViewModel.Id ?? 0; // означает, если Id = 0, то вернем 0
        }

        public IEnumerable<ProductViewModel> GetProduts()
        {
            if (_memoryCache.TryGetValue("products", out List<ProductViewModel>? productsCache)) // проверяет, если кэш не пустой
            {
                return productsCache!;
            }
            using (var productContext = new ProductContext())
            {
                var products = productContext.Products.Select(_mapper.Map<ProductViewModel>).ToList();
                _memoryCache.Set("products", products, TimeSpan.FromMinutes(30));   //"products" - ключ, products - что кэшируем,
                                                                                    //TimeSpan.FromMinutes(30) - время кэша
                return products;
            }
        }

        public int UpdateProduct(int id, float price)
        {
            using (var productContext = new ProductContext())
            {
                if (productContext.Products.Count(x => x.Id == id) > 0)
                {
                    var x = productContext.Products.FirstOrDefault(x => x.Id == id);
                    x.Price = price;
                    productContext.SaveChanges();
                    _memoryCache.Remove("products");
                }
                else
                {
                    throw new Exception("There is no such product");
                }
            }
            return id;
        }

        public int DeleteProduct(int id)
        {
            using (var productContext = new ProductContext())
            {
                if (productContext.Products.Count(x => x.Id == id) > 0)
                {
                    var x = productContext.Products.FirstOrDefault(x => x.Id == id);
                    productContext.Products.Remove(x);
                    productContext.SaveChanges();
                    _memoryCache.Remove("products");
                }
                else
                {
                    throw new Exception("There is no such product");
                }
            }
            return id;
        }
    }
}
