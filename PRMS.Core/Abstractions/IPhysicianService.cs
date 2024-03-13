using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IPhysicianService
{
    public Task<IEnumerable<PhysicianDetailsDto>> GetDetails(string physicianId);
}

