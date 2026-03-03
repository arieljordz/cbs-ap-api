using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class GoodsReceiptRepository : IGoodsReceiptRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GoodsReceiptRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GoodReceipt>> GetGoodsReceipts(long entityProfileId, long supplierInfoId, string? goodsReceiptNumber, bool? isActive)
        {
            var query = _dbContext.GoodsReceipts
                .AsNoTracking()
                .Include(gr => gr.EntityProfile)
                .Include(gr => gr.Supplier)
                .WhereIf(entityProfileId > 0, gr => gr.EntityProfileID == entityProfileId)
                .WhereIf(supplierInfoId > 0, gr => gr.SupplierInfoID == supplierInfoId)
                .WhereIf(!string.IsNullOrEmpty(goodsReceiptNumber), gr => gr.GoodsReceiptNumber.Contains(goodsReceiptNumber!))
                .WhereIf(isActive.HasValue, gr => gr.Active == isActive!.Value);

            return await query.ToListAsync();
        }

        public IQueryable<GoodReceipt> GetGoodsReceiptsAsQueryable()
        {
            return _dbContext.GoodsReceipts.AsQueryable();
        }
    }
}
