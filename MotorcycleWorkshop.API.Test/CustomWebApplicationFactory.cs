using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleWorkshop.Infrastructure;
using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop.API.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1) Alten WorkshopDBContext entfernen
                //    (AddDbContext registriert sowohl DbContextOptions<WorkshopDBContext> als auch WorkshopDBContext selbst)
                var optDescriptor = services.FirstOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<WorkshopDBContext>));
                if (optDescriptor != null) services.Remove(optDescriptor);

                var ctxDescriptor = services.FirstOrDefault(d =>
                    d.ServiceType == typeof(WorkshopDBContext));
                if (ctxDescriptor != null) services.Remove(ctxDescriptor);

                // 2) Neuen InMemory‐Context registrieren und TransactionWarning unterdrücken
                services.AddDbContext<WorkshopDBContext>(opts =>
                {
                    opts.UseInMemoryDatabase("TestDb");
                    opts.ConfigureWarnings(w =>
                        w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    opts.EnableSensitiveDataLogging();
                });

                // 3) Datenbank anlegen und seed‐daten einfügen
                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
                db.Database.EnsureCreated();

                // Seed
                var customer = Customer.Create(
                    "Test Customer", "Test Street", "Test City",
                    "12345", "123-456-789", "test@example.com");
                db.Customers.Add(customer);

                var mechanic = Mechanic.Create(
                    "Test Mechanic", "Mech Street", "Mech City",
                    "54321", "987-654-321", "mech@example.com",
                    "Master", 50m);
                db.Mechanics.Add(mechanic);

                var p1 = (Part)Activator.CreateInstance(typeof(Part), true)!;
                p1.Name = "Bremsscheibe"; p1.Price = 199.99m;
                var p2 = (Part)Activator.CreateInstance(typeof(Part), true)!;
                p2.Name = "Luftfilter";    p2.Price = 29.99m;
                db.Parts.AddRange(p1, p2);

                var repair = Repair.Create(customer, mechanic, DateTime.Today.AddDays(1));
                repair.AddPart(p1);
                repair.AddPart(p2);
                db.Repairs.Add(repair);

                var bike = (Motorcycle)Activator.CreateInstance(typeof(Motorcycle), true)!;
                bike.Model = "Test Bike";
                bike.Year = 2023;
                bike.UpdateMileage(1000m);
                bike.Owner = customer;
                db.Motorcycles.Add(bike);

                db.SaveChanges();
            });
        }
    }
}
