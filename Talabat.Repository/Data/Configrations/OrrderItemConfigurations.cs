using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Data.Configrations;
public class OrrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(oI => oI.Product, p =>
        {
            p.WithOwner();
            p.Property(p => p.ProductName).IsRequired();
            p.Property(p => p.PictureUrl).IsRequired();
        });
        builder.Property(oI => oI.Price).HasColumnType("decimal(18,2)");

    }
}
