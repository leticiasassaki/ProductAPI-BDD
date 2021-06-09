﻿using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Repositories
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
           : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
