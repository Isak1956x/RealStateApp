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
    public class PropertyTypeEntityConfiguration : IEntityTypeConfiguration<PropertyType>
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            #region Basic Configurations
            builder.HasKey(pt => pt.Id);
            builder.ToTable("PropertyTypes");
            #endregion

            #region Properties Configuration

            builder.Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pt => pt.Description)
                .HasMaxLength(1000);

            #endregion

            #region Relationships Configuration

            builder.HasMany(pt => pt.Properties)
                .WithOne(p => p.PropertyType)
                .HasForeignKey(p => p.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}
