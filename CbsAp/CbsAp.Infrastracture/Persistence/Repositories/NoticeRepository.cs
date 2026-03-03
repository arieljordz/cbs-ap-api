using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Domain.Entities.Dashboard;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class NoticeRepository : INoticeRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public NoticeRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Notice>> GetAllNotice()
        {
            var notices = await _dbcontext.Notices
                .AsNoTracking()
                .OrderByDescending(x => x.NoticeID).ToListAsync();
            return notices?? Enumerable.Empty<Notice>(); 
        }

    }
}