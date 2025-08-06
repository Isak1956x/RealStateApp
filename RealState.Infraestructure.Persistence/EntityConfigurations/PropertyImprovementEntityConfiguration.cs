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
    public class PropertyImprovementEntityConfiguration : IEntityTypeConfiguration<PropertyImprovement>
    {
        public void Configure(EntityTypeBuilder<PropertyImprovement> builder)
        {
            #region Basic Configurations
            builder.HasKey(pi => new { pi.PropertyId, pi.ImprovementId });
            builder.ToTable("PropertyImprovements");
            #endregion

      

            #region Relationships Configuration

            builder.HasOne(pi => pi.Property)
                .WithMany(p => p.PropertyImprovements)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pi => pi.Improvement)
                .WithMany()
                .HasForeignKey(pi => pi.ImprovementId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

        }
    }
}
