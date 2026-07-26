using Jobee.Workplace.Jobs.Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobee.Workplace.Jobs.Infrastructure.Offers.EntityConfigurations;

public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("location", Constants.SchemaName);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.OfferId)
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(e => e.Coordinates, coordinates =>
        {
            coordinates.Property(c => c.Latitude).IsRequired();
            coordinates.Property(c => c.Longitude).IsRequired();
        });

        builder.OwnsOne(e => e.Address, address =>
        {
            address.ToJson();
            address.Property(a => a.City).HasMaxLength(200);
            address.Property(a => a.Street).HasMaxLength(200);
            address.Property(a => a.PostalCode).HasMaxLength(20);
            address.Property(a => a.Country).HasMaxLength(100);
            address.Property(a => a.FirstLine).HasMaxLength(500);
            address.Property(a => a.SecondLine).HasMaxLength(500);
        });
    }
}

