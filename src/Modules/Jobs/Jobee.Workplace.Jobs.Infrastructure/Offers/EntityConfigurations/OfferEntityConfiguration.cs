using Jobee.Workplace.Jobs.Domain.Companies;
using Jobee.Workplace.Jobs.Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobee.Workplace.Jobs.Infrastructure.Offers.EntityConfigurations;

public class OfferEntityConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("offer", Constants.SchemaName);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CompanyId)
            .IsRequired();

        builder.Property(e => e.CategoryId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .IsRequired();

        builder.Property(e => e.WorkType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.ContractType)
            .IsRequired()
            .HasConversion<string>();
        
        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(e => e.CompanyId);

        builder.HasMany(e => e.Requirements)
            .WithOne()
            .HasForeignKey(r => r.OfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Locations)
            .WithOne()
            .HasForeignKey(l => l.OfferId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

