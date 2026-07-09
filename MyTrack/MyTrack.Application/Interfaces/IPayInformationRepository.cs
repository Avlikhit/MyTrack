using MyTrack.API.Models;
using MyTrack.Domain.Entities;

namespace MyTrack.Infrastructure.Repositories;

public interface IPayInformationRepository
{
    Task<List<PayInformation>> GetAllAsync();

    Task<PayInformation?> GetByIdAsync(int id);

    Task AddAsync(PayInformation payInformation);

    Task UpdateAsync(PayInformation payInformation);

    Task DeleteAsync(int id);
}