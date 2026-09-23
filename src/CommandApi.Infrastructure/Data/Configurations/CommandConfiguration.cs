namespace CommandApi.Infrastructure.Data.Configurations;

public class CommandConfiguration : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> builder)
    {
        builder.ToTable("Commands");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.HowTo)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(c => c.CommandLine)
               .IsRequired();

        builder.Property(c => c.CreatedAt)
               .IsRequired();

        builder.HasIndex(c => c.PlatformId)
               .HasDatabaseName("Index_Command_PlatformId");
    }
}