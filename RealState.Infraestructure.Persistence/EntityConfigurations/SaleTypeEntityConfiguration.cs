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
    public class SaleTypeEntityConfiguration : IEntityTypeConfiguration<SaleType>
    {
        public void Configure(EntityTypeBuilder<SaleType> builder)
        {
            #region Basic Configurations
            builder.HasKey(st => st.Id);
            builder.ToTable("SaleTypes");
            #endregion

            #region Properties Configuration

            builder.Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(st => st.Description)
                .HasMaxLength(1000);

            #endregion

            #region Relationships Configuration

            builder.HasMany(st => st.Properties)
                .WithOne(p => p.SaleType)
                .HasForeignKey(p => p.SaleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}
