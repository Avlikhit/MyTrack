using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines application operations for managing pay information.
/// </summary>
public interface IPayInformationService
{
    Task<PayInformationResponse> CreateAsync(CreatePayInformationRequest request);

    Task<PayInformationResponse?> UpdateAsync(int id, UpdatePayInformationRequest request);

    Task<PayInformationResponse?> GetByIdAsync(int id);

    Task<IEnumerable<PayInformationResponse>> GetAllAsync();

    Task<IEnumerable<PayInformationResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<bool> DeleteAsync(int id);
}