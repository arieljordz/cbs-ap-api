using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Menus;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public MenuRepository(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<MenuListDto> GetMenuDto(long roleID)
        {
            var query = await _dbcontext.Menus
            .Where(m => m.Operation.ApplyOperationIn == "menu")
            .Where(m => m.Operation.PermissionGroups.Any(pg =>
                pg.Access &&

                _dbcontext.RolePermissionGroups.Any(rpg =>
                    rpg.PermissionID == pg.PermissionID &&
                    rpg.RoleID == roleID)))
            .Include(m => m.MenuItems)
            .Select(m => new MenuDto
            {
                Label = m.Label,
                Icon = m.Icon,
                RouterLink = new List<string>() { m.RouterLink, },
                items = m.MenuItems
                    .Select(mi => new MenuItemDto
                    {
                        Label = mi.Label,
                        Icon = mi.Icon,
                        RouterLink = new List<string>() { mi.RouterLink }
                    })
                    .ToList()
            })
            .ToListAsync();
            var query2 = _dbcontext.Menus
            .Where(m => m.Operation.ApplyOperationIn == "menu")
            .Where(m => m.Operation.PermissionGroups.Any(pg =>
                pg.Access &&

                _dbcontext.RolePermissionGroups.Any(rpg =>
                    rpg.PermissionID == pg.PermissionID &&
                    rpg.RoleID == roleID)))
            .Include(m => m.MenuItems);
            string sql = query2.ToQueryString();

            var response = new MenuListDto { items = query };

            return response;
        }
    }
}