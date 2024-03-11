using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions
{
    public interface IPatientService
    {
        Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId);
        public Task<Result> GetPatientAppointments(string userId, string? status, PaginationFilter paginationFilter);

	}
}