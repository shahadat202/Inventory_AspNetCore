using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class ProductSearchDto
    {
        public string? Name { get; set; }
        public string? Barcode { get; set; }
        public string? CategoryId { get; set; }
        public decimal? Tax { get; set; }
    }
}
