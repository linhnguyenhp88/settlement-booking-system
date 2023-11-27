using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettlementBookingSystem.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Infrastructure.EntityFrameworkCore
{
    public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> bookingConfiguration)
        {
            bookingConfiguration.ToTable("Booking", SettlementBookingSystemContext.DEFAULT_SCHEMA);

            bookingConfiguration.HasKey(o => o.Id);
        
            bookingConfiguration
                .Property<DateTimeOffset>("CreatedAt")
                .HasColumnName("CreatedAt")
                .IsRequired();

            bookingConfiguration
                .Property<string>("Name")
                .HasColumnName("Name")
                .IsRequired();

            bookingConfiguration
                .Property<DateTime>("BookingTime")
                .HasColumnName("BookingTime")
                .IsRequired();
        }
    }
}
