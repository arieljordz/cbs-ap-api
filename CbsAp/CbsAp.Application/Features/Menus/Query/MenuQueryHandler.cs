using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Menus;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Menus.Query
{
    public class MenuQueryHandler : IQueryHandler<MenuQuery, ResponseResult<List<MenuListDto>>>
    {
        private readonly IMenuRepository _menuRepository;

        public MenuQueryHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<ResponseResult<List<MenuListDto>>> Handle(MenuQuery request, CancellationToken cancellationToken)
        {
            var result = await _menuRepository.GetMenuDto(request.roleID);
            foreach (var menu in result.items)
            {
                if (menu.items != null && !menu.items.Any())
                {
                    menu.items = null;
                }
            }

            var response = new List<MenuListDto> { result };
            return ResponseResult<List<MenuListDto>>.OK(response);
        }
    }
}