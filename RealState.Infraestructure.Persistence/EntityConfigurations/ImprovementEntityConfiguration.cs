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
    public class ImprovementEntityConfiguration : IEntityTypeConfiguration<Improvement>
    {
        public void Configure(EntityTypeBuilder<Improvement> builder)
        {
            #region Basic Configurations
            builder.HasKey(i => i.Id);
            builder.ToTable("Improvements");
            #endregion

            #region Properties Configuration

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100); 

            builder.Property(i => i.Description)
                .HasMaxLength(1000); 

            #endregion

        }
    }
}
