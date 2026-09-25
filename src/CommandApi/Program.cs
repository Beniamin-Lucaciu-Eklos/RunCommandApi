namespace CommandApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, configuration) =>
            configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithThreadId()
                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                    .WriteTo.File(
                        path: "logs/app-.log",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}",
                        retainedFileCountLimit: 30));

        // Add services to the container.

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection"),
            Username = builder.Configuration["DbUserId"],
            Password = builder.Configuration["DbPassword"]
        };
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString.ConnectionString));

        builder.Services.AddMapster();

        builder.Services
                    .AddApplication()
                    .AddInfrastructure(builder.Configuration);

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
