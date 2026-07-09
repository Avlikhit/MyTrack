using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;

namespace MyTrack.Api.Controllers;

[ApiController]
[Route("api/pay-information")]
[Authorize]
public class PayInformationController : ControllerBase
{
    private readonly IPayInformationService _payInformationService;

    public PayInformationController(IPayInformationService payInformationService)
    {
        _payInformationService = payInformationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _payInformationService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _payInformationService.GetByIdAsync(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetByDateRangeAsync(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await _payInformationService.GetByDateRangeAsync(startDate, endDate);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreatePayInformationRequest request)
    {
        var result = await _payInformationService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        UpdatePayInformationRequest request)
    {
        var result = await _payInformationService.UpdateAsync(id, request);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var deleted = await _payInformationService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}