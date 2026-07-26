using Jobee.Workplace.Shared.Infrastructure;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jobee.Workplace.Infrastructure.Postgres;

public class PostgresDbContext : DbContext, IDataProtectionKeyContext
{
    private const string SharedSchemaName = "shared";
    
    private readonly InfrastructureAssemblyCollection _configurationAssemblies;

    public DbSet<DataProtectionKey> DataProtectionKeys { get; } = null!;
    
    public PostgresDbContext(InfrastructureAssemblyCollection configurationAssemblies,
        DbContextOptions<PostgresDbContext> options) : base(options)
    {
        _configurationAssemblies = configurationAssemblies;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.AddInboxStateEntity(c =>
        {
            c.ToTable("inbox_state", SharedSchemaName);
        });

        modelBuilder.AddOutboxStateEntity(c =>
        {
            c.ToTable("outbox_state", SharedSchemaName);
        });

        modelBuilder.AddOutboxMessageEntity(c =>
        {
            c.ToTable("outbox_message", SharedSchemaName);
        });
        
        new JobSagaMap(false).Configure(modelBuilder);
        new JobTypeSagaMap(false).Configure(modelBuilder);
        new JobAttemptSagaMap(false).Configure(modelBuilder);
        modelBuilder.Entity<JobSaga>().ToTable("job_saga", SharedSchemaName);
        modelBuilder.Entity<JobTypeSaga>().ToTable("job_type_saga", SharedSchemaName);
        modelBuilder.Entity<JobAttemptSaga>().ToTable("job_attempt_saga", SharedSchemaName);

        foreach (var assembly in _configurationAssemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }

}