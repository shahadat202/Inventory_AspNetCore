namespace Inventory.Domain.Entities
{
    public class Product : IEntity<Guid>
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

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        //public string ImageUrl { get; set; }
    }
}