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
    public class ChatEntityConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            #region Basic Configurations
            builder.HasKey(c => c.ChatId);
            builder.ToTable("Chats");
            #endregion

            #region Properties Configuration

            builder.Property(c => c.PropertyId)
                .IsRequired();

            builder.Property(c => c.CustomerId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.AgentId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.CreateAt)
           .IsRequired()
          .HasDefaultValueSql("GETDATE()");


            #endregion

            #region Relationships Configuration

            builder.HasOne(c => c.Property)
                .WithMany()
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatID)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
