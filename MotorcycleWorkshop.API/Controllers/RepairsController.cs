using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MotorcycleWorkshop.API.Commands;
using MotorcycleWorkshop.API.DTOs;
using MotorcycleWorkshop.Infrastructure;
using MotorcycleWorkshop.model;

// TODO Controller mit 2x GET, POST, PATCH, DELETE (CRUD)

namespace MotorcycleWorkshop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepairsController : ControllerBase
{
    private readonly WorkshopDBContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateRepairCommand> _createValidator;
    private readonly IValidator<UpdateRepairCommand> _updateValidator;

    public RepairsController(
        WorkshopDBContext context,
        IMapper mapper,
        IValidator<CreateRepairCommand> createValidator,
        IValidator<UpdateRepairCommand> updateValidator)
    {
        _context = context;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepairDto>>> GetRepairs([FromQuery] bool onlyPastWeek = false)
    {
        var query = _context.Repairs.AsQueryable()
            .Include(r => r.Mechanic)
            .Include(r => r.Customer)
            .Include(r => r.UsedParts)
            .Where(r => !onlyPastWeek || r.RepairDate >= DateTime.Now.AddDays(-7));

        var repairs = await query.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<RepairDto>>(repairs));
    }
    
    
    [HttpGet("{guid}")]
    public async Task<ActionResult<RepairDetailDto>> GetRepair(string guid)
    {
        // Regulärer Ausdruck für eine GUID
        var guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");
    
        if (!guidRegex.IsMatch(guid))
        {
            return BadRequest(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext, // TODO RFC 9457
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid ID format",
                    detail: "The ID must be a valid GUID in format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"));
        }

        var guidId = Guid.Parse(guid); // Jetzt sicher, da RegEx bereits geprüft hat
        
        var repair = await _context.Repairs
            .Include(r => r.Mechanic)
            .Include(r => r.Customer)
            .Include(r => r.UsedParts)
            .FirstOrDefaultAsync(r => r.AlternateId == guidId);

        if (repair == null)  // TODO ProblemDetailsFactory
        {
            return NotFound(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext,
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Repair not found"));
        }

        return Ok(_mapper.Map<RepairDetailDto>(repair));
    }


    [HttpPost]
    public async Task<ActionResult<RepairDetailDto>> CreateRepair(CreateRepairCommand command)
    {
        var validationResult = await _createValidator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            // 1) Fehler aus FluentValidation in eine ModelStateDictionary übernehmen
            var ms = new ModelStateDictionary();
            foreach (var err in validationResult.Errors)
                ms.AddModelError(err.PropertyName, err.ErrorMessage);

            // 2) MVC erzeugt automatisch eine ValidationProblemDetails
            return ValidationProblem(ms);
        }



        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.AlternateId == command.CustomerId);
        var mechanic = await _context.Mechanics
            .FirstOrDefaultAsync(m => m.AlternateId == command.MechanicId);
        var parts = await _context.Parts
            .Where(p => command.PartIds.Contains(p.AlternateId))
            .ToListAsync();


        if (customer == null || mechanic == null)
        {
            return NotFound(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext,
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Customer or Mechanic not found"));

        }
        var repair = Repair.Create(customer, mechanic, command.RepairDate);


        foreach (var part in parts)
        {
            repair.AddPart(part);
        }

        _context.Repairs.Add(repair);
        await _context.SaveChangesAsync();
        

        return Ok(_mapper.Map<RepairDetailDto>(repair));
    }


    [HttpPatch("{id}")]
    public async Task<ActionResult<RepairDetailDto>> UpdateRepair(string id, UpdateRepairCommand command)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            return BadRequest(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid ID format",
                    detail: "The ID must be a valid GUID"));
        }

        var validationResult = await _updateValidator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation Error",
                detail: string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
    
            return BadRequest(problemDetails);
        }


        var repair = await _context.Repairs
            .Include(r => r.Mechanic)
            .Include(r => r.Customer)
            .Include(r => r.UsedParts)
            .FirstOrDefaultAsync(r => r.AlternateId == guidId);

        if (repair == null)
        {
            return NotFound(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext,
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Repair not found"));
        }

        if (command.RepairDate.HasValue)
        {
            repair.RepairDate = command.RepairDate.Value;
        }

        if (command.MechanicId.HasValue)
        {
            var mechanic = await _context.Mechanics
                .FirstOrDefaultAsync(m => m.AlternateId == command.MechanicId);
            if (mechanic == null)
            {
                return NotFound(
                    ProblemDetailsFactory.CreateProblemDetails(
                        HttpContext,
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Mechanic not found"));
            }
            repair.AssignMechanic(mechanic); 
        }


        if (command.PartIdsToAdd?.Any() == true)
        {
            var partsToAdd = await _context.Parts
                .Where(p => command.PartIdsToAdd.Contains(p.AlternateId))
                .ToListAsync();
            foreach (var part in partsToAdd)
            {
                repair.AddPart(part);
            }
        }

        if (command.PartIdsToRemove?.Any() == true)
        {
            var partsToRemove = repair.UsedParts
                .Where(p => command.PartIdsToRemove.Contains(p.AlternateId))
                .ToList();
            foreach (var part in partsToRemove)
            {
                repair.RemovePart(part.Id);
            }
        }

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<RepairDetailDto>(repair));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRepair(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            return BadRequest(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid ID format",
                    detail: "The ID must be a valid GUID"));
        }

        var repair = await _context.Repairs
            .Include(r => r.UsedParts)
            .FirstOrDefaultAsync(r => r.AlternateId == guidId);

        if (repair == null)
        {
            return NotFound(
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext,
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Repair not found"));
        }

        // Prüfe ob die Reparatur eine zugehörige Invoice hat
        if (await _context.Invoices.AnyAsync(i => i.RepairId == repair.Id))
        {
            return Problem(
                detail: "Cannot delete repair with associated invoice",
                title: "Cannot delete repair",
                statusCode: StatusCodes.Status400BadRequest);
        }

        _context.Repairs.Remove(repair);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}