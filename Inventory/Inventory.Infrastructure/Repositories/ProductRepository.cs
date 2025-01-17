using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        private readonly InventoryDbContext _context;
        public ProductRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }

        public (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, y => y.Include(z => z.Category),
                    pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Name.Contains(search.Value), order,
                    y => y.Include(z => z.Category), pageIndex, pageSize, true);
        }

        //public (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex, 
        //    int pageSize, DataTablesSearch search, string? order)
        //{
        //    var query = _context.Products.AsQueryable();

        //    if (!string.IsNullOrEmpty(search.Value))
        //    {
        //        query = query.Where(p => p.Name.Contains(search.Value) ||
        //                         p.Barcode.Contains(search.Value) ||
        //                         p.Category.Contains(search.Value) ||
        //                         p.Tax.ToString().Contains(search.Value) ||
        //                         p.SellingWithTax.ToString().Contains(search.Value) ||
        //                         p.StockQuantity.ToString().Contains(search.Value) ||
        //                         p.Status.ToString().Contains(search.Value));
        //    }

        //    int total = query.Count();

        //    if (!string.IsNullOrEmpty(order))
        //        query = query.OrderBy(order);

        //    var data = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        //    return (data, total, total);
        //}

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.Name == title) > 0;
            }
            else
            {
                return GetCount(x => x.Name == title) > 0;
            }
        }

        public bool IsProductExist(string title, Guid id)
        {
            return _context.Products.Any(x => x.Name == title && x.Id == id);       
        }
    }
}
