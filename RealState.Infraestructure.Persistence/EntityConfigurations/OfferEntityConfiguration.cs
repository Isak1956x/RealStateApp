using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Enums;

namespace RealState.Infraestructure.Persistence.EntityConfigurations
{
    public class OfferEntityConfiguration : IEntityTypeConfiguration<Offer>
    {
  
            public void Configure(EntityTypeBuilder<Offer> builder)
        {
            #region Basic Configurations
            builder.HasKey(o => o.Id);
            builder.ToTable("Offers");
            #endregion

            #region Properties Configuration

            builder.Property(o => o.ClientId)
                .IsRequired();


            builder.Property(o => o.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Date)
                .IsRequired();

            builder.Property(o => o.Status)
                .IsRequired().HasDefaultValue(Status.Pending);

            #endregion

            #region Relationships Configuration

            builder.HasOne(o => o.Property)
                .WithMany(p => p.Offers)
                .HasForeignKey(o => o.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
