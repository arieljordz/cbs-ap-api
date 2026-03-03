using CbsAp.Application.DTOs.Menus;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IMenuRepository
    {
        Task<MenuListDto> GetMenuDto(long roleID);
    }
}