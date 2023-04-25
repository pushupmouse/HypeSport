using Microsoft.EntityFrameworkCore;

namespace HypeSport.Repositories
{
    public class HomeRepository: IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(string sTerm="", int categoryId=0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                            join category in _db.Categories
                            on product.CategoryId equals category.CategoryId
                            where string.IsNullOrWhiteSpace(sTerm) || (product!=null && product.ProductName.ToLower().StartsWith(sTerm))
                            select new Product
                            {
                                ProductId = product.ProductId,
                                ProductImage = product.ProductImage,
                                ProductName = product.ProductName,
                                CategoryId = product.CategoryId,
                                ProductPrice = product.ProductPrice,
                                CategoryName = category.CategoryName,
                                ProductQuantity = product.ProductQuantity
                            }
                            ).ToListAsync();
            if (categoryId > 0)
            {
                products = products.Where(a => a.CategoryId == categoryId).ToList();
            }
            return products;
        }
    }
}
