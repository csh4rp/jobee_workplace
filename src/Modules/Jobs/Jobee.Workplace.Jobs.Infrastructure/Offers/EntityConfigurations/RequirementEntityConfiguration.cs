using Jobee.Workplace.Jobs.Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobee.Workplace.Jobs.Infrastructure.Offers.EntityConfigurations;

public class RequirementEntityConfiguration : IEntityTypeConfiguration<Requirement>
{
    public void Configure(EntityTypeBuilder<Requirement> builder)
    {
        builder.ToTable("requirement", Constants.SchemaName);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.OfferId)
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Level)
            .IsRequired()
            .HasConversion<string>();
    }
}


