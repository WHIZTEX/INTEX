﻿namespace INTEX.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set;}
        public PaginationInfo PaginationInfo { get; set;}
    }
}