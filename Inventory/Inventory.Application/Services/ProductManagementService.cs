﻿using Inventory.Domain;
using Inventory.Domain.Entities;
using System.Reflection.Metadata;

namespace Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ProductManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public Product GetProduct(Guid id)
        {
            return _inventoryUnitOfWork.ProductRepository.GetById(id);
        }

        public (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.ProductRepository.GetPagedProducts(pageIndex, pageSize, search, order);  
        }

        public void InsertProduct(Product product)
        {
            if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Name))
            {
                _inventoryUnitOfWork.ProductRepository.Add(product);
                _inventoryUnitOfWork.Save();
            }
        }
        
        public void UpdateProduct(Product product)
        {
            if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Name, product.Id))
            {
                _inventoryUnitOfWork.ProductRepository.Edit(product);
                _inventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Title should be unique.");
        }

        public void DeleteProduct(Guid id)
        {
            _inventoryUnitOfWork.ProductRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }

    }
}