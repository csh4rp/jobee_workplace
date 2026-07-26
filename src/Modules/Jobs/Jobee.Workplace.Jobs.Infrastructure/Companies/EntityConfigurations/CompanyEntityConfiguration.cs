using Jobee.Workplace.Jobs.Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobee.Workplace.Jobs.Infrastructure.Companies.EntityConfigurations;

public class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("company", Constants.SchemaName);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .IsRequired();

        builder.OwnsOne(e => e.Address, address =>
        {
            address.Property(a => a.City).HasMaxLength(200);
            address.Property(a => a.Street).HasMaxLength(200);
            address.Property(a => a.PostalCode).HasMaxLength(20);
            address.Property(a => a.Country).HasMaxLength(100);
            address.Property(a => a.FirstLine).HasMaxLength(500);
            address.Property(a => a.SecondLine).HasMaxLength(500);
        });
    }
}

