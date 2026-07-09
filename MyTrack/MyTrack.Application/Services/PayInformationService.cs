using MyTrack.API.Models;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using MyTrack.Domain.Entities;
using MyTrack.Infrastructure.Repositories;

namespace MyTrack.Application.Services;

public class PayInformationService : IPayInformationService
{
    private readonly IPayInformationRepository _payInformationRepository;

    public PayInformationService(IPayInformationRepository payInformationRepository)
    {
        _payInformationRepository = payInformationRepository;
    }

    public async Task<PayInformationResponse> CreateAsync(CreatePayInformationRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var payInformation = new PayInformation
        {
            PayDate = request.PayDate,
            PayPeriodStart = request.PayPeriodStart,
            PayPeriodEnd = request.PayPeriodEnd,
            GrossPay = request.GrossPay,
            FederalTax = request.FederalTax,
            StateTax = request.StateTax,
            SocialSecurityTax = request.SocialSecurityTax,
            MedicareTax = request.MedicareTax,
            OtherDeductions = request.OtherDeductions,
            NetPay = request.NetPay,
            Notes = request.Notes,
            CreatedDateTime = DateTime.UtcNow
        };

        await _payInformationRepository.AddAsync(payInformation);

        return MapToResponse(payInformation);
    }

    public async Task<PayInformationResponse?> UpdateAsync(int id, UpdatePayInformationRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var existingPayInformation = await _payInformationRepository.GetByIdAsync(id);

        if (existingPayInformation is null)
        {
            return null;
        }

        existingPayInformation.PayDate = request.PayDate;
        existingPayInformation.PayPeriodStart = request.PayPeriodStart;
        existingPayInformation.PayPeriodEnd = request.PayPeriodEnd;
        existingPayInformation.GrossPay = request.GrossPay;
        existingPayInformation.FederalTax = request.FederalTax;
        existingPayInformation.StateTax = request.StateTax;
        existingPayInformation.SocialSecurityTax = request.SocialSecurityTax;
        existingPayInformation.MedicareTax = request.MedicareTax;
        existingPayInformation.OtherDeductions = request.OtherDeductions;
        existingPayInformation.NetPay = request.NetPay;
        existingPayInformation.Notes = request.Notes;

        await _payInformationRepository.UpdateAsync(existingPayInformation);

        return MapToResponse(existingPayInformation);
    }

    public async Task<PayInformationResponse?> GetByIdAsync(int id)
    {
        var payInformation = await _payInformationRepository.GetByIdAsync(id);

        return payInformation is null ? null : MapToResponse(payInformation);
    }

    public async Task<IEnumerable<PayInformationResponse>> GetAllAsync()
    {
        var payInformation = await _payInformationRepository.GetAllAsync();

        return payInformation.Select(MapToResponse);
    }

    public async Task<IEnumerable<PayInformationResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var payInformation = await _payInformationRepository.GetAllAsync();

        return payInformation
            .Where(pay => pay.PayDate.Date >= startDate.Date && pay.PayDate.Date <= endDate.Date)
            .Select(MapToResponse);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingPayInformation = await _payInformationRepository.GetByIdAsync(id);

        if (existingPayInformation is null)
        {
            return false;
        }

        await _payInformationRepository.DeleteAsync(id);

        return true;
    }

    private static PayInformationResponse MapToResponse(PayInformation payInformation)
    {
        return new PayInformationResponse
        {
            Id = payInformation.Id,
            PayDate = payInformation.PayDate,
            PayPeriodStart = payInformation.PayPeriodStart,
            PayPeriodEnd = payInformation.PayPeriodEnd,
            GrossPay = payInformation.GrossPay,
            FederalTax = payInformation.FederalTax,
            StateTax = payInformation.StateTax,
            SocialSecurityTax = payInformation.SocialSecurityTax,
            MedicareTax = payInformation.MedicareTax,
            OtherDeductions = payInformation.OtherDeductions,
            NetPay = payInformation.NetPay,
            Notes = payInformation.Notes,
            CreatedDateTime = payInformation.CreatedDateTime
        };
    }
}