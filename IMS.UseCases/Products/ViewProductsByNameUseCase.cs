﻿using IMS.CoreBusiness;
using IMS.UseCases.Plugininterfaces;
using IMS.UseCases.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Products
{
    public class ViewProductsByNameUseCase : IViewProductsByNameUseCase
    {
        private readonly IProductRepository productRepository;

        public ViewProductsByNameUseCase(IProductRepository productRepository) 
        {
            this.productRepository = productRepository;
        }


        public async Task<IEnumerable<Product>> ExecuteAsync(string name = "") //returns a list of inventories
        {
            return await productRepository.GetProductsByNameUseCase(name);
        }
    }
}
