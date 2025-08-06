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
    public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            #region Basic Configurations
            builder.HasKey(m => m.Id);
            builder.ToTable("Messages");
            #endregion

            #region Properties Configuration


            builder.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(2000); 

            builder.Property(m => m.SenderID)
                .IsRequired();

            builder.Property(m => m.Date)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");;

            #endregion

            #region Relationships Configuration

            builder.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatID)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
