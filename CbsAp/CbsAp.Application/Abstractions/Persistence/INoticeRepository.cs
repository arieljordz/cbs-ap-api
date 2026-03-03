using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Dashboard;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface INoticeRepository
    {
        Task<IEnumerable<Notice>> GetAllNotice();

    }
}