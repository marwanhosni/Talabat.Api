using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Data.Configrations;
public class OrderConfiGurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(O => O.Subtotal).HasColumnType("decimal(18,2)");
        builder.Property(builder => builder.Status)
            .HasConversion(
            status => status.ToString(),
            status => (OrderStatus)Enum.Parse(typeof(OrderStatus), status)
        );
        builder.OwnsOne(O => O.ShippingAddress, a =>
        {
            a.WithOwner();
        });
         builder.HasOne(O => O.DeliveryMethods)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
