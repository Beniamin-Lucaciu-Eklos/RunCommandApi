namespace CommandApi.Infrastructure.Data.Configurations;

public sealed class PlatformEntityConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.ToTable("Platforms");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PlatformName)
              .HasMaxLength(256)
              .IsRequired();

        builder.Property(p => p.CreatedAt)
               .IsRequired();

        builder.HasMany(p => p.Commands)
               .WithOne()
               .HasForeignKey(c=>c.PlatformId)
               .OnDelete(DeleteBehavior.Cascade);
                 
    }
}