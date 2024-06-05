using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configration
{
    public class ProductConfigrations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                   .IsRequired();
            builder.Property(p=>p.Description)
                   .IsRequired();
            builder.Property(p=>p.PictureUrl)
                   .IsRequired();
            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");
            builder.HasOne(p => p.ProductBrand)
                   .WithMany()
                   .HasForeignKey(p=>p.BrandId);
            builder.HasOne(p=>p.Productcategories) 
                   .WithMany()
                   .HasForeignKey(p=>p.categorieId);
            
        }
    }
}
