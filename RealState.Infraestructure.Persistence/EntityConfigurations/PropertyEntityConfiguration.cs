using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;


namespace RealState.Infraestructure.Persistence.EntityConfigurations
{
    public class PropertyEntityConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            #region Basic Configuations
            builder.HasKey(p => p.Id);
            builder.ToTable("Properties");
            #endregion

            #region Properties Configuration

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(6); // Puedes ajustar el tamaño si lo deseas

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.SizeInMeters)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Bedrooms)
                .IsRequired();

            builder.Property(p => p.Bathrooms)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(1000); // Opcional, ajusta el límite si quieres

            builder.Property(p => p.IsAvailable)
                .IsRequired().HasDefaultValue(true);

            builder.Property(p => p.AgentId)
                .IsRequired();
            #endregion

            #region Relatioships Configuration
            builder.HasOne(p => p.PropertyType)
                .WithMany()
                .HasForeignKey(p => p.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.SaleType)
                .WithMany()
                .HasForeignKey(p => p.SaleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.PropertyImprovements)
                .WithOne()
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Offers)
                .WithOne()
                .HasForeignKey(o => o.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
