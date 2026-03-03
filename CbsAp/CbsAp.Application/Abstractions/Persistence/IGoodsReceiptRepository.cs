using CbsAp.Domain.Entities.GoodReceipts;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IGoodsReceiptRepository
    {
        Task<IEnumerable<GoodReceipt>> GetGoodsReceipts(long entityProfileId, long supplierInfoId, string? goodsReceiptNumber, bool? isActive);

        IQueryable<GoodReceipt> GetGoodsReceiptsAsQueryable();
    }
}
