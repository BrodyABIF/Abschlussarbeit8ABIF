// MotorcycleWorkshop.API.Tests/RepairsControllerTests.cs

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleWorkshop.API.Commands;
using MotorcycleWorkshop.API.DTOs;
using MotorcycleWorkshop.API.Test;
using MotorcycleWorkshop.Infrastructure;
using MotorcycleWorkshop.model;
using Xunit;
using System;
using System.Net.Http;

public class RepairsControllerTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RepairsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;

        // Jedes Mal, wenn ein Testklasse-Instanz erzeugt wird,
        // wird erstmal die In-Memory-DB geleert und legen sie neu an:dot
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }

        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public void Dispose() => _client.Dispose();

    
    /// <summary>
    /// Überprüft den grundlegenden Abruf aller Reparaturen.
    /// Testet den erfolgreichen Response und das korrekte Content-Type Format.
    /// </summary>
    [Fact]
    public async Task GetRepairs_ReturnsSuccessAndCorrectContentType()
    {
        var response = await _client.GetAsync("/api/repairs");
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    
    /// <summary>
    /// Überprüft, ob der GET-Endpoint korrekt auf verschiedene ungültige GUID-Formate reagiert
    /// und einen BadRequest (400) mit passender Fehlermeldung zurückgibt.
    /// </summary>
    [Theory]
    [InlineData("invalid-guid")]
    [InlineData("12345678-1234-1234-1234-1234567890zz")] // ungültige Zeichen
    [InlineData("12345678123412341234123456789012")]      // fehlendes Format
    [InlineData("12345678-1234-1234-1234")]               // zu kurz
    public async Task GetRepair_WithInvalidGuid_ReturnsBadRequest(string invalidGuid)
    {
        var response = await _client.GetAsync($"/api/repairs/{invalidGuid}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var details = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(details);
        Assert.Equal("Invalid ID format", details.Title);
        Assert.Contains("must be a valid GUID in format", details.Detail);
    }

    
    
    /// <summary>
    /// Testet den Abruf einer nicht existierenden Reparatur.
    /// Erwartet wird ein NotFound-Response.
    /// </summary>
    [Fact]
    public async Task GetRepair_WithNonexistentGuid_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/repairs/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var details = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(details);
        Assert.Equal("Repair not found", details.Title);
    }

    
    /// <summary>
    /// Überprüft die erfolgreiche Erstellung einer Reparatur mit gültigen Daten.
    /// Validiert, ob die erstellte Reparatur die korrekten Beziehungen zu Kunde und Mechaniker enthält.
    /// </summary>
    [Fact]
    public async Task CreateRepair_WithValidData_ReturnsOk()
    {
        // Arrange: Erst Customer und Mechanic in DB anlegen
        var customer = Customer.Create(
            "Test Customer", 
            "Test Street", 
            "Test City", 
            "12345", 
            "123456789", 
            "test@example.com"
        );
        var mechanic = Mechanic.Create(
            "Test Mechanic",
            "Mech Street",
            "Mech City",
            "54321",
            "987654321",
            "mech@example.com",
            "Master",
            45m
        );

        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
            ctx.Customers.Add(customer);
            ctx.Mechanics.Add(mechanic);
            await ctx.SaveChangesAsync();
        }

        // Act: Repair erstellen
        var cmd = new CreateRepairCommand 
        { 
            CustomerId = customer.AlternateId,
            MechanicId = mechanic.AlternateId,
            RepairDate = DateTime.Today.AddDays(1),
            PartIds = new List<Guid>()
        };
        var response = await _client.PostAsJsonAsync("/api/repairs", cmd);

        // Assert
        response.EnsureSuccessStatusCode();
        var repair = await response.Content.ReadFromJsonAsync<RepairDetailDto>();
        Assert.NotNull(repair);
        Assert.NotNull(repair.Customer);
        Assert.NotNull(repair.Mechanic);
        Assert.Equal(customer.AlternateId, repair.Customer.AlternateId);
        Assert.Equal(mechanic.AlternateId, repair.Mechanic.AlternateId);
    }


    /// <summary>
    /// Testet die Erstellung einer Reparatur mit einem nicht existierenden Mechaniker.
    /// Erwartet wird ein NotFound-Response.
    /// </summary>
    [Fact]
    public async Task CreateRepair_WithoutMechanic_ReturnsNotFound()
    {
        // Arrange: Nur Customer anlegen
        var customer = Customer.Create(
            "Test Customer", 
            "Test Street", 
            "Test City", 
            "12345", 
            "123456789", 
            "test@example.com"
        );

        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
            ctx.Customers.Add(customer);
            await ctx.SaveChangesAsync();
        }

        // Act: Repair mit nicht-existentem Mechanic erstellen
        var cmd = new CreateRepairCommand 
        { 
            CustomerId = customer.AlternateId,
            MechanicId = Guid.NewGuid(), // Nicht-existente Mechanic ID
            RepairDate = DateTime.Today.AddDays(1),
            PartIds = new List<Guid>()
        };
        var response = await _client.PostAsJsonAsync("/api/repairs", cmd);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var details = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(details);
        Assert.Equal("Customer or Mechanic not found", details.Title);
    }
    

    /// <summary>
    /// Testet den Versuch, eine Reparatur mit zugehöriger Rechnung zu löschen.
    /// Erwartet wird ein BadRequest, da Reparaturen mit Rechnungen nicht gelöscht werden dürfen.
    /// </summary>
    [Fact]
    public async Task DeleteRepair_WithAssociatedInvoice_ReturnsBadRequest()
    {
        Repair repair;

        // 1) Seed Customer, Mechanic, Repair & Invoice in exakt dieser DB
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();

            // Kunde anlegen
            var customer = Customer.Create(
                "Max Mustermann",
                "Musterstraße 1",
                "Musterstadt",
                "12345",
                "0123456789",
                "max@example.com"
            );
            ctx.Customers.Add(customer);

            // Mechaniker anlegen
            var mechanic = Mechanic.Create(
                "John Doe",
                "Musterstraße 2",
                "Musterstadt",
                "12345",
                "0987654321",
                "john@example.com",
                "Master Mechanic",
                50m
                
            );
            ctx.Mechanics.Add(mechanic);

            await ctx.SaveChangesAsync();

            // Repair anlegen
            repair = Repair.Create(customer, mechanic, DateTime.Today.AddDays(1));
            ctx.Repairs.Add(repair);
            await ctx.SaveChangesAsync();

            // Invoice erzeugen
            var invoice = repair.GetRepairInvoice();
            ctx.Invoices.Add(invoice);
            await ctx.SaveChangesAsync();
        }

        // 2) den gleichen Client nutzen
        var response = await _client.DeleteAsync($"/api/repairs/{repair.AlternateId}");

        // 3) Erwartung: 400 BadRequest mit ProblemDetails.Title = "Cannot delete repair"
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var pd = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(pd);
        Assert.Equal("Cannot delete repair", pd.Title);
    }
    
    
    /// <summary>
    /// Testet das Hinzufügen und Entfernen von Ersatzteilen bei einer Reparatur.
    /// Prüft, ob nach dem Update genau das neue Teil (part2) der Reparatur zugeordnet ist
    /// und das alte Teil (part1) erfolgreich entfernt wurde.
    /// </summary>
    [Fact]
    public async Task UpdateRepair_WithPartsManagement_ModifiesPartsCorrectly()
    {
        // Arrange
        Repair repair;
        Guid part1Id, part2Id;
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
        
            var customer = Customer.Create(
                "Test Customer", 
                "Test Street", 
                "Test City", 
                "12345", 
                "123456789", 
                "test@example.com"
            );
            var mechanic = Mechanic.Create(
                "Test Mechanic",
                "Mech Street",
                "Mech City",
                "54321",
                "987654321",
                "mech@example.com",
                "Master",
                45m
            );
        
            // Verwende die vorhandenen Parts aus der Seed-Datenbank
            var parts = await ctx.Parts.Take(2).ToListAsync();
            Assert.Equal(2, parts.Count); // Stelle sicher, dass wir 2 Teile haben
            part1Id = parts[0].AlternateId;
            part2Id = parts[1].AlternateId;
        
            ctx.Customers.Add(customer);
            ctx.Mechanics.Add(mechanic);
            await ctx.SaveChangesAsync();

            repair = Repair.Create(customer, mechanic, DateTime.Today.AddDays(1));
            repair.AddPart(parts[0]); // Füge das erste Teil hinzu
            ctx.Repairs.Add(repair);
            await ctx.SaveChangesAsync();
        }

        var cmd = new UpdateRepairCommand 
        {
            PartIdsToAdd = new List<Guid> { part2Id },
            PartIdsToRemove = new List<Guid> { part1Id }
        };

        // Act
        var response = await _client.PatchAsync(
            $"/api/repairs/{repair.AlternateId}",
            JsonContent.Create(cmd));

        // Assert
        response.EnsureSuccessStatusCode();
    
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
            var updatedRepair = await ctx.Repairs
                .Include(r => r.UsedParts)
                .FirstAsync(r => r.AlternateId == repair.AlternateId);
        
            Assert.Single(updatedRepair.UsedParts);
            Assert.Equal(part2Id, updatedRepair.UsedParts.First().AlternateId);
        }
    }

    
    /// <summary>
    /// Überprüft die Filterung von Reparaturen der letzten Woche.
    /// Erwartet wird nur die neuere Reparatur (2 Tage alt) in der Antwort,
    /// während die ältere Reparatur (10 Tage alt) nicht enthalten sein soll.
    /// </summary>
    [Fact]
    public async Task GetRepairs_WithOnlyPastWeekTrue_ReturnsOnlyRecentRepairs()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
        
            var customer = Customer.Create(
                "Test Customer", 
                "Test Street", 
                "Test City", 
                "12345", 
                "123456789", 
                "test@example.com"
            );
            var mechanic = Mechanic.Create(
                "Test Mechanic",
                "Mech Street",
                "Mech City",
                "54321",
                "987654321",
                "mech@example.com",
                "Master",
                45m
            );
            ctx.Customers.Add(customer);
            ctx.Mechanics.Add(mechanic);
            await ctx.SaveChangesAsync();

            // Alte Reparatur (10 Tage alt)
            var oldRepair = Repair.Create(customer, mechanic, DateTime.Now.AddDays(-10));
            // Neue Reparatur (2 Tage alt)
            var newRepair = Repair.Create(customer, mechanic, DateTime.Now.AddDays(-2));
        
            ctx.Repairs.AddRange(oldRepair, newRepair);
            await ctx.SaveChangesAsync();
        }

        // Act
        var response = await _client.GetAsync("/api/repairs?onlyPastWeek=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var repairs = await response.Content.ReadFromJsonAsync<IEnumerable<RepairDto>>();
        Assert.NotNull(repairs);
        Assert.Single(repairs);
    }

    
    /// <summary>
    /// Testet die Validierung beim Update einer Reparatur mit einem ungültigen Datum (in der Vergangenheit).
    /// Erwartet wird ein BadRequest mit einer Validierungsfehlermeldung.
    /// </summary>
    [Fact]
    public async Task UpdateRepair_WithInvalidDate_ReturnsValidationError()
    {
        // Arrange
        Repair repair;
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
            var customer = Customer.Create(
                "Test Customer", 
                "Test Street", 
                "Test City", 
                "12345", 
                "123456789", 
                "test@example.com"
            );
            var mechanic = Mechanic.Create(
                "Test Mechanic",
                "Mech Street",
                "Mech City",
                "54321",
                "987654321",
                "mech@example.com",
                "Master",
                45m
            );
            ctx.Customers.Add(customer);
            ctx.Mechanics.Add(mechanic);
            await ctx.SaveChangesAsync();

            repair = Repair.Create(customer, mechanic, DateTime.Today.AddDays(1));
            ctx.Repairs.Add(repair);
            await ctx.SaveChangesAsync();
        }

        var cmd = new UpdateRepairCommand 
        { 
            RepairDate = DateTime.Today.AddDays(-1) // Datum in der Vergangenheit
        };

        // Act
        var response = await _client.PatchAsync(
            $"/api/repairs/{repair.AlternateId}",
            JsonContent.Create(cmd));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal("Validation Error", problemDetails.Title);
    }

    
    /// <summary>
    /// Testet das erfolgreiche Löschen einer Reparatur ohne zugehörige Rechnung.
    /// Überprüft, ob die Reparatur nach dem Löschen nicht mehr in der Datenbank existiert
    /// und der Server mit NoContent (204) antwortet.
    /// </summary>
    [Fact]
    public async Task DeleteRepair_WithoutInvoice_RemovesRepairAndReturnsNoContent()
    {
        // Arrange
        Repair repair;
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
        
            var customer = Customer.Create(
                "Test Customer", 
                "Test Street", 
                "Test City", 
                "12345", 
                "123456789", 
                "test@example.com"
            );
            var mechanic = Mechanic.Create(
                "Test Mechanic",
                "Mech Street",
                "Mech City",
                "54321",
                "987654321",
                "mech@example.com",
                "Master",
                45m
            );
            ctx.Customers.Add(customer);
            ctx.Mechanics.Add(mechanic);
            await ctx.SaveChangesAsync();

            repair = Repair.Create(customer, mechanic, DateTime.Today.AddDays(1));
            ctx.Repairs.Add(repair);
            await ctx.SaveChangesAsync();
        }

        // Act
        var response = await _client.DeleteAsync($"/api/repairs/{repair.AlternateId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    
        using (var scope = _factory.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<WorkshopDBContext>();
            Assert.False(await ctx.Repairs.AnyAsync(r => r.AlternateId == repair.AlternateId));
        }
    }
    
}