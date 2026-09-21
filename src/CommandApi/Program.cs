namespace CommandApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection"),
            Username = builder.Configuration["DbUserId"],
            Password = builder.Configuration["DbPassword"]
        };

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString.ConnectionString));
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
