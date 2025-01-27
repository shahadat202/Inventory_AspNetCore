using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public int StockQuantity { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal SellingWithTax { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string CategoryName { get; set; }
        public string? Image { get; set; }

    }
}
