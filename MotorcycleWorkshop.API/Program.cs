using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleWorkshop.API.Mappings;
using MotorcycleWorkshop.API.Validators;
using MotorcycleWorkshop.Infrastructure;

namespace MotorcycleWorkshop.API;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // DbContext-Registrierung: hier mit SQLite (oder wahlweise InMemory)
        builder.Services.AddDbContext<WorkshopDBContext>(options =>
                // options.UseSqlite("Data Source=workshop.db")
             options.UseInMemoryDatabase("WorkshopDb")
        );

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddValidatorsFromAssemblyContaining<CreateRepairCommandValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateRepairCommandValidator>();

        var app = builder.Build();

        // ─── Datenbank komplett neu anlegen (nur für Dev, ohne Migrationen) ─────────
        using (var scope = app.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
        // ─────────────────────────────────────────────────────────────────────────────

        ConfigureWebApp(app);

        app.Run();
    }

    public static void ConfigureWebApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}