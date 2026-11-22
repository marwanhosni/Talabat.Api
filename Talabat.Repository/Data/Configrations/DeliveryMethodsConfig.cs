using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Data.Configrations;
public class DeliveryMethodsConfig : IEntityTypeConfiguration<DeliveryMethods>
{
     public void Configure(EntityTypeBuilder<DeliveryMethods> builder)
    {
        builder.Property(dm => dm.Cost ).HasColumnType("decimal(18,2)");
    }
}
