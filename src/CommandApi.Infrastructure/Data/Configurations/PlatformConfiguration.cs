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
    }
}