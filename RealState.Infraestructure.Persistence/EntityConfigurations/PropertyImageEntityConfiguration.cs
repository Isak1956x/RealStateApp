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
    public class PropertyImageEntityConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            #region Basic Configurations
            builder.HasKey(pi => pi.Id);
            builder.ToTable("PropertyImages");
            #endregion

            #region Properties Configuration

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasMaxLength(500); 


            #endregion

            #region Relationships Configuration

            builder.HasOne(pi => pi.Property)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
