using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Domain.Entities.Dashboard;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.Dashboard
{
    public class NoticeMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<Notice, NoticeDTO>()
             .MapWith(notice => new NoticeDTO
             {
                 NoticeID = notice.NoticeID,
                 Heading = notice.Heading ?? "",
                 Message = notice.Message ?? "",
                 MessageDate = notice.LastUpdatedDate == null ? notice.CreatedDate.Value.LocalDateTime : notice.LastUpdatedDate.Value.LocalDateTime,
                 UserName = notice.CreatedBy ?? "",
                 IsNew = (notice.LastUpdatedDate == null ? notice.CreatedDate.Value.LocalDateTime : notice.LastUpdatedDate.Value.LocalDateTime).Date==DateTime.Today
             });

        }

    }
}