﻿using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IProductManagementService
    {
        Product GetProduct(Guid id);
        Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order);
        void InsertProduct(Product product);
        void UpdateProduct(Product product);
        bool ProductExists(string name, Guid id);
        void DeleteProduct(Guid id);
    }
}