using System;
using ShoppDog.Services.Catalog.Api.Infrastructure.Exceptions;

namespace ShoppDog.Services.Catalog.Api.Models
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureFileName { get; set; }
        public string PictureUri { get; set; }
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }
        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }
        public int AvailableStock { get; set; }
        public int RestockThreshold { get; set; }
        public int MaxStockThreshold { get; set; }
        public bool OnReorder { get; set; }
        public CatalogItem()
        {

        }

        public int RemoveStock(int quantityDesired)
        {
            if (AvailableStock == 0)
                throw new CatalogDomainException($"Empty stock, product item {Name} is sold out");
            if (quantityDesired <= 0)
                throw new CatalogDomainException($"Item units desired should be greater than zero");

            int removed = Math.Min(quantityDesired, this.AvailableStock);
            this.AvailableStock -= removed;
            return removed;
        }

        public int AddStock(int quantity)
        {
            int original = this.AvailableStock;

            if ((this.AvailableStock + quantity) > this.MaxStockThreshold)
                this.AvailableStock += (this.MaxStockThreshold - this.AvailableStock);
            else this.AvailableStock += quantity;

            this.OnReorder = false;
            return this.AvailableStock - original;
        }
    }
}