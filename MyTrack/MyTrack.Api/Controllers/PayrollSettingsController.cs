using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;

namespace MyTrack.API.Controllers;

/// <summary>
/// Controller for payroll settings.
/// </summary>
[ApiController]
[Route("api/payroll-settings")]
[Authorize]
public class PayrollSettingsController : ControllerBase
{
    private readonly IPayrollSettingsService _payrollSettingsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PayrollSettingsController"/> class.
    /// </summary>
    public PayrollSettingsController(IPayrollSettingsService payrollSettingsService)
    {
        _payrollSettingsService = payrollSettingsService;
    }

    /// <summary>
    /// Gets payroll settings.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _payrollSettingsService.GetAsync();
        return Ok(result);
    }

    /// <summary>
    /// Updates payroll settings.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdatePayrollSettingsRequest request)
    {
        var result = await _payrollSettingsService.UpdateAsync(request);
        return Ok(result);
    }
}