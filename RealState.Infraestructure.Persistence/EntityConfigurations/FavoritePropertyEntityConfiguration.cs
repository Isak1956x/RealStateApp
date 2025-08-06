using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;

namespace RealState.Infraestructure.Persistence.EntityConfigurations
{
    public class FavoritePropertyEntityConfiguration : IEntityTypeConfiguration<FavoriteProperty>
    {
        public void Configure(EntityTypeBuilder<FavoriteProperty> builder)
        {
            #region Basic Configurations
            builder.HasKey(fp => fp.FavoritePropertyId);
            builder.ToTable("FavoriteProperties");
            #endregion

            #region Properties Configuration

            builder.Property(fp => fp.UserId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(fp => fp.CreateAt)
              .IsRequired()
          .HasDefaultValueSql("GETDATE()");
            #endregion

            #region Relationships Configuration

            builder.HasOne(fp => fp.Property)
                .WithMany()
                .HasForeignKey(fp => fp.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
